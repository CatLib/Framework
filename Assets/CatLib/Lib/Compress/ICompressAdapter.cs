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

namespace CatLib.Compress
{
    /// <summary>
    /// 压缩适配器
    /// </summary>
    public interface ICompressAdapter
    {
        /// <summary>
        /// 对字节序进行压缩
        /// </summary>
        /// <param name="bytes">需要被压缩的字节序</param>
        /// <param name="level">压缩等级(0-9)</param>
        /// <returns>被压缩后的字节序</returns>
        byte[] Compress(byte[] bytes, int level);

        /// <summary>   
        /// 对文件列表进行压缩   
        /// </summary>   
        /// <param name="fileNames">要打包的文件列表</param>   
        /// <param name="gzipFileName">压缩后的文件名</param>   
        /// <param name="sleepTimer">休眠时间（单位毫秒）</param>        
        /// /// <param name="compressionLevel">压缩品质级别（0~9）</param>   
        void Compress(System.Collections.Generic.List<System.IO.FileInfo> fileNames, string gzipFileName, int sleepTimer, int compressionLevel = 6);

        /// <summary>
        /// 对目录进行压缩
        /// </summary>
        /// <param name="directory">被压缩的路径</param>
        /// <param name="gzipFileName">目标文件名</param>
        /// <param name="compressionLevel">压缩品质级别（0~9）</param>
        void CompressDirectory(string directory, string gzipFileName, int compressionLevel = 6);

        /// <summary>
        /// 解压缩字节序
        /// </summary>
        /// <param name="bytes">被压缩的字节序</param>
        /// <returns>被解压的字节序</returns>
        byte[] UnCompress(byte[] bytes);

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="gzipFile">压缩包文件名</param>
        /// <returns>返回解压的路径</returns>
        string UnCompress(string gzipFile);

        /// <summary>   
        /// 解压缩文件   
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>   
        /// <param name="targetPath">解压缩目标路径</param>          
        void UnCompress(string gzipFile, string targetPath);
        /// <summary>   
        /// 解压缩文件到当前目录 
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>
        /// <returns>返回解压的路径</returns>
        string UnCompressToCurrentDirectory(string gzipFile);

    }
}
