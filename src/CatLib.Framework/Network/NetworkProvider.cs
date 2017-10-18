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
using CatLib.Network.Packer;

namespace CatLib.Network
{
    /// <summary>
    /// 网络服务提供者
    /// </summary>
    public sealed class NetworkProvider : IServiceProvider
    {
        /// <summary>
        /// Tick桥
        /// </summary>
        private sealed class TickBridge : ITick
        {
            /// <summary>
            /// Tick桥
            /// </summary>
            private readonly NetworkManager manager;

            /// <summary>
            /// Tick桥
            /// </summary>
            /// <param name="manager"></param>
            public TickBridge(NetworkManager manager)
            {
                this.manager = manager;
            }
            /// <summary>
            /// 定期调用
            /// </summary>
            /// <param name="elapseMillisecond">流逝的时间</param>
            public void Tick(int elapseMillisecond)
            {
                manager.Tick(elapseMillisecond);
            }
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<NetworkManager>().Alias<INetworkManager>().OnResolving((_, obj) =>
            {
                var factory = (NetworkManager)obj;
                ExtendNetworkMaker(factory);
                BindSocketFactory(factory);
                App.MakeWith<TickBridge>(factory);
                return factory;
            }).OnRelease((_, obj) =>
            {
                App.Release<TickBridge>();
            });
            App.Singleton<TickBridge>();
        }

        /// <summary>
        /// 绑定套接字工厂
        /// </summary>
        /// <param name="factory">网络构造工厂</param>
        private void BindSocketFactory(NetworkManager factory)
        {
            var socketFactory = App.Make<ISocketManager>();

            if (socketFactory == null)
            {
                throw new RuntimeException("Need register SocketProvider");
            }

            factory.OnReleased += (channel) =>
            {
                socketFactory.Release(channel.Name);
            };
        }

        /// <summary>
        /// 拓展网络构建器
        /// </summary>
        /// <param name="factory">网络构造工厂</param>
        private void ExtendNetworkMaker(NetworkManager factory)
        {
            factory.ExtendChannelMaker("tcp", (uri, packer, name) => MakeChannel(uri.OriginalString, packer, name));
            factory.ExtendChannelMaker("kcp", (uri, packer, name) => MakeChannel(uri.OriginalString, packer, name));
            factory.ExtendChannelMaker("frame.tcp", (uri, packer, name) => MakeChannel(uri.OriginalString.Substring(6), packer ?? new FramePacker(), name));
            factory.ExtendChannelMaker("frame.kcp", (uri, packer, name) => MakeChannel(uri.OriginalString.Substring(6), packer ?? new FramePacker(), name));
            factory.ExtendChannelMaker("text.tcp", (uri, packer, name) => MakeChannel(uri.OriginalString.Substring(5), packer ?? new TextPacker(), name));
            factory.ExtendChannelMaker("text.kcp", (uri, packer, name) => MakeChannel(uri.OriginalString.Substring(5), packer ?? new TextPacker(), name));
        }

        /// <summary>
        /// 生成网络频道
        /// </summary>
        /// <param name="nsp">网络提供商</param>
        /// <param name="packer">打包解包器</param>
        /// <param name="name">名字</param>
        /// <returns>网络频道</returns>
        private INetworkChannel MakeChannel(string nsp, IPacker packer, string name)
        {
            if (packer == null)
            {
                throw new ArgumentNullException("packer", "Please call : Make(string, IPacker, string)");
            }
            var socketFactory = App.Make<ISocketManager>();
            var socket = socketFactory.Make(nsp, name);
            return new NetworkChannel(socket, packer, name);
        }
    }
}
