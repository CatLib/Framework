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
using System.IO;
using CatLib.API.IO;

namespace CatLib.IO
{
    /// <summary>
    /// 文件
    /// </summary>
    public sealed class File : IFile
    {
        /// <summary>
        /// 本地磁盘
        /// </summary>
        private readonly LocalDisk disk;

        /// <summary>
        /// 文件夹路径
        /// </summary>
        private string directory;

        /// <summary>
        /// 拓展名
        /// </summary>
        private string extension;

        /// <summary>
        /// 文件名
        /// </summary>
        private string fileName;

        /// <summary>
        /// 文件信息
        /// </summary>
        private FileInfo fileInfo;

        /// <summary>
        /// 文件信息
        /// </summary>
        private FileInfo FileInfo
        {
            get { return fileInfo ?? (fileInfo = new FileInfo(FullName)); }
        }

        /// <summary>
        /// 构建一个文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="disk">本地磁盘</param>
        public File(string path, LocalDisk disk)
        {
            this.disk = disk;
            ParseFile(path);
        }

        /// <summary>
		/// 文件拓展名
		/// </summary>
        public string Extension
        {
            get { return extension; }
        }

        /// <summary>
		/// 文件路径
		/// </summary>
        public string FullName
        {
            get { return directory + Path.AltDirectorySeparatorChar + fileName + extension; }
        }

        /// <summary>
		/// 文件名
		/// </summary>
        public string Name
        {
            get { return fileName + extension; }
        }

        /// <summary>
		/// 文件名不包含路径
		/// </summary>
        public string NameWithoutExtension
        {
            get { return fileName; }
        }

        /// <summary>
		/// 文件长度
		/// </summary>
        public long Length
        {
            get { return FileInfo.Length; }
        }

        /// <summary>
		/// 是否存在
		/// </summary>
        public bool Exists
        {
            get { return FileInfo.Exists; }
        }

        /// <summary>
		/// 所属的文件夹
		/// </summary>
        public IDirectory Directory
        {
            get { return new Directory(directory, disk); }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public void Delete()
        {
            FileInfo.Delete();
            Refresh();
        }

        /// <summary>
		/// 复制文件
		/// </summary>
		/// <param name="targetFileName">目标文件</param>
        public IFile CopyTo(string targetFileName)
        {
            IFile destFile = new File(targetFileName, disk);
            destFile.Directory.Create();
            FileInfo.CopyTo(targetFileName);
            return ((File)destFile).Refresh();
        }

        /// <summary>
		/// 复制文件
		/// </summary>
		/// <param name="targetDirectory">目标文件夹</param>
        public IFile CopyTo(IDirectory targetDirectory)
        {
            return CopyTo(targetDirectory + Path.AltDirectorySeparatorChar.ToString() + Name);
        }

        /// <summary>
		/// 移动到指定文件夹
		/// </summary>
		/// <param name="targetDirectory">目标文件夹</param>
        public void MoveTo(IDirectory targetDirectory)
        {
            MoveTo(targetDirectory.Path);
        }

        /// <summary>
		/// 移动到指定文件
		/// </summary>
		/// <param name="targetDirectory">目标文件夹</param>
        public void MoveTo(string targetDirectory)
        {
            LocalDisk.GuardValidatePath(targetDirectory);
            (new Directory(targetDirectory, disk)).Create();
            FileInfo.MoveTo(targetDirectory + Path.AltDirectorySeparatorChar + fileName + extension);
            directory = targetDirectory;
            Refresh();
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="newName">新的名字</param>
        public void Rename(string newName)
        {
            LocalDisk.IsValidFileName(newName);

            if (newName.Contains(Path.AltDirectorySeparatorChar.ToString()))
            {
                throw new ArgumentException("rename can't be used to change a files location use Move(string newPath) instead.");
            }

            var newExtension = Path.GetExtension(newName);
            var newFileName = Path.GetFileNameWithoutExtension(newName);

            IFile targetFile = new File(directory + Path.AltDirectorySeparatorChar + newFileName + newExtension, disk);
            if (targetFile.Exists)
            {
                throw new ArgumentException("duplicate file name:" + newName);
            }
            FileInfo.MoveTo(directory + Path.AltDirectorySeparatorChar + newFileName + newExtension);
            fileName = newFileName;
            extension = newExtension;

            Refresh();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="data">文件数据</param>
        public void Create(byte[] data)
        {
            if (Exists)
            {
                throw new IOException("file is exists");
            }
            data = Encrypted(data);
            using (var fs = System.IO.File.Create(FullName))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// 异步创建文件
        /// </summary>
        /// <param name="data">文件数据</param>
        /// <param name="callback">当完成时的回调</param>
        public void CreateAsync(byte[] data, Action callback)
        {
            if (Exists)
            {
                throw new IOException("file is exists");
            }

            data = Encrypted(data);

            var fs = System.IO.File.Create(FullName);
            fs.BeginWrite(data, 0, data.Length, (result) =>
            {
                fs.EndRead(result);
                fs.Close();
                fs.Dispose();

                if (callback != null)
                {
                    callback.Invoke();
                }
            }, null);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns>文件数据</returns>
        public byte[] Read()
        {
            if (!Exists)
            {
                throw new IOException("file is not exists : " + FullName);
            }

            using (var fs = new FileStream(FullName, System.IO.FileMode.Open))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                data = Decrypted(data);
                return data;
            }
        }

        /// <summary>
        /// 异步读取文件
        /// </summary>
        /// <param name="callback">读取的数据回调</param>
        public void ReadAsync(Action<byte[]> callback)
        {
            if (!Exists)
            {
                throw new IOException("file is not exists");
            }

            var fs = new FileStream(FullName,
                                            FileMode.Open,
                                            FileAccess.Read,
                                            FileShare.None,
                                            1024,
                                            true
                                            );

            var data = new byte[fs.Length];
            fs.BeginRead(data, 0, data.Length, (result) =>
            {
                fs.EndRead(result);
                data = Decrypted(data);
                fs.Close();
                fs.Dispose();
                callback.Invoke(data);
            }, null);
        }

        /// <summary>
        /// 刷新文件信息
        /// </summary>
        /// <returns>文件实例</returns>
        private IFile Refresh()
        {
            fileInfo = null;
            return this;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">文件数据</param>
        /// <returns>加密后的文件数据</returns>
        private byte[] Encrypted(byte[] data)
        {
            if (disk.IOCrypt != null)
            {
                data = disk.IOCrypt.Encrypted(FullName, data);
            }
            return data;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">加密的文件数据</param>
        /// <returns>解密后的文件数据</returns>
        private byte[] Decrypted(byte[] data)
        {
            if (disk.IOCrypt != null)
            {
                data = disk.IOCrypt.Decrypted(FullName, data);
            }
            return data;
        }

        /// <summary>
        /// 格式化路径
        /// </summary>
        /// <param name="path">文件路径</param>
        private void ParseFile(string path)
        {
            extension = Path.GetExtension(path);
            fileName = Path.GetFileNameWithoutExtension(path);
            directory = Path.GetDirectoryName(path);
        }
    }
}