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

namespace CatLib.Network.Packer
{
    /// <summary>
    /// Frame协议
    /// 协议格式为 总包长+包体，其中包长(总包长+包体)为4字节网络字节序的整数，包体可以是普通文本或者二进制数据。
    /// </summary>
    public sealed class FramePacker : IPacker
    {
        /// <summary>
        /// <para>检查包的完整性</para>
        /// <para>如果能够得到包长，则返回包的在buffer中的长度(包含包头)，否则返回0继续等待数据</para>
        /// <para>如果协议有问题，则填入ex参数，当前连接会因此断开</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public int Input(byte[] source, out Exception ex)
        {
            ex = null;
            try
            {
                return source == null || source.Length < 4 ? 0 : BitConverter.ToInt32(Arr.Slice(source, 0, 4), 0);
            }
            catch (Exception e)
            {
                ex = e;
                return 0;
            }
        }

        /// <summary>
        /// 序列化消息包。
        /// </summary>
        /// <param name="packet">要序列化的消息包。</param>
        /// <param name="ex">用户自定义异常</param>
        /// <returns>序列化后的消息包字节流。</returns>
        public byte[] Encode(object packet, out Exception ex)
        {
            var data = packet as byte[];
            ex = data == null ? new RuntimeException("packet is Invalid") : null;
            if (data != null && data.Length > 0)
            {
                data = Arr.Merge(BitConverter.GetBytes(data.Length + 4), data);
            }
            return data == null || data.Length <= 0 ? null : data;
        }

        /// <summary>
        /// 反序列化消息包(包体)。
        /// </summary>
        /// <param name="source">反序列化的数据。</param>
        /// <param name="ex">用户自定义错误数据。</param>
        /// <returns>反序列化后的消息包。</returns>
        public object Decode(byte[] source, out Exception ex)
        {
            ex = null;
            return Arr.Slice(source, 4);
        }
    }
}
