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

namespace CatLib.API.Network
{
    /// <summary>
    /// Udp连接
    /// </summary>
    public interface IConnectorUdp : IConnectorSocket
    {
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="package">数据包</param>
        /// <param name="host">host</param>
        /// <param name="port">端口</param>
        void Send(IPackage package, string host, int port);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="host">host</param>
        /// <param name="port">端口</param>
        void Send(byte[] data, string host, int port);
    }
}