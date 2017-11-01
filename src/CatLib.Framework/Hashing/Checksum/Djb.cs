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

namespace CatLib.Hashing.Checksum
{
    /// <summary>
    /// Djb Hash
    /// </summary>
    public sealed class Djb : HashAlgorithm , IChecksum
    {
        /// <summary>
        /// Hash值
        /// </summary>
        private uint hash;

        /// <summary>
        /// Djb Hash
        /// </summary>
        public Djb()
        {
            Reset();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 重置数据校验，恢复到初始状态
        /// </summary>
        void IChecksum.Reset()
        {
            Reset();
        }

        /// <summary>
        /// 返回到目前为止计算的数据校验和
        /// </summary>
        long IChecksum.Value
        {
            get
            {
                return BitConverter.ToUInt32(Hash, 0);
            }
        }

        /// <summary>
        /// 增加一个字节的校验
        /// </summary>
        /// <param name="bval">要添加的数据，int的高字节被忽略</param>
        public void Update(int bval)
        {
            Update(BitConverter.GetBytes(bval));
        }

        /// <summary>
        /// 使用传入的字节数组更新数据校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        public void Update(byte[] buffer)
        {
            Guard.Requires<ArgumentNullException>(buffer != null);
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
            HashingGuard.BufferOffsetCount(buffer, offset, count);
            ComputeHash(buffer, offset, count);
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
