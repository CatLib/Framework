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

using CatLib.API.Socket;
using System;
using System.Net;
using System.Net.Sockets;

namespace CatLib.Socket
{
    /// <summary>
    /// Tcp连接器
    /// </summary>
    internal sealed class TcpConnector : ISocket, IDisposable
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
        /// 客户端
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// 链接状态
        /// </summary>
        private enum Status
        {
            /// <summary>
            /// 初始化
            /// </summary>
            Initial = 1,

            /// <summary>
            /// 连接中
            /// </summary>
            Connecting = 2,

            /// <summary>
            /// 建立链接
            /// </summary>
            Establish = 3,

            /// <summary>
            /// 关闭
            /// </summary>
            Closed = 4,
        }

        /// <summary>
        /// Ip地址
        /// </summary>
        private string remoteHostname;

        /// <summary>
        /// 端口
        /// </summary>
        private int remotePort;

        /// <summary>
        /// Tcp客户端
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// 链接状态
        /// </summary>
        private Status status;

        /// <summary>
        /// 网络链接基础数据流
        /// </summary>
        private NetworkStream networkStream;

        /// <summary>
        /// 接收缓冲区Buffer
        /// </summary>
        private byte[] receiveBuffer;

        /// <summary>
        /// 事件发射器
        /// </summary>
        private readonly Emmiter emmiter;

        /// <summary>
        /// 构建一个Tcp连接器
        /// </summary>
        /// <param name="hostname">IP地址</param>
        /// <param name="port">链接端口</param>
        public TcpConnector(string hostname, int port)
        {
            remoteHostname = hostname;
            remotePort = port;
            status = Status.Initial;
            emmiter = new Emmiter();
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
        /// <param name="port">服务器端口</param>
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
        /// 异步发送
        /// </summary>
        /// <param name="data">发送数据</param>
        /// <returns>异步等待接口</returns>
        public IAwait Send(byte[] data)
        {
            Guard.Requires<InvalidOperationException>(status == Status.Establish);

            var asyncResult = new InternalRuntime
            {
                Result = data
            };
            networkStream.BeginWrite(data, 0, data.Length, OnSendCallBack, asyncResult);
            return asyncResult;
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
        /// 释放时
        /// </summary>
        public void Dispose()
        {
            if (status == Status.Closed)
            {
                return;
            }

            if (networkStream != null)
            {
                networkStream.Close();
            }

            if (client != null)
            {
                client.Close();
            }

            status = Status.Closed;
            Trigger(SocketEvents.Closed, this);
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
                client = Client ?? MakeDefaultTcpClient();
                client.BeginConnect(ipAddress, remotePort, OnConnectComplete, payload);
            }
            catch (Exception ex)
            {
                ThrowException(payload, ex);
            }
        }
        
        /// <summary>
        /// 构建一个默认的TcpClient
        /// </summary>
        /// <returns>TcpClient</returns>
        private TcpClient MakeDefaultTcpClient()
        {
            var client = new TcpClient
            {
                ReceiveBufferSize = 4096,
                ReceiveTimeout = 0,
                SendBufferSize = 4096,
                SendTimeout = 0,
                NoDelay = true
            };

            //client.Client.IOControl(IOControlCode.KeepAliveValues, KeepAlive(1, 15000, 5000), null);

            return client;
        }

        /// <summary>
        /// Tcp保活
        /// </summary>
        /// <param name="active">是否启动</param>
        /// <param name="keepAliveIdle">如该连接在指定时间内没有任何数据往来,则进行探测</param>
        /// <param name="keepAliveInterval">保活探测间隔</param>
        /// <returns>保活数据</returns>
        private byte[] KeepAlive(int active, int keepAliveIdle, int keepAliveInterval)
        {
            var buffer = new byte[12];
            BitConverter.GetBytes(active).CopyTo(buffer, 0);
            BitConverter.GetBytes(keepAliveIdle).CopyTo(buffer, 4);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
            return buffer;
        }

        /// <summary>
        /// 当链接完成时
        /// </summary>
        /// <param name="result">异步执行结果</param>
        private void OnConnectComplete(IAsyncResult result)
        {
            var payload = (InternalRuntime)result.AsyncState;
            try
            {
                client.EndConnect(result);

                networkStream = client.GetStream();
                receiveBuffer = new byte[client.ReceiveBufferSize];
                networkStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnReceiveCallBack, payload);

                status = Status.Establish;
                payload.Result = true;
                payload.IsDone = true;
                Trigger(SocketEvents.Connect, this);
            }
            catch (Exception ex)
            {
                ThrowException(payload, ex);
            }
        }

        /// <summary>
        /// 当存在数据时回调
        /// </summary>
        /// <param name="result">异步执行结果</param>
        private void OnReceiveCallBack(IAsyncResult result)
        {
            var payload = (InternalRuntime)result.AsyncState;
            try
            {
                var read = networkStream.EndRead(result);
                if (read <= 0)
                {
                    Dispose();
                    return;
                }

                var callbackBuff = new byte[read];
                Buffer.BlockCopy(receiveBuffer, 0, callbackBuff, 0, read);
                Trigger(SocketEvents.Message, callbackBuff);

                Array.Clear(receiveBuffer, 0, read);
                networkStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnReceiveCallBack, payload);
            }
            catch (Exception)
            {
                if (Connected)
                {
                    Dispose();
                }
            }
        }

        /// <summary>
        /// 当发送完成时
        /// </summary>
        /// <param name="result">异步执行结果</param>
        private void OnSendCallBack(IAsyncResult result)
        {
            var payload = (InternalRuntime)result.AsyncState;
            try
            {
                networkStream.EndWrite(result);

                var data = payload.Result;
                payload.Result = true;
                payload.IsDone = true;
                Trigger(SocketEvents.Sent, data);
            }
            catch (Exception ex)
            {
                ThrowException(payload, ex);
            }
        }

        /// <summary>
        /// 触发异常
        /// </summary>
        /// <param name="runtime">运行时</param>
        /// <param name="ex">触发的异常</param>
        private void ThrowException(InternalRuntime runtime, Exception ex)
        {
            runtime.Result = ex;
            runtime.IsDone = true;
            Trigger(SocketEvents.Error, ex);
            Dispose();
        }
    }
}
