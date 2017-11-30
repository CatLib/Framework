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
using CatLib._3rd.SharpCompress.Compressors;
using CatLib._3rd.SharpCompress.Compressors.LZMA;
using System.IO;

namespace CatLib.Compress
{
    /// <summary>
    /// Lzma压缩解压缩
    /// </summary>
    public sealed class LzmaAdapter : ICompress
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes">需要压缩的字节流</param>
        /// <returns>压缩后的结果</returns>
        public byte[] Compress(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var lzma = new LZipStream(ms, CompressionMode.Compress, true);
                lzma.Write(bytes, 0, bytes.Length);
                lzma.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="bytes">需要解压缩的字节流</param>
        /// <returns>解压缩的结果</returns>
        public byte[] Decompress(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var lzma = new LZipStream(new MemoryStream(bytes), CompressionMode.Decompress, true);
                var count = 0;
                var data = new byte[4096];
                while ((count = lzma.Read(data, 0, data.Length)) != 0)
                {
                    ms.Write(data, 0, count);
                }
                lzma.Close();
                return ms.ToArray();
            }
        }
    }
}
