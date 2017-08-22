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
using System.Security.Cryptography;

namespace CatLib.Hashing.HashString
{
    /// <summary>
    /// Djb Hash
    /// </summary>
    public sealed class DjbHash : HashAlgorithm
    {
        /// <summary>
        /// Hash值
        /// </summary>
        private uint hash;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            Reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            hash = 5381;
        }

        /// <summary>
        /// 核心Hash算法
        /// </summary>
        /// <param name="array">Hash字节流</param>
        /// <param name="ibStart">起始</param>
        /// <param name="cbSize">长度</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (var i = ibStart; cbSize > 0; i++, cbSize--)
            {
                hash = ((hash << 5) + hash) + array[i];
            }
        }

        /// <summary>
        /// Hash完成
        /// </summary>
        /// <returns>哈希值</returns>
        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(hash);
        }
    }
}
