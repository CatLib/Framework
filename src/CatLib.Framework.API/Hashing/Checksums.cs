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

namespace CatLib.API.Hashing
{
    /// <summary>
    /// 使用的校验方法
    /// </summary>
    public class Checksums : Enum
    {
        /// <summary>
        /// Adler32
        /// </summary>
        public static readonly Checksums Adler32 = new Checksums("Adler32");

        /// <summary>
        /// Crc32
        /// </summary>
        public static readonly Checksums Crc32 = new Checksums("Crc32");

        /// <summary>
        /// Djb
        /// </summary>
        public static readonly Checksums Djb = new Checksums("Djb");

        /// <summary>
        /// Murmur32
        /// </summary>
        public static readonly Checksums Murmur32 = new Checksums("Murmur32");

        /// <summary>
        /// 哈希算法类型
        /// </summary>
        /// <param name="name">哈希算法名字</param>
        protected Checksums(string name) : base(name)
        {
        }

        /// <summary>
        /// 字符串转Checksums
        /// </summary>
        /// <param name="type">类型</param>
        public static implicit operator Checksums(string type)
        {
            return new Checksums(type);
        }
    }
}
