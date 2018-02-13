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
using CatLib.API.Network;
using CatLib.API.Socket;

namespace CatLib.Network
{
    /// <summary>
    /// 网络频道
    /// </summary>
    public sealed class NetworkChannel : INetworkChannel , ITick
    {
        /// <summary>
        /// 内部等待接口
        /// </summary>
        private class InternalAwait : IAwait
        {
            /// <summary>
            /// 是否准备完成
            /// </summary>
            public bool IsDone { get; set; }

            /// <summary>
            /// 实现
            /// </summary>
            public object Result { get; set; }
        }

        /// <summary>
        /// 网络名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 频道使用的套接字
        /// </summary>
        private readonly ISocket socket;

        /// <summary>
        /// 基础套接字
        /// </summary>
        public ISocket Socket
        {
            get { return socket; }
        }

        /// <summary>
        /// 拆包打包器
        /// </summary>
        private readonly IPacker packer;

        /// <summary>
        /// 当网络链接完成时
        /// </summary>
        public event Action<INetworkChannel> OnConnected;

        /// <summary>
        /// 当网络出现异常时
        /// </summary>
        public event Action<INetworkChannel, Exception> OnError;

        /// <summary>
        /// 当收到数据包时
        /// </summary>
        public event Action<INetworkChannel, object> OnMessage;

        /// <summary>
        /// 当网络断开链接
        /// </summary>
        public event Action<INetworkChannel> OnDisconnect;

        /// <summary>
        /// 当网络链接关闭时
        /// </summary>
        public event Action<INetworkChannel, Exception> OnClosed;

        /// <summary>
        /// 当数据发送完成
        /// </summary>
        public event Action<INetworkChannel, byte[]> OnSent;

        /// <summary>
        /// 当丢失心跳时
        /// </summary>
        public event Action<INetworkChannel, int> OnMissHeartBeat;

        /// <summary>
        /// 接收状态
        /// </summary>
        private readonly ReceiveState receiveState;

        /// <summary>
        /// 心跳包状态
        /// </summary>
        private readonly HeartBeatState heartBeatState;

        /// <summary>
        /// 最后引发的异常
        /// </summary>
        private Exception lastException;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 网络频道
        /// </summary>
        /// <param name="socket">网络套接字</param>
        /// <param name="packer">拆包打包器</param>
        /// <param name="name">频道名字</param>
        public NetworkChannel(ISocket socket, IPacker packer, string name)
        {
            Guard.Requires<ArgumentNullException>(socket != null);
            Guard.Requires<ArgumentNullException>(packer != null);
            Guard.NotEmptyOrNull(name, "name");

            Name = name;
            this.socket = socket;
            this.packer = packer;
            receiveState = new ReceiveState(packer);
            heartBeatState = new HeartBeatState(0);
            lastException = null;

            heartBeatState.OnMissHeartBeat += (count) =>
            {
                if (OnMissHeartBeat != null)
                {
                    OnMissHeartBeat.Invoke(this, count);
                }
            };

            socket.On(SocketEvents.Connect, OnSocketConnect);
            socket.On(SocketEvents.Error, OnSocketError);
            socket.On(SocketEvents.Closed, OnSocketClosed);
            socket.On(SocketEvents.Message, OnSocketMessage);
            socket.On(SocketEvents.Disconnect, OnSocketDisconnect);
            socket.On(SocketEvents.Sent, OnSocketSent);
        }

        /// <summary>
        /// 设定心跳包(毫秒)
        /// </summary>
        /// <param name="intervalMillisecond">心跳包间隔（毫秒）</param>
        public void SetHeartBeat(int intervalMillisecond)
        {
            lock (heartBeatState)
            {
                heartBeatState.SetInterval(intervalMillisecond);
            }
        }

        /// <summary>
        /// 滴答
        /// </summary>
        /// <param name="elapseMillisecond">滴答间经过的毫秒</param>
        public void Tick(int elapseMillisecond)
        {
            if (socket.Connected)
            {
                lock (heartBeatState)
                {
                    heartBeatState.Tick(elapseMillisecond);
                }
            }
        }

        /// <summary>
        /// 链接到服务器
        /// </summary>
        public IAwait Connect()
        {
            lock (syncRoot)
            {
                return socket.Connect();
            }
        }

        /// <summary>
        /// 链接到服务器
        /// </summary>
        /// <param name="hostname">服务器名</param>
        /// <param name="port">端口</param>
        public IAwait Connect(string hostname, int port)
        {
            lock (syncRoot)
            {
                return socket.Connect(hostname, port);
            }
        }

        /// <summary>
        /// 释放时
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// 断开链接
        /// </summary>
        public void Disconnect()
        {
            lock (syncRoot)
            {
                socket.Disconnect();
            }
        }

        /// <summary>
        /// 断开链接,并给定一个断开链接的异常
        /// </summary>
        /// <param name="ex">由于什么异常导致断开链接</param>
        public void Disconnect(Exception ex)
        {
            lock (syncRoot)
            {
                lastException = ex;
                socket.Trigger(SocketEvents.Error, ex);
                Disconnect();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="packet">网络数据包</param>
        public IAwait Send(object packet)
        {
            Exception ex;
            var data = packer.Encode(packet, out ex);

            if (ex != null)
            {
                OnSocketError(ex);
                throw ex;
            }

            return Send(data);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        private IAwait Send(byte[] data)
        {
            if (data == null || data.Length <= 0)
            {
                return new InternalAwait
                {
                    IsDone = true,
                    Result = true,
                };
            }

            lock (syncRoot)
            {
                return socket.Send(data);
            }
        }

        /// <summary>
        /// 当链接完成时
        /// </summary>
        /// <param name="obj">null</param>
        private void OnSocketConnect(object obj)
        {
            lastException = null;
            if (OnConnected != null)
            {
                OnConnected.Invoke(this);
            }
        }

        /// <summary>
        /// 当异常时
        /// </summary>
        /// <param name="obj">异常</param>
        private void OnSocketError(object obj)
        {
            lastException = obj as Exception;
            if (OnError != null)
            {
                OnError.Invoke(this, (Exception)obj);
            }
        }

        /// <summary>
        /// 当断开连接时
        /// </summary>
        /// <param name="obj">socket对象</param>
        private void OnSocketDisconnect(object obj)
        {
            if (OnDisconnect != null)
            {
                OnDisconnect.Invoke(this);
            }
        }

        /// <summary>
        /// 当发送完成
        /// </summary>
        /// <param name="obj">发送的数据</param>
        private void OnSocketSent(object obj)
        {
            if (OnSent != null)
            {
                var data = (byte[])obj;
                OnSent.Invoke(this, data);
            }
        }

        /// <summary>
        /// 当关闭时
        /// </summary>
        /// <param name="obj">socket对象</param>
        private void OnSocketClosed(object obj)
        {
            lock (heartBeatState)
            {
                heartBeatState.Reset();
            }
            receiveState.Reset();
            if (OnClosed != null)
            {
                OnClosed.Invoke(this, lastException);
            }
        }

        /// <summary>
        /// 当接收到消息
        /// </summary>
        /// <param name="obj">消息内容</param>
        private void OnSocketMessage(object obj)
        {
            var data = (byte[])obj;
            Exception ex;

            var packets = receiveState.Input(data, out ex);

            if (ex != null)
            {
                OnSocketError(ex);
                Disconnect();
                return;
            }

            if (packets == null)
            {
                return;
            }

            lock (heartBeatState)
            {
                heartBeatState.Reset();
            }

            if (OnMessage != null)
            {
                foreach (var packet in packets)
                {
                    OnMessage.Invoke(this, packet);
                }
            }
        }
    }
}
