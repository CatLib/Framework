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
using ICSharpCode.SharpZipLib.GZip;

namespace CatLib.Compress
{
    /// <summary>
    /// ShareZipLib 适配器
    /// </summary>
    public sealed class ShareZipLibAdapter : ICompressAdapter
    {
        /// <summary>
        /// 对字节序进行压缩
        /// </summary>
        /// <param name="bytes">需要被压缩的字节序</param>
        /// <param name="level">压缩等级(0-9)</param>
        /// <returns>被压缩后的字节序</returns>
        public byte[] Compress(byte[] bytes, int level)
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
        /// 解压缩字节序
        /// </summary>
        /// <param name="bytes">被压缩的字节序</param>
        /// <returns>被解压的字节序</returns>
        public byte[] UnCompress(byte[] bytes)
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