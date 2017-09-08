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
    /// 哈希算法
    /// </summary>
    public class Hashes : Enum
    {
        /// <summary>
        /// DJB Hash
        /// </summary>
        public static readonly Hashes Djb = new Hashes("Djb");

        /// <summary>
        /// Murmur Hash
        /// </summary>
        public static readonly Hashes MurmurHash = new Hashes("MurmurHash");

        /// <summary>
        /// 哈希算法类型
        /// </summary>
        /// <param name="name">哈希算法名字</param>
        protected Hashes(string name) : base(name)
        {
        }

        /// <summary>
        /// 字符串转Hashes
        /// </summary>
        /// <param name="type">类型</param>
        public static implicit operator Hashes(string type)
        {
            return new Hashes(type);
        }
    }
}
