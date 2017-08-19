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


    }
}
