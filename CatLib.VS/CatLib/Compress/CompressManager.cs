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

using CatLib.API.Compress;

namespace CatLib.Compress
{
    /// <summary>
    /// 压缩解压缩
    /// </summary>
    internal sealed class CompressManager : SingleManager<ICompress>, ICompressManager
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes">需要压缩的字节流</param>
        /// <param name="name">使用的压缩解压缩名字</param>
        /// <returns>压缩后的结果</returns>
        public byte[] Compress(byte[] bytes, string name = null)
        {
            return Get(name).Compress(bytes);
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="bytes">需要解压缩的字节流</param>
        /// <param name="name">使用的压缩解压缩名字</param>
        /// <returns>解压缩的结果</returns>
        public byte[] Decomporess(byte[] bytes, string name = null)
        {
            return Get(name).Decompress(bytes);
        }
    }
}
