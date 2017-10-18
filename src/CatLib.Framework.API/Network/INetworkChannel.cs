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
using CatLib.API.Socket;

namespace CatLib.API.Network
{
    /// <summary>
    /// 网络频道
    /// </summary>
    public interface INetworkChannel
    {
        /// <summary>
        /// 当网络链接完成时
        /// </summary>
        event Action<INetworkChannel> OnConnected;

        /// <summary>
        /// 当网络出现异常时
        /// </summary>
        event Action<INetworkChannel, Exception> OnError;

        /// <summary>
        /// 当收到数据包时
        /// </summary>
        event Action<INetworkChannel, object> OnMessage;

        /// <summary>
        /// 当网络断开链接
        /// </summary>
        event Action<INetworkChannel> OnDisconnect;

        /// <summary>
        /// 当网络链接关闭时
        /// </summary>
        event Action<INetworkChannel, Exception> OnClosed;

        /// <summary>
        /// 当数据发送完成
        /// </summary>
        event Action<INetworkChannel, byte[]> OnSent;

        /// <summary>
        /// 当丢失心跳时
        /// </summary>
        event Action<INetworkChannel, int> OnMissHeartBeat;

        /// <summary>
        /// 网络频道名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 基础网络套接字
        /// </summary>
        ISocket Socket { get; }

        /// <summary>
        /// 设定心跳包(毫秒)
        /// </summary>
        /// <param name="intervalMillisecond">心跳包间隔(毫秒)</param>
        void SetHeartBeat(int intervalMillisecond);

        /// <summary>
        /// 链接到服务器
        /// </summary>
        IAwait Connect();

        /// <summary>
        /// 链接到服务器
        /// </summary>
        /// <param name="hostname">服务器名</param>
        /// <param name="port">端口</param>
        IAwait Connect(string hostname, int port);

        /// <summary>
        /// 断开链接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 断开链接,并给定一个断开链接的异常
        /// </summary>
        /// <param name="ex">由于什么异常导致断开链接</param>
        void Disconnect(Exception ex);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="packet">网络数据包</param>
        IAwait Send(object packet);
    }
}
