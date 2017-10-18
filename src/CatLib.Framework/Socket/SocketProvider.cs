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

namespace CatLib.Socket
{
    /// <summary>
    /// Socket服务提供者
    /// </summary>
    public sealed class SocketProvider : IServiceProvider
    {
        /// <summary>
        /// Tick桥
        /// </summary>
        private sealed class TickBridge : CatLib.ITick
        {
            /// <summary>
            /// Tick桥
            /// </summary>
            private readonly SocketManager manager;

            /// <summary>
            /// Tick桥
            /// </summary>
            /// <param name="manager"></param>
            public TickBridge(SocketManager manager)
            {
                this.manager = manager;
            }
            /// <summary>
            /// 定期调用
            /// </summary>
            /// <param name="elapseMillisecond">流逝的时间</param>
            public void Tick(int elapseMillisecond)
            {
                manager.Tick();
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
            App.Singleton<SocketManager>().Alias<ISocketManager>().OnResolving((_, obj) =>
            {
                var factory = (SocketManager)obj;
                ExtendSocketMaker(factory);
                App.MakeWith<TickBridge>(factory);
                return factory;
            }).OnRelease((_, __) =>
            {
                App.Release<TickBridge>();
            });
            App.Singleton<TickBridge>();
        }

        /// <summary>
        /// 拓展Socket构建器
        /// </summary>
        /// <param name="factory">Socket工厂</param>
        private void ExtendSocketMaker(SocketManager factory)
        {
            factory.ExtendSocketMaker("tcp", (uri) => new TcpConnector(uri.Host, uri.Port));
            factory.ExtendSocketMaker("kcp", (uri) =>
            {
                try
                {
                    var conv = uint.Parse(uri.UserInfo);
                    return new KcpConnector(conv, uri.Host, uri.Port);
                }
                catch
                {
                    throw new RuntimeException("kcp need [conv](uint) , eg: [kcp://81729@localhost:7777]");
                }
            });
        }
    }
}
