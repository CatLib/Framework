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

namespace CatLib.Hashing.Checksum
{
    /// <summary>
    /// Adler32校验
    /// </summary>
    public sealed class Adler32 : IChecksum
    {
        /// <summary>
        /// 最大素数
        /// </summary>
        private const uint Base = 65521;

        /// <summary>
        /// 校验和
        /// </summary>
        private uint checkValue;

        /// <summary>
        /// 构建一个Adler32校验
        /// </summary>
        public Adler32()
        {
            Reset();
        }

        /// <summary>
        /// 重置数据校验，恢复到初始状态
        /// </summary>
        public void Reset()
        {
            checkValue = 1;
        }

        /// <summary>
        /// 返回Adler32校验和
        /// </summary>
        public long Value
        {
            get
            {
                return checkValue;
            }
        }

        /// <summary>
        /// 使用输入值更新校验和
        /// </summary>
        /// <param name="bval">要添加的数据，int的高字节被忽略</param>
        public void Update(int bval)
        {
            var s1 = checkValue & 0xFFFF;
            var s2 = checkValue >> 16;

            s1 = (s1 + ((uint)bval & 0xFF)) % Base;
            s2 = (s1 + s2) % Base;

            checkValue = (s2 << 16) + s1;
        }

        /// <summary>
        /// 使用传入的字节数组更新数据校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        public void Update(byte[] buffer)
        {
            Update(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 将字节数组添加到数据校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="offset">起始偏移量</param>
        /// <param name="count">多少长度会被添加到数据校验</param>
        public void Update(byte[] buffer, int offset, int count)
        {
            Guard.Requires<ArgumentNullException>(buffer != null);
            Guard.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Guard.Requires<ArgumentOutOfRangeException>(offset < buffer.Length);
            Guard.Requires<ArgumentOutOfRangeException>(count >= 0);
            Guard.Requires<ArgumentOutOfRangeException>(offset + count <= buffer.Length);

            var s1 = checkValue & 0xFFFF;
            var s2 = checkValue >> 16;

            while (count > 0)
            {
                var n = 3800;
                if (n > count)
                {
                    n = count;
                }
                count -= n;
                while (--n >= 0)
                {
                    s1 = s1 + (uint)(buffer[offset++] & 0xff);
                    s2 = s2 + s1;
                }
                s1 %= Base;
                s2 %= Base;
            }

            checkValue = (s2 << 16) | s1;
        }
    }
}
