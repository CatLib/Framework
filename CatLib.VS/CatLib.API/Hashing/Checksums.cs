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
    public enum Checksums
    {
        /// <summary>
        /// Adler32
        /// </summary>
        Adler32,

        /// <summary>
        /// BZip2Crc32
        /// </summary>
        BZip2Crc32,

        /// <summary>
        /// Crc32
        /// </summary>
        Crc32,
    }
}
