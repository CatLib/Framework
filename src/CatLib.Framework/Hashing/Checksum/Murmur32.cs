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
using CatLib._3rd.Murmur;

namespace CatLib.Hashing.Checksum
{
    /// <summary>
    /// Murmur32
    /// </summary>
    internal sealed class Murmur32 : Murmur32ManagedX86, IChecksum
    {
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
            base.Initialize();
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
    }
}
