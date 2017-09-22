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
using System.Threading;
using CatLib.API.Socket;

namespace CatLib.Socket
{
    /// <summary>
    /// Socket服务提供者
    /// </summary>
    public sealed class SocketProvider : IServiceProvider
    {
        /// <summary>
        /// Bool释放器
        /// </summary>
        private class BooleanDisposable : IDisposable
        {
            /// <summary>
            /// 是否被释放
            /// </summary>
            public bool IsDispose { get; private set; }

            /// <summary>
            /// Bool释放器
            /// </summary>
            public BooleanDisposable()
            {
                IsDispose = false;
            }

            /// <summary>
            /// 释放
            /// </summary>
            public void Dispose()
            {
                IsDispose = true;
            }
        }

        /// <summary>
        /// 帧刷新时间，以1MS为单位
        /// </summary>
        [Config]
        public int Tick { get; set; }

        /// <summary>
        /// 释放句柄
        /// </summary>
        private IDisposable disposable;

        /// <summary>
        /// Socket服务提供者
        /// </summary>
        public SocketProvider()
        {
            Tick = 10;
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
                MakeTimeInterval(factory);
                return factory;
            }).OnRelease((_, __) =>
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            });
        }

        /// <summary>
        /// 生成定时刷新帧
        /// </summary>
        /// <param name="factory"></param>
        private void MakeTimeInterval(SocketManager factory)
        {
            var booleanDisposable = new BooleanDisposable();
            disposable = booleanDisposable;
            ThreadPool.QueueUserWorkItem(TickThread, booleanDisposable);
        }

        /// <summary>
        /// 计时线程
        /// </summary>
        /// <param name="payload">载荷</param>
        private void TickThread(object payload)
        {
            var factory = App.Make<SocketManager>();
            var booleanDisposable = (BooleanDisposable)payload;
            while (!booleanDisposable.IsDispose)
            {
                factory.Tick();
                Thread.Sleep(Tick);
            }
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
