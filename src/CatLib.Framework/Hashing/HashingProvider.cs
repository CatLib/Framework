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

#if CATLIB
using System.Text;
using CatLib.API.Hashing;
using CatLib.Hashing.Checksum;

namespace CatLib.Hashing
{
    /// <summary>
    /// 哈希服务提供者
    /// </summary>
    public sealed class HashingProvider : IServiceProvider
    {
        /// <summary>
        /// 默认的校验类
        /// </summary>
        public string DefaultChecksum { get; set; }

        /// <summary>
        /// 默认的编码
        /// </summary>
        public Encoding DefaultEncoding { get; set; }

        /// <summary>
        /// 哈希服务提供者
        /// </summary>
        public HashingProvider()
        {
            DefaultChecksum = Checksums.Crc32;
            DefaultEncoding = Encoding.Default;
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<Hashing>((_, __) => new Hashing(DefaultChecksum, DefaultEncoding))
                .Alias<IHashing>().OnResolving((_, obj) =>
            {
                var hashing = (Hashing)obj;

                hashing.Extend(Checksums.Crc32, () => new Crc32());
                hashing.Extend(Checksums.Adler32, () => new Adler32());
                hashing.Extend(Checksums.Djb, () => new Djb());
                hashing.Extend(Checksums.Murmur32, () => new Murmur32());
                return obj;
            });
        }
    }
}
#endif