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

using System;
using System.Collections.Generic;
using System.IO;
using CatLib.API.Compress;
using ICSharpCode.SharpZipLib.Zip;

namespace CatLib.Compress
{
    /// <summary>
    /// 压缩服务
    /// </summary>
    public sealed class CompressService : ICompress
    {
        /// <summary>
        /// 压缩服务适配器，接入第三方的压缩组件
        /// </summary>
        private readonly ICompressAdapter compress;

        /// <summary>
        /// 构造一个压缩服务
        /// </summary>
        /// <param name="adapter">适配器</param>
        public CompressService(ICompressAdapter adapter)
        {
            compress = adapter;
        }

        /// <summary>
        /// 对字节序进行压缩
        /// </summary>
        /// <param name="bytes">需要被压缩的字节序</param>
        /// <param name="level">压缩等级(0-9)</param>
        /// <returns>被压缩后的字节序</returns>
        public byte[] Compress(byte[] bytes, int level = 6)
        {
            if (bytes == null)
            {
                return null;
            }
            return bytes.Length <= 0 ? bytes : compress.Compress(bytes, level);
        }

        /// <summary>
        /// 解压缩字节序
        /// </summary>
        /// <param name="bytes">被压缩的字节序</param>
        /// <returns>被解压的字节序</returns>
        public byte[] UnCompress(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return bytes.Length <= 0 ? bytes : compress.UnCompress(bytes);
        }

        /// <summary>
        /// 对目录进行压缩
        /// </summary>
        /// <param name="directory">被压缩的路径</param>
        /// <param name="gzipFileName">目标文件名</param>
        /// <param name="compressionLevel">压缩品质级别（0~9）</param>
        public void CompressDirectory(string directory, string gzipFileName, int compressionLevel = 6)
        {
            if (!string.IsNullOrEmpty(gzipFileName))
            {
                if (compressionLevel <= 9 && compressionLevel >= 0)
                {
                    compress.CompressDirectory(directory, gzipFileName, compressionLevel);
                }
                else
                {
                    compress.CompressDirectory(directory, gzipFileName);
                }
            }
        }


        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileNames">要打包的文件列表</param>   
        /// <param name="gzipFileName">目标文件名</param>   
        /// <param name="sleepTimer">休眠时间（单位毫秒）</param>        
        /// /// <param name="compressionLevel">压缩品质级别（0~9）</param>   
        public void Compress(List<FileInfo> fileNames, string gzipFileName, int sleepTimer, int compressionLevel = 6)
        {
            compress.Compress(fileNames, gzipFileName, sleepTimer, compressionLevel);
        }



        /// <summary>   
        /// 解压缩文件   
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>   
        public string UnCompress(string gzipFile)
        {
            return compress.UnCompress(gzipFile);
        }

        /// <summary>   
        /// 解压缩文件到当前目录 
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>
        public string UnCompressToCurrentDirectory(string gzipFile)
        {
            return compress.UnCompressToCurrentDirectory(gzipFile);
        }

        /// <summary>   
        /// 解压缩文件   
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>   
        /// <param name="targetPath">解压缩目标路径</param>          
        public void UnCompress(string gzipFile, string targetPath)
        {
            compress.UnCompress(gzipFile, targetPath);
        }

    }
}

