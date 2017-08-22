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

using CatLib.API.Hashing;
using CatLib.Hashing.Checksum;
using System.Collections.Generic;

namespace CatLib.Hashing
{
    /// <summary>
    /// 哈希
    /// </summary>
    public sealed class Hashing : IHashing
    {
        /// <summary>
        /// 校验类字典
        /// </summary>
        private readonly Dictionary<Checksums, IChecksum> checksumsDict;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 哈希
        /// </summary>
        public Hashing()
        {
            checksumsDict = new Dictionary<Checksums, IChecksum>
            {
                { Checksums.Crc32, new Crc32() },
                { Checksums.BZip2Crc32, new BZip2Crc32() },
                { Checksums.Adler32, new Adler32() }
            };
        }

        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        public long Checksum(byte[] buffer, Checksums checksum = Checksums.Crc32)
        {
            IChecksum checksumClass;
            if (!checksumsDict.TryGetValue(checksum, out checksumClass))
            {
                throw new RuntimeException("Undefiend Checksum:" + checksum);
            }
            lock (syncRoot)
            {
                checksumClass.Reset();
                checksumClass.Update(buffer);
                return checksumClass.Value;
            }
        }

        /// <summary>
        /// 对输入值进行Hash
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="factor">加密因子</param>
        /// <returns>哈细值</returns>
        public string Hash(string input, int factor = 10)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, factor);
        }

        /// <summary>
        /// 验证输入值和哈希值是否匹配
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">哈希值</param>
        /// <returns>是否匹配</returns>
        public bool Check(string input, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(input, hash);
        }
    }
}
