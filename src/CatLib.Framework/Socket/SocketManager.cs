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
using CatLib.API.Socket;

namespace CatLib.Socket
{
    /// <summary>
    /// Socket管理器
    /// </summary>
    public sealed class SocketManager : SingleManager<ISocket>, ISocketManager
    {
        /// <summary>
        /// Socket构建字典
        /// </summary>
        private readonly Dictionary<string, Func<Uri, ISocket>> socketMaker;

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
        /// Socket管理器
        /// </summary>
        public SocketManager()
        {
            socketMaker = new Dictionary<string, Func<Uri, ISocket>>();
            makeHash = new HashSet<string>();
            ticks = new List<ITick>();
            syncRoot = new object();
        }

        /// <summary>
        /// 建立链接
        /// </summary>
        /// <param name="nsp">
        /// 网络服务提供商
        /// <para>tcp://localhost:9999</para>
        /// <para>kcp://819182618@localhost:9999</para>
        /// <para>ws://localhost:9999</para>
        /// </param>
        /// <param name="name">名字</param>
        /// <returns>Socket链接</returns>
        public ISocket Make(string nsp, string name = null)
        {
            lock (syncRoot)
            {
                Guard.NotEmptyOrNull(nsp, "nsp");
                name = string.IsNullOrEmpty(name) ? GetDefaultName() : name;

                if (!ContainsExtend(name))
                {
                    ExtendSocket(nsp, name);
                }

                makeHash.Add(name);

                var socket = Get(name);
                var tickSocket = socket as ITick;
                if (tickSocket != null)
                {
                    ticks.Add(tickSocket);
                }
                return socket;
            }
        }

        /// <summary>
        /// 释放生成的Socket
        /// </summary>
        /// <param name="name">解决方案名</param>
        public new void Release(string name = null)
        {
            lock (syncRoot)
            {
                var socket = Get(name);
                var tick = socket as ITick;

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
                socket.Disconnect();
            }
        }

        /// <summary>
        /// 定期调用
        /// </summary>
        public void Tick()
        {
            lock (syncRoot)
            {
                foreach (var tick in ticks)
                {
                    tick.Tick();
                }
            }
        }

        /// <summary>
        /// 扩展套接字协议
        /// </summary>
        /// <param name="protocol">协议名</param>
        /// <param name="maker"></param>
        public void ExtendSocketMaker(string protocol, Func<Uri, ISocket> maker)
        {
            Guard.NotEmptyOrNull(protocol, "protocol");
            Guard.Requires<ArgumentNullException>(maker != null);
            socketMaker.Add(NormalProtocol(protocol), maker);
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
        /// 拓展Socket
        /// </summary>
        /// <param name="nsp">拓展端口</param>
        /// <param name="name">名字</param>
        private void ExtendSocket(string nsp, string name)
        {
            var uri = new Uri(nsp);
            Func<Uri, ISocket> maker;
            if (!socketMaker.TryGetValue(NormalProtocol(uri.Scheme), out maker))
            {
                throw new RuntimeException("Undefined socket protocol [" + uri.Scheme + "]");
            }

            Extend(() => maker.Invoke(uri), name);
        }
    }
}
