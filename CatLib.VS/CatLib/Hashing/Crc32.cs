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
using System.IO;

namespace CatLib.Hashing
{
    /// <summary>
    /// Crc32校验
    /// </summary>
    public sealed class Crc32
    {
        /// <summary>
        /// Crc种子
        /// </summary>
        private const uint CrcSeed = 0xFFFFFFFF;

        /// <summary>
        /// 校验表
        /// </summary>
        private static readonly uint[] Table;

        /// <summary>
        /// Crc32校验
        /// </summary>
        static Crc32()
        {
            Table = new uint[256];
            const uint kPoly = 0xEDB88320;
            for (uint i = 0; i < 256; i++)
            {
                var r = i;
                for (var j = 0; j < 8; j++)
                    if ((r & 1) != 0)
                        r = (r >> 1) ^ kPoly;
                    else
                        r >>= 1;
                Table[i] = r;
            }
        }

        /// <summary>
        /// 校验值
        /// </summary>
        private uint crc = CrcSeed;

        /// <summary>
        /// 校验值
        /// </summary>
        public uint Value
        {
            get
            {
                return crc ^ CrcSeed;
            }
            set
            {
                crc = value ^ CrcSeed;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns>本实例</returns>
        public Crc32 Reset()
        {
            crc = CrcSeed;
            return this;
        }

        /// <summary>
        /// 添加整数进行校验
        /// </summary>
        /// <param name = "value">添加的整数</param>
        /// <returns>Crc实例</returns>
        public Crc32 Update(int value)
        {
            crc = Table[(crc ^ value) & 0xFF] ^ (crc >> 8);
            return this;
        }

        /// <summary>
        /// 对字节数组进行校验
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">长度</param>
        /// <returns>Crc实例</returns>
        public Crc32 Update(byte[] buffer, int offset = 0, int count = -1)
        {
            Guard.Requires<ArgumentNullException>(buffer != null);

            if (count <= 0)
            {
                count = buffer.Length;
            }

            if (offset < 0 || offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            while (--count >= 0)
            {
                crc = Table[(crc ^ buffer[offset++]) & 0xFF] ^ (crc >> 8);
            }

            return this;
        }

        /// <summary>
        /// 对数据流进行校验
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="count">长度</param>
        /// <returns>Crc实例</returns>
        public Crc32 Update(Stream stream, long count = -1)
        {
            Guard.Requires<ArgumentNullException>(stream != null);

            if (count <= 0)
            {
                count = long.MaxValue;
            }

            while (--count >= 0)
            {
                var b = stream.ReadByte();
                if (b == -1)
                {
                    break;
                }
                crc = Table[(crc ^ b) & 0xFF] ^ (crc >> 8);
            }

            return this;
        }
    }
}
