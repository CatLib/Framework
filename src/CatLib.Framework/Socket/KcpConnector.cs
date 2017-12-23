/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using CatLib.API.Socket;
using CatLib._3rd.Kcp;

namespace CatLib.Socket
{
    /// <summary>
    /// Kcp连接器
    /// </summary>
    internal sealed class KcpConnector : ISocket, ITick
    {
        /// <summary>
        /// 内部运行时
        /// </summary>
        private class InternalRuntime : IAwait
        {
            /// <summary>
            /// 是否准备完成
            /// </summary>
            public bool IsDone { get; internal set; }

            /// <summary>
            /// 实现
            /// </summary>
            public object Result { get; internal set; }
        }

        /// <summary>
        /// 发送窗口大小
        /// </summary>
        public int SendWindow { get; set; }

        /// <summary>
        /// 接受窗口大小
        /// </summary>
        public int RecvWindow { get; set; }

        /// <summary>
        /// 无延迟
        /// </summary>
        public bool NoDelay { get; set; }

        /// <summary>
        /// 内部更新器时间
        /// </summary>
        public int IntervalTimer { get; set; }

        /// <summary>
        /// 重发频率（0禁止，1快速重发）
        /// </summary>
        public int Resend { get; set; }

        /// <summary>
        /// 堵塞控制
        /// </summary>
        public bool CongestionControl { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public UdpClient Client { get; set; }

        /// <summary>
        /// kcp异常字典
        /// </summary>
        private readonly Dictionary<int, Exception> kcpExceptionDict = new Dictionary<int, Exception>
        {
            { int.MinValue , new RuntimeException("Undefiend Exception") },
            { -1,  new ArgumentOutOfRangeException("data", "Send data length is zero")},
            { -2,  new ArgumentOutOfRangeException("data", "Send data is large than mss")},
        };

        /// <summary>
        /// Kcp链接状态
        /// </summary>
        private enum Status
        {
            /// <summary>
            /// 初始化
            /// </summary>
            Initial = 1,

            /// <summary>
            /// 链接中
            /// </summary>
            Connecting = 2,

            /// <summary>
            /// 维持链接
            /// </summary>
            Establish = 3,

            /// <summary>
            /// 断开链接
            /// </summary>
            Closed = 4,
        }

        /// <summary>
        /// 状态
        /// </summary>
        private Status status;

        /// <summary>
        /// Udp客户端
        /// </summary>
        private UdpClient client;

        /// <summary>
        /// 事件发射器
        /// </summary>
        private readonly Emmiter emmiter;

        /// <summary>
        /// 收到的数据的网络断点(来源)
        /// </summary>
        private IPEndPoint listenEndPoint;

        /// <summary>
        /// 服务器的网络端点
        /// </summary>
        private IPEndPoint serviceEndPoint;

        /// <summary>
        /// Ip地址
        /// </summary>
        private string remoteHostname;

        /// <summary>
        /// 端口
        /// </summary>
        private int remotePort;

        /// <summary>
        /// Kcp
        /// </summary>
        private KCP kcp;

        /// <summary>
        /// 开关队列
        /// </summary>
        private readonly SwitchQueue<byte[]> receiveQueue = new SwitchQueue<byte[]>(128);

        /// <summary>
        /// 下一次刷新时间
        /// </summary>
        private uint nextUpdateTime;

        /// <summary>
        /// 会话Id
        /// </summary>
        private readonly uint conv;

        /// <summary>
        /// Kcp连接器
        /// </summary>
        /// <param name="conv">会话</param>
        /// <param name="hostname">IP地址</param>
        /// <param name="port">链接端口</param>
        public KcpConnector(uint conv, string hostname, int port)
        {
            status = Status.Initial;
            emmiter = new Emmiter();
            this.conv = conv;
            remoteHostname = hostname;
            remotePort = port;
            SendWindow = 128;
            RecvWindow = 128;
            NoDelay = true;
            IntervalTimer = 10;
            Resend = 2;
            CongestionControl = true;
        }

        /// <summary>
        /// 是否已经链接远端主机
        /// </summary>
        public bool Connected
        {
            get { return status == Status.Establish; }
        }

        /// <summary>
        /// 建立链接
        /// </summary>
        /// <returns>异步等待接口</returns>
        public IAwait Connect()
        {
            return Connect(remoteHostname, remotePort);
        }

        /// <summary>
        /// 建立链接
        /// </summary>
        /// <param name="hostname">服务器地址</param>
        /// <param name="port">端口</param>
        /// <returns>异步等待接口</returns>
        public IAwait Connect(string hostname, int port)
        {
            if (status != Status.Initial && status != Status.Closed)
            {
                throw new RuntimeException("Current statu [" + status + "] can not connect");
            }

            remoteHostname = hostname;
            remotePort = port;

            status = Status.Connecting;
            var asyncResult = new InternalRuntime();
            Dns.BeginGetHostAddresses(remoteHostname, OnDnsGetHostAddressesComplete, asyncResult);
            return asyncResult;
        }

        /// <summary>
        /// 注册一条事件
        /// </summary>
        /// <param name="socketEvent">Socket事件</param>
        /// <param name="callback">回调</param>
        public void On(SocketEvents socketEvent, Action<object> callback)
        {
            emmiter.On(socketEvent, callback);
        }

        /// <summary>
        /// 反注册一条事件
        /// </summary>
        /// <param name="socketEvent">Socket事件</param>
        /// <param name="callback">回调</param>
        public void Off(SocketEvents socketEvent, Action<object> callback)
        {
            emmiter.Off(socketEvent, callback);
        }

        /// <summary>
        /// 触发一个Socket事件
        /// </summary>
        /// <param name="socketEvent">Socket事件</param>
        /// <param name="payload">载荷</param>
        public void Trigger(SocketEvents socketEvent, object payload)
        {
            emmiter.Trigger(socketEvent, payload);
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="data">发送数据</param>
        /// <returns>异步等待接口</returns>
        public IAwait Send(byte[] data)
        {
            Guard.Requires<InvalidOperationException>(status == Status.Establish);
            Guard.Requires<ArgumentNullException>(data != null);
            var code = kcp.Send(data);

            if (code < 0)
            {
                Exception ex;
                if (!kcpExceptionDict.TryGetValue(code, out ex))
                {
                    ex = kcpExceptionDict[int.MinValue];
                }
                Trigger(SocketEvents.Error, ex);
            }
            else
            {
                Trigger(SocketEvents.Sent, data);
            }

            return new InternalRuntime
            {
                Result = code >= 0,
                IsDone = true
            };
        }

        /// <summary>
        /// 断开链接
        /// </summary>
        public void Disconnect()
        {
            if (status == Status.Closed)
            {
                return;
            }
            Trigger(SocketEvents.Disconnect, this);
            Dispose();
        }

        /// <summary>
        /// 更新Kcp
        /// </summary>
        public void Tick()
        {
            Update(Clock());
        }

        /// <summary>
        /// 释放时
        /// </summary>
        public void Dispose()
        {
            if (status == Status.Closed)
            {
                return;
            }

            if (client != null)
            {
                client.Close();
            }
            status = Status.Closed;
            Trigger(SocketEvents.Closed, this);
        }

        /// <summary>
        /// 时钟
        /// </summary>
        /// <returns>时钟</returns>
        private static uint Clock()
        {
            return (uint)(Convert.ToInt64(DateTime.UtcNow.Subtract(SystemTime.UtcTime).TotalMilliseconds) & 0xffffffff);
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <param name="current">时钟</param>
        private void Update(uint current)
        {
            if (kcp == null)
            {
                return;
            }

            if (!ProcessReceiveQueue() && current < nextUpdateTime)
            {
                return;
            }

            kcp.Update(current);
            nextUpdateTime = kcp.Check(current);
        }

        /// <summary>
        /// 执行接收队列
        /// </summary>
        /// <returns>是否需要更新</returns>
        private bool ProcessReceiveQueue()
        {
            receiveQueue.Switch();
            var needUpdate = false;
            while (!receiveQueue.Empty())
            {
                kcp.Input(receiveQueue.Pop());
                needUpdate = true;

                for (var size = kcp.PeekSize(); size > 0; size = kcp.PeekSize())
                {
                    var buffer = new byte[size];
                    if (kcp.Recv(buffer) > 0)
                    {
                        Trigger(SocketEvents.Message, buffer);
                    }
                }
            }
            return needUpdate;
        }

        /// <summary>
        /// 根据Dns获取到Ip地址后
        /// </summary>
        /// <param name="result">异步执行结果</param>
        private void OnDnsGetHostAddressesComplete(IAsyncResult result)
        {
            var payload = (InternalRuntime)result.AsyncState;

            try
            {
                var ipAddress = Dns.EndGetHostAddresses(result);

                client = Client ?? new UdpClient();

                serviceEndPoint = new IPEndPoint(ipAddress[0], remotePort);
                client.Connect(serviceEndPoint);
                InitKcp(conv);
                client.BeginReceive(OnReceiveCallBack, null);

                status = Status.Establish;
                Trigger(SocketEvents.Connect, this);
                payload.Result = true;
                payload.IsDone = true;
            }
            catch (Exception ex)
            {
                payload.Result = ex;
                payload.IsDone = true;
                Trigger(SocketEvents.Error, ex);
                Dispose();
            }
        }

        /// <summary>
        /// 初始化Kcp
        /// </summary>
        /// <param name="conv">会话</param>
        private void InitKcp(uint conv)
        {
            kcp = new KCP(conv, (buf, size) =>
            {
                client.Send(buf, size);
            });

            kcp.NoDelay(NoDelay ? 1 : 0, IntervalTimer, Resend, CongestionControl ? 1 : 0);
            kcp.WndSize(SendWindow, RecvWindow);
        }

        /// <summary>
        /// 当接收到回调
        /// </summary>
        /// <param name="result">结果</param>
        private void OnReceiveCallBack(IAsyncResult result)
        {
            try
            {
                var receiveBytes = (listenEndPoint == null) ?
                    client.Receive(ref listenEndPoint) :
                    client.EndReceive(result, ref listenEndPoint);

                if (receiveBytes == null || receiveBytes.Length <= 0)
                {
                    Dispose();
                    return;
                }

                receiveQueue.Push(receiveBytes);
                client.BeginReceive(OnReceiveCallBack, null);
            }
            catch (Exception)
            {
                Dispose();
            }
        }
    }
}
