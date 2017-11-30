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

using System.IO;
using CatLib.API.Compress;
using CatLib._3rd.ICSharpCode.SharpZipLib.GZip;

namespace CatLib.Compress
{
    /// <summary>
    /// Gzip压缩解压缩
    /// </summary>
    public sealed class GZipAdapter : ICompress
    {
        /// <summary>
        /// 压缩等级
        /// </summary>
        private readonly int level;

        /// <summary>
        /// 压缩解压缩
        /// </summary>
        /// <param name="level">压缩等级</param>
        public GZipAdapter(int level = 6)
        {
            this.level = level;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes">需要压缩的字节流</param>
        /// <returns>压缩后的结果</returns>
        public byte[] Compress(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var gzip = new GZipOutputStream(ms);
                gzip.Write(bytes, 0, bytes.Length);
                gzip.SetLevel(level);
                gzip.Close();

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
                var gzip = new GZipInputStream(new MemoryStream(bytes));
                var count = 0;
                var data = new byte[4096];
                while ((count = gzip.Read(data, 0, data.Length)) != 0)
                {
                    ms.Write(data, 0, count);
                }
                return ms.ToArray();
            }
        }
    }
}
