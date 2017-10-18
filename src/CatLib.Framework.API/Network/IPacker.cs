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
    /// 打包解包器
    /// </summary>
    public interface IPacker
    {
        /// <summary>
        /// <para>检查包的完整性。</para>
        /// <para>如果能够得到包长，则返回包的在buffer中的长度(包含包头)，否则返回0继续等待数据。</para>
        /// <para>如果协议有问题，则填入ex参数，当前连接会因此断开。</para>
        /// </summary>
        /// <param name="source">需要检查完整性的数据</param>
        /// <param name="ex">用户自定义异常</param>
        /// <returns>如果能够得到包长，则返回包的在buffer中的长度(包含包头)，否则返回0继续等待数据。</returns>
        int Input(byte[] source, out Exception ex);

        /// <summary>
        /// 序列化消息包。
        /// <para>如果协议有问题，则填入ex参数，当前连接会因此断开。</para>
        /// </summary>
        /// <param name="packet">需要序列化的消息包。</param>
        /// <param name="ex">用户自定义异常。</param>
        /// <returns>序列化后的消息包字节流。如果需要抛弃数据包则返回null。</returns>
        byte[] Encode(object packet, out Exception ex);

        /// <summary>
        /// 反序列化消息包(包体)。
        /// <para>如果协议有问题，则填入ex参数，当前连接会因此断开。</para>
        /// </summary>
        /// <param name="source">需要反序列化的数据。</param>
        /// <param name="ex">用户自定义异常。</param>
        /// <returns>反序列化后的消息包。如果需要抛弃数据包则返回null。</returns>
        object Decode(byte[] source, out Exception ex);
    }
}
