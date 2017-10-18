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

namespace CatLib.API.Socket
{
    /// <summary>
    /// Socket管理器
    /// </summary>
    public interface ISocketManager : ISingleManager<ISocket>
    {
        /// <summary>
        /// 建立链接
        /// </summary>
        /// <param name="nsp">
        /// 网络服务提供商
        /// <para>tcp://localhost:9999</para>
        /// <para>kcp://localhost:9999</para>
        /// <para>ws://localhost:9999</para>
        /// <para>wss://localhost:9999</para>
        /// </param>
        /// <param name="name">名字</param>
        /// <returns>Socket链接</returns>
        ISocket Make(string nsp, string name = null);
    }
}
