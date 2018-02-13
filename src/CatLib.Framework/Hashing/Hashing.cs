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
using System;
using System.Collections.Generic;
using System.Text;
using CatLib._3rd.BCrypt.Net;

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
        private readonly Dictionary<Checksums, Func<IChecksum>> checksumsMaker;

        /// <summary>
        /// 校验类字典
        /// </summary>
        private readonly Dictionary<Checksums, IChecksum> checksumsDict;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 默认的校验算法
        /// </summary>
        private readonly Checksums defaultChecksum;

        /// <summary>
        /// 默认的编码
        /// </summary>
        private readonly Encoding defaultEncoding;

        /// <summary>
        /// 哈希
        /// </summary>
        public Hashing(Checksums defaultChecksum, Encoding defaultEncoding)
        {
            Guard.Requires<ArgumentNullException>(defaultChecksum != null);
            Guard.Requires<ArgumentNullException>(defaultEncoding != null);

            this.defaultChecksum = defaultChecksum;
            this.defaultEncoding = defaultEncoding;

            checksumsMaker = new Dictionary<Checksums, Func<IChecksum>>();
            checksumsDict = new Dictionary<Checksums, IChecksum>();
        }

        /// <summary>
        /// 拓展校验算法
        /// </summary>
        /// <param name="checksum">校验类类型</param>
        /// <param name="builder">构建器</param>
        public void Extend(Checksums checksum, Func<IChecksum> builder)
        {
            Guard.Requires<ArgumentNullException>(checksum != null);
            Guard.Requires<ArgumentNullException>(builder != null);
            checksumsMaker.Add(checksum, builder);
        }

        /// <summary>
        /// 使用默认的校验算法计算校验和
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>校验和</returns>
        public long Checksum(string input)
        {
            return Checksum(input, defaultChecksum);
        }

        /// <summary>
        /// 使用默认的校验算法计算校验和
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        public long Checksum(string input, Checksums checksum)
        {
            return Checksum(input, defaultEncoding, checksum);
        }

        /// <summary>
        /// 使用默认的校验算法计算校验和
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="encoding">编码</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        public long Checksum(string input, Encoding encoding, Checksums checksum)
        {
            Guard.Requires<ArgumentNullException>(input != null);
            Guard.Requires<ArgumentNullException>(encoding != null);
            return Checksum(encoding.GetBytes(input), checksum);
        }

        /// <summary>
        /// 使用默认的校验算法计算校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns>校验和</returns>
        public long Checksum(byte[] buffer)
        {
            return Checksum(buffer, defaultChecksum);
        }

        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        public long Checksum(byte[] buffer, Checksums checksum)
        {
            Guard.Requires<ArgumentNullException>(buffer != null);

            lock (syncRoot)
            {
                var checksumClass = GetChecksum(checksum);
                checksumClass.Update(buffer);
                return checksumClass.Value;
            }
        }

        /// <summary>
        /// 将字节数组添加到数据校验和
        /// </summary>
        /// <param name="callback">回调闭包</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns></returns>
        public long Checksum(Action<Action<byte[], int, int>> callback , Checksums checksum)
        {
            Guard.Requires<ArgumentNullException>(callback != null);
            lock (syncRoot)
            {
                var checksumClass = GetChecksum(checksum);
                long value = 0;
                callback.Invoke((buffer, offset, count) => 
                {
                    value = Checksum(buffer, offset, count, checksumClass);
                });
                return value;
            }
        }

        /// <summary>
        /// 将字节数组添加到数据校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="offset">起始偏移量</param>
        /// <param name="count">多少长度会被添加到数据校验</param>
        /// <param name="checksum">使用效验类类型</param>
        /// <returns>效验值</returns>
        private long Checksum(byte[] buffer, int offset, int count, IChecksum checksum)
        {
            HashingGuard.BufferOffsetCount(buffer, offset, count);
            checksum.Update(buffer, offset, count);
            return checksum.Value;
        }

        /// <summary>
        /// 对输入值进行加密性Hash
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="factor">加密因子</param>
        /// <returns>哈希值</returns>
        public string HashPassword(string input, int factor = 10)
        {
            Guard.Requires<ArgumentNullException>(input != null);
            return BCrypt.HashPassword(input, factor);
        }

        /// <summary>
        /// 验证输入值和加密性哈希值是否匹配
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">哈希值</param>
        /// <returns>是否匹配</returns>
        public bool CheckPassword(string input, string hash)
        {
            Guard.Requires<ArgumentNullException>(input != null);
            Guard.Requires<ArgumentNullException>(hash != null);
            return BCrypt.Verify(input, hash);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        public uint HashString(string input)
        {
            return HashString(input, Hashes.MurmurHash);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        public uint HashString(string input, Hashes hash)
        {
            return HashString(input, defaultEncoding, hash);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="encoding">编码</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        public uint HashString(string input, Encoding encoding, Hashes hash)
        {
            Guard.Requires<ArgumentNullException>(input != null);
            Guard.Requires<ArgumentNullException>(encoding != null);
            var data = encoding.GetBytes(input);
            return HashByte(data, hash);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashByte is obsolete, please use Checksum")]
        public uint HashByte(byte[] input)
        {
            return HashByte(input, Hashes.MurmurHash);
        }

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashByte is obsolete, please use Checksum")]
        public uint HashByte(byte[] input, Hashes hash)
        {
            Guard.Requires<ArgumentNullException>(input != null);
            Guard.Requires<ArgumentNullException>(hash != null);

            var mapping = new Dictionary<Hashes, Checksums>
            {
                { Hashes.MurmurHash, Checksums.Murmur32 },
                { Hashes.Djb, Checksums.Djb }
            };

            return (uint) Checksum(input, mapping[hash]);
        }

        /// <summary>
        /// 获取校验器
        /// </summary>
        /// <param name="checksum">校验器类型</param>
        /// <returns>校验器</returns>
        private IChecksum GetChecksum(Checksums checksum)
        {
            IChecksum checksumClass;
            if (!checksumsDict.TryGetValue(checksum, out checksumClass))
            {
                Func<IChecksum> checksumMaker;
                if (!checksumsMaker.TryGetValue(checksum, out checksumMaker)
                    || (checksumClass = checksumMaker.Invoke()) == null)
                {
                    throw new RuntimeException("Undefiend Checksum:" + checksum);
                }
                checksumsDict[checksum] = checksumClass;
            }
            checksumClass.Reset();
            return checksumClass;
        }
    }
}
