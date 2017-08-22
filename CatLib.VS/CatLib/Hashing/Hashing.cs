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
using CatLib.Hashing.HashString;
using Murmur;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CatLib.Hashing
{
    /// <summary>
    /// 哈希
    /// </summary>
    internal sealed class Hashing : IHashing
    {
        /// <summary>
        /// 校验类字典
        /// </summary>
        private readonly Dictionary<Checksums, IChecksum> checksumsDict;

        /// <summary>
        /// 非加密哈希字典
        /// </summary>
        private readonly Dictionary<Hashes, HashAlgorithm> hashByteDict;

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

            hashByteDict = new Dictionary<Hashes, HashAlgorithm>
            {
                { Hashes.MurmurHash , new Murmur32ManagedX86() },
                { Hashes.Djb, new DjbHash() }
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
        /// 对输入值进行加密性Hash
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="factor">加密因子</param>
        /// <returns>哈希值</returns>
        public string HashPassword(string input, int factor = 10)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, factor);
        }

        /// <summary>
        /// 验证输入值和加密性哈希值是否匹配
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">哈希值</param>
        /// <returns>是否匹配</returns>
        public bool CheckPassword(string input, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(input, hash);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        public uint HashString(string input, Hashes hash = Hashes.MurmurHash)
        {
            return HashString(input, Encoding.Default, hash);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="encoding">编码</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        public uint HashString(string input, Encoding encoding, Hashes hash = Hashes.MurmurHash)
        {
            Guard.Requires<ArgumentNullException>(encoding != null);
            var data = encoding.GetBytes(input);
            return HashByte(data);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        public uint HashByte(byte[] input, Hashes hash = Hashes.MurmurHash)
        {
            HashAlgorithm hashStringClass;
            if (!hashByteDict.TryGetValue(hash, out hashStringClass))
            {
                throw new RuntimeException("Undefiend Hashing:" + hash);
            }
            return BitConverter.ToUInt32(hashStringClass.ComputeHash(input), 0);
        }
    }
}
