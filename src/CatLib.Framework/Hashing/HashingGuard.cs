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

namespace CatLib.Hashing
{
    /// <summary>
    /// 守卫
    /// </summary>
    internal static class HashingGuard
    {
        /// <summary>
        /// 校验BufferOffsetCount
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">多少长度会被添加到数据校验</param>
        public static void BufferOffsetCount(byte[] buffer, int offset, int count)
        {
            Guard.Requires<ArgumentNullException>(buffer != null);
            Guard.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Guard.Requires<ArgumentOutOfRangeException>(offset < buffer.Length);
            Guard.Requires<ArgumentOutOfRangeException>(count >= 0);
            Guard.Requires<ArgumentOutOfRangeException>(offset + count <= buffer.Length);
        }
    }
}
