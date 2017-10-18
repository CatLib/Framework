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

namespace CatLib.API.Network
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public interface INetworkManager : ISingleManager<INetworkChannel>
    {
        /// <summary>
        /// 当释放时
        /// </summary>
        event Action<INetworkChannel> OnReleased;

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
        /// 建立链接
        /// </summary>
        /// <param name="nsp">网络服务提供商</param>
        /// <param name="name">名字</param>
        /// <returns>网络频道</returns>
        INetworkChannel Make(string nsp, string name = null);

        /// <summary>
        /// 建立链接
        /// </summary>
        /// <param name="nsp">网络服务提供商</param>
        /// <param name="packer">打包解包器</param>
        /// <param name="name">名字</param>
        /// <returns>网络频道</returns>
        INetworkChannel Make(string nsp, IPacker packer, string name = null);
    }
}
