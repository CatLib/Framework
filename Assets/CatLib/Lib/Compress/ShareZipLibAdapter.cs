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
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;

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


        /// <summary>
        /// 对目录进行压缩
        /// </summary>
        /// <param name="directory">被压缩的路径</param>
        /// <param name="gzipFileName">目标文件名</param>
        /// <param name="compressionLevel">压缩品质级别（0~9）</param>
        public void CompressDirectory(string directory, string gzipFileName, int compressionLevel = 6)
        {
            if (Directory.Exists(directory))
            {
                string[] fileStrings = Directory.GetDirectories(directory);
                List<FileInfo> fileInfos = new List<FileInfo>();
                for (int index = 0; index < fileStrings.Length; index++)
                {
                    string fileString = fileStrings[index];
                    fileInfos.Add(new FileInfo(fileString));
                }
                Compress(fileInfos, gzipFileName, 0, compressionLevel);
            }
            else
            {
                throw new DirectoryNotFoundException(directory + " Not Found ！");
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
            ZipOutputStream s = new ZipOutputStream(File.Create(gzipFileName));
            try
            {
                s.SetLevel(compressionLevel);   //0 - store only to 9 - means best compression   
                foreach (FileInfo file in fileNames)
                {
                    FileStream fs = null;
                    try
                    {
                        fs = file.Open(FileMode.Open, FileAccess.ReadWrite);
                    }
                    catch
                    {
                        continue;
                    }
                    //  方法二，将文件分批读入缓冲区   
                    byte[] data = new byte[2048];
                    int size = 2048;
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file.Name));
                    entry.DateTime = (file.CreationTime > file.LastWriteTime ? file.LastWriteTime : file.CreationTime);
                    s.PutNextEntry(entry);
                    while (true)
                    {
                        size = fs.Read(data, 0, size);
                        if (size <= 0)
                        {
                            break;
                        }
                        s.Write(data, 0, size);
                    }
                    fs.Flush();
                    fs.Close();
                    //file.Delete();
                    System.Threading.Thread.Sleep(sleepTimer);
                }
            }
            finally
            {
                s.Finish();
                s.Close();
            }
        }



        /// <summary>   
        /// 解压缩文件   
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>   
        public string UnCompress(string gzipFile)
        {
            string targetPath = string.Format("{0}{1}{2}{3}", Path.GetDirectoryName(gzipFile), Path.AltDirectorySeparatorChar, Path.GetFileNameWithoutExtension(gzipFile), Path.AltDirectorySeparatorChar);
            UnCompress(gzipFile, targetPath);
            return targetPath;
        }

        /// <summary>   
        /// 解压缩文件到当前目录 
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>
        public string UnCompressToCurrentDirectory(string gzipFile)
        {
            string targetPath = string.Format("{0}{1}", Path.GetDirectoryName(gzipFile), Path.AltDirectorySeparatorChar);
            UnCompress(gzipFile, targetPath);
            return targetPath;
        }

        /// <summary>   
        /// 解压缩文件   
        /// </summary>   
        /// <param name="gzipFile">压缩包文件名</param>   
        /// <param name="targetPath">解压缩目标路径</param>          
        public void UnCompress(string gzipFile, string targetPath)
        {
            //string directoryName = Path.GetDirectoryName(targetPath + "\\") + "\\";   
            string directoryName = targetPath;
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);//生成解压目录   
            }
            string currentDirectory = directoryName;
            byte[] data = new byte[2048];
            int size = 2048;
            using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(gzipFile)))
            {
                ZipEntry theEntry = null;
                while ((theEntry = zipInputStream.GetNextEntry()) != null)
                {
                    if (theEntry.IsDirectory)
                    {
                        // 该结点是目录   
                        if (!Directory.Exists(currentDirectory + theEntry.Name))
                        {
                            Directory.CreateDirectory(currentDirectory + theEntry.Name);
                        }
                    }
                    else
                    {
                        if (theEntry.Name != string.Empty)
                        {
                            //解压文件到指定的目录   
                            using (FileStream streamWriter = File.Create(currentDirectory + theEntry.Name))
                            {
                                while (true)
                                {
                                    size = zipInputStream.Read(data, 0, data.Length);
                                    if (size <= 0)
                                    {
                                        break;
                                    }

                                    streamWriter.Write(data, 0, size);
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                zipInputStream.Close();
            }
        }

    }
}