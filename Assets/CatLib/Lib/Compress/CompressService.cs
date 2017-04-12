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

using CatLib.API.Compress;

namespace CatLib.Compress
{
    /// <summary>
    /// 压缩服务
    /// </summary>
    public sealed class CompressService : ICompress
    {
        /// <summary>
        /// 压缩服务适配器，接入第三方的压缩组件
        /// </summary>
        private readonly ICompressAdapter compress;

        /// <summary>
        /// 构造一个压缩服务
        /// </summary>
        /// <param name="adapter">适配器</param>
        public CompressService(ICompressAdapter adapter)
        {
            compress = adapter;
        }

        /// <summary>
        /// 对字节序进行压缩
        /// </summary>
        /// <param name="bytes">需要被压缩的字节序</param>
        /// <param name="level">压缩等级(0-9)</param>
        /// <returns>被压缩后的字节序</returns>
        public byte[] Compress(byte[] bytes, int level = 6)
        {
            if (bytes == null)
            {
                return null;
            }
            return bytes.Length <= 0 ? bytes : compress.Compress(bytes, level);
        }

        /// <summary>
        /// 解压缩字节序
        /// </summary>
        /// <param name="bytes">被压缩的字节序</param>
        /// <returns>被解压的字节序</returns>
        public byte[] UnCompress(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return bytes.Length <= 0 ? bytes : compress.UnCompress(bytes);
        }
    }
}

