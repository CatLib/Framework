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

using System.IO;
using System.Security.AccessControl;

namespace CatLib.Hashing
{
    /// <summary>
    /// 哈希
    /// </summary>
    public sealed class Hashing
    {
        /// <summary>
        /// Crc32
        /// </summary>
        private Crc32 crc32;

        /// <summary>
        /// Crc32
        /// </summary>
        private Crc32 Crc32Instance
        {
            get
            {
                return crc32 ?? (crc32 = new Crc32());
            }
        }

        /// <summary>
        /// 进行Crc32校验
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count"></param>
        /// <returns>校验码</returns>
        public uint Crc32(byte[] buffer, int offset = 0, int count = -1)
        {
            return Crc32Instance.Reset().Update(buffer, offset, count).Value;
        }

        /// <summary>
        /// 进行Crc32校验
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="count">长度</param>
        /// <returns>校验码</returns>
        public uint Crc32(Stream stream, long count)
        {
            return Crc32Instance.Reset().Update(stream, count).Value;
        }
    }
}
