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

namespace CatLib.API.Compress
{
    /// <summary>
    /// 压缩解压缩
    /// </summary>
    public interface ICompress
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes">需要压缩的字节流</param>
        /// <returns>压缩后的结果</returns>
        byte[] Compress(byte[] bytes);

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="bytes">需要解压缩的字节流</param>
        /// <returns>解压缩的结果</returns>
        byte[] Decompress(byte[] bytes);
    }
}
