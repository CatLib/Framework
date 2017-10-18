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

using CatLib.API.Network;
using System;
using System.Collections.Generic;

namespace CatLib.Network
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    internal sealed class NetworkManager : SingleManager<INetworkChannel>, INetworkManager
    {
        /// <summary>
        /// 当释放时
        /// </summary>
        public event Action<INetworkChannel> OnReleased;

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
        /// 网络频道构建字典
        /// </summary>
        private readonly Dictionary<string, Func<Uri, IPacker, string, INetworkChannel>> channelMaker;

        /// <summary>
        /// 构建Hash
        /// </summary>
        private readonly HashSet<string> makeHash;

        /// <summary>
        /// 需要定期调用的表
        /// </summary>
        private readonly List<ITick> ticks;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot;

        /// <summary>
        /// 网络管理器
        /// </summary>
        public NetworkManager()
        {
            channelMaker = new Dictionary<string, Func<Uri, IPacker, string, INetworkChannel>>();
            makeHash = new HashSet<string>();
            ticks = new List<ITick>();
            syncRoot = new object();
        }

        /// <summary>
        /// 扩展套接字协议
        /// </summary>
        /// <param name="protocol">协议名</param>
        /// <param name="maker"></param>
        public void ExtendChannelMaker(string protocol, Func<Uri, IPacker, string, INetworkChannel> maker)
        {
            Guard.NotEmptyOrNull(protocol, "protocol");
            Guard.Requires<ArgumentNullException>(maker != null);
            channelMaker.Add(NormalProtocol(protocol), maker);
        }

        /// <summary>
        /// 建立链接 
        /// </summary>
        /// <param name="nsp">网络服务提供商</param>
        /// <param name="name">名字</param>
        /// <returns>网络频道</returns>
        public INetworkChannel Make(string nsp, string name = null)
        {
            return Make(nsp, null, name);
        }

        /// <summary>
        /// 建立链接
        /// </summary>
        /// <param name="nsp">网络服务提供商</param>
        /// <param name="packer">打包解包器</param>
        /// <param name="name">名字</param>
        /// <returns>网络频道</returns>
        public INetworkChannel Make(string nsp, IPacker packer, string name = null)
        {
            lock (syncRoot)
            {
                Guard.NotEmptyOrNull(nsp, "nsp");
                name = string.IsNullOrEmpty(name) ? GetDefaultName() : name;

                if (!ContainsExtend(name))
                {
                    ExtendChannel(nsp, packer, name);
                }

                makeHash.Add(name);

                return AddChannelTick(name);
            }
        }

        /// <summary>
        /// 释放生成的网络频道
        /// </summary>
        /// <param name="name">解决方案名</param>
        public new void Release(string name = null)
        {
            lock (syncRoot)
            {
                var channel = Get(name);
                var tick = channel as ITick;

                base.Release(name);
                if (makeHash.Contains(name))
                {
                    ReleaseExtend(name);
                    makeHash.Remove(name);
                }

                if (tick != null)
                {
                    lock (syncRoot)
                    {
                        ticks.Remove(tick);
                    }
                }

                channel.Disconnect();
                UnregisterEvent(channel);

                if (OnReleased != null)
                {
                    OnReleased.Invoke(channel);
                }
            }
        }

        /// <summary>
        /// 自定义解决方案
        /// </summary>
        /// <param name="resolve">解决方案</param>
        /// <param name="name">名字</param>
        public new void Extend(Func<INetworkChannel> resolve, string name = null)
        {
            Guard.Requires<ArgumentNullException>(resolve != null);
            base.Extend(() =>
            {
                var channel = resolve.Invoke();
                RegisterEvent(channel);
                return channel;
            }, name);
        }

        /// <summary>
        /// 定期调用
        /// </summary>
        /// <param name="elapseMillisecond">流逝的时间</param>
        public void Tick(int elapseMillisecond)
        {
            elapseMillisecond = Math.Max(0, elapseMillisecond);
            lock (syncRoot)
            {
                foreach (var tick in ticks)
                {
                    tick.Tick(elapseMillisecond);
                }
            }
        }

        /// <summary>
        /// 添加到计时器
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>网络频道</returns>
        private INetworkChannel AddChannelTick(string name)
        {
            var channel = Get(name);
            var tickChannel = channel as ITick;
            if (tickChannel != null)
            {
                ticks.Add(tickChannel);
            }
            return channel;
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="channel">网络频道</param>
        private void RegisterEvent(INetworkChannel channel)
        {
            channel.OnError += OnChannelError;
            channel.OnClosed += OnChannelClosed;
            channel.OnMissHeartBeat += OnChannelMissHeartBeat;
            channel.OnConnected += OnChannelConnected;
            channel.OnDisconnect += OnChannelDisconnect;
            channel.OnMessage += OnChannelMessage;
            channel.OnSent += OnChannelSent;
        }

        /// <summary>
        /// 反注册事件
        /// </summary>
        /// <param name="channel">网络频道</param>
        private void UnregisterEvent(INetworkChannel channel)
        {
            channel.OnError -= OnChannelError;
            channel.OnClosed -= OnChannelClosed;
            channel.OnMissHeartBeat -= OnChannelMissHeartBeat;
            channel.OnConnected -= OnChannelConnected;
            channel.OnDisconnect -= OnChannelDisconnect;
            channel.OnMessage -= OnChannelMessage;
            channel.OnSent -= OnChannelSent;
        }

        /// <summary>
        /// 当网络频道异常时
        /// </summary>
        /// <param name="channel">网络频道</param>
        /// <param name="ex">异常内容</param>
        private void OnChannelError(INetworkChannel channel, Exception ex)
        {
            if (OnError == null)
            {
                return;
            }

            lock (OnError)
            {
                if (OnError != null)
                {
                    OnError.Invoke(channel, ex);
                }
            }
        }

        /// <summary>
        /// 当网络频道关闭时
        /// </summary>
        /// <param name="channel">网络频道</param>
        /// <param name="ex">由于什么原因导致的关闭</param>
        private void OnChannelClosed(INetworkChannel channel, Exception ex)
        {
            if (OnClosed == null)
            {
                return;
            }

            lock (OnClosed)
            {
                if (OnClosed != null)
                {
                    OnClosed.Invoke(channel, ex);
                }
            }
        }

        /// <summary>
        /// 当网络频道丢失心跳包时
        /// </summary>
        /// <param name="channel">网络频道</param>
        /// <param name="missCount">心跳丢失次数</param>
        private void OnChannelMissHeartBeat(INetworkChannel channel, int missCount)
        {
            if (OnMissHeartBeat == null)
            {
                return;
            }

            lock (OnMissHeartBeat)
            {
                if (OnMissHeartBeat != null)
                {
                    OnMissHeartBeat.Invoke(channel, missCount);
                }
            }
        }

        /// <summary>
        /// 当网络频道连接成功
        /// </summary>
        /// <param name="channel">网络频道</param>
        private void OnChannelConnected(INetworkChannel channel)
        {
            if (OnConnected == null)
            {
                return;
            }

            lock (OnConnected)
            {
                if (OnConnected != null)
                {
                    OnConnected.Invoke(channel);
                }
            }
        }

        /// <summary>
        /// 当网络频道断开链接
        /// </summary>
        /// <param name="channel">网络频道</param>
        private void OnChannelDisconnect(INetworkChannel channel)
        {
            if (OnDisconnect == null)
            {
                return;
            }

            lock (OnDisconnect)
            {
                if (OnDisconnect != null)
                {
                    OnDisconnect.Invoke(channel);
                }
            }
        }

        /// <summary>
        /// 当网络频道收到消息
        /// </summary>
        /// <param name="channel">网络频道</param>
        /// <param name="message">消息内容</param>
        private void OnChannelMessage(INetworkChannel channel, object message)
        {
            if (OnMessage == null)
            {
                return;
            }

            lock (OnMessage)
            {
                if (OnMessage != null)
                {
                    OnMessage.Invoke(channel, message);
                }
            }
        }

        /// <summary>
        /// 当网络频道发送成功
        /// </summary>
        /// <param name="channel">网络频道</param>
        /// <param name="message">发送的数据</param>
        private void OnChannelSent(INetworkChannel channel, byte[] message)
        {
            if (OnSent == null)
            {
                return;
            }

            lock (OnSent)
            {
                if (OnSent != null)
                {
                    OnSent.Invoke(channel, message);
                }
            }
        }

        /// <summary>
        /// 标准化协议名
        /// </summary>
        /// <param name="protocol">协议名</param>
        /// <returns>标准化后的协议名</returns>
        private string NormalProtocol(string protocol)
        {
            return protocol.ToLower();
        }

        /// <summary>
        /// 拓展Channel
        /// </summary>
        /// <param name="nsp">拓展端口</param>
        /// <param name="packer">打包解包器</param>
        /// <param name="name">名字</param>
        private void ExtendChannel(string nsp, IPacker packer, string name)
        {
            var uri = new Uri(nsp);
            Func<Uri, IPacker, string, INetworkChannel> maker;
            if (!channelMaker.TryGetValue(NormalProtocol(uri.Scheme), out maker))
            {
                throw new RuntimeException("Undefined network protocol [" + uri.Scheme + "]");
            }

            Extend(() => maker.Invoke(uri, packer, name), name);
        }
    }
}
