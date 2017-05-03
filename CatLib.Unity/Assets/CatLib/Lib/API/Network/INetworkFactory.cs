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
    /// 网络服务
    /// </summary>
    public interface INetworkFactory
    {
        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="name">连接名</param>
        T Create<T>(string name) where T : IConnector;

        /// <summary>
        /// 释放一个网络链接
        /// </summary>
        /// <param name="name">连接名</param>
        void Destroy(string name);

        /// <summary>
        /// 获取一个链接器
        /// </summary>
        /// <typeparam name="T">连接类型</typeparam>
        /// <param name="name">连接名</param>
        /// <returns>连接器</returns>
        T Get<T>(string name) where T : IConnector;
    }
}
