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

using CatLib.API.IO;
using System;
using System.IO;

namespace CatLib.IO
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public sealed class Directory : IDirectory
    {
        /// <summary>
        /// 本地磁盘
        /// </summary>
        private readonly LocalDisk disk;

        /// <summary>
        /// 文件夹目录位置
        /// </summary>
        private string path;

        /// <summary>
        /// 文件夹信息
        /// </summary>
        private DirectoryInfo dir;

        /// <summary>
        /// 文件夹信息
        /// </summary>
        private DirectoryInfo DirectoryInfo
        {
            get { return dir ?? (dir = new DirectoryInfo(path)); }
        }

        /// <summary>
        /// 文件夹目录位置
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// 文件夹名字
        /// </summary>
        public string Name
        {
            get { return DirectoryInfo.Name; }
        }

        /// <summary>
        /// 构建一个文件夹
        /// </summary>
        /// <param name="directoryPath">默认根路径</param>
        /// <param name="disk">本地磁盘</param>
        public Directory(string directoryPath, LocalDisk disk)
        {
            LocalDisk.GuardValidatePath(directoryPath);
            this.disk = disk;
            path = directoryPath;
        }

        /// <summary>
        /// 返回此目录的子目录（如果存在，反之抛出一个异常）
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        /// <returns>子目录文件夹</returns>
        public IDirectory this[string directoryPath]
        {
            get
            {
                LocalDisk.GuardValidatePath(directoryPath);
                directoryPath = LocalDisk.NormalizePath(directoryPath);
                if (Exists(directoryPath))
                {
                    return new Directory(path + directoryPath, disk);
                }
                else
                {
                    throw new DirectoryNotFoundException("a directory was not found at " + directoryPath);
                }
            }
        }

        /// <summary>
        /// 刷新文件夹信息
        /// </summary>
        /// <returns>文件夹实例</returns>
        private IDirectory Refresh()
        {
            dir = null;
            return this;
        }

        /// <summary>
        /// 创建文件夹,如果文件夹已经存在那么不会进行任何操作
        /// </summary>
        public void Create()
        {
            if (Exists())
            {
                return;
            }
            DirectoryInfo.Create();
            Refresh();
        }

        /// <summary>
        /// 创建子目录文件夹,如果文件夹已经存在那么不会进行任何操作
        /// </summary>
        /// <param name="directoryPath">子目录相对路径</param>
        /// <returns>子文件夹实例</returns>
        public IDirectory Create(string directoryPath)
        {
            LocalDisk.GuardValidatePath(directoryPath);
            directoryPath = LocalDisk.NormalizePath(directoryPath);
            IDirectory directory = null;
            if (!Exists(path + directoryPath))
            {
                var dir = new DirectoryInfo(path + directoryPath);
                dir.Create();
            }
            directory = new Directory(path + directoryPath, disk);
            return directory;
        }

        /// <summary>
        /// 文件夹是否是空的
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                var fileLength = DirectoryInfo.GetFiles().Length;
                if (fileLength > 0)
                {
                    return false;
                }

                IDirectory dirInfo;
                var infos = System.IO.Directory.GetDirectories(Path);
                for (var i = 0; i < infos.Length; i++)
                {
                    dirInfo = new Directory(infos[i], disk);
                    if (!dirInfo.IsEmpty)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="targetDirectroy">目标文件夹</param>
        public IDirectory CopyTo(string targetDirectroy)
        {
            LocalDisk.GuardValidatePath(targetDirectroy);
            IDirectory dir = new Directory(targetDirectroy, disk);
            dir.Create();

            var files = System.IO.Directory.GetFiles(Path);

            string fileName;
            string destFile;
            foreach (var s in files)
            {
                fileName = System.IO.Path.GetFileName(s);
                destFile = targetDirectroy + System.IO.Path.AltDirectorySeparatorChar + fileName;
                System.IO.File.Copy(s, destFile, true);
            }

            IDirectory drinfo;
            foreach (var info in System.IO.Directory.GetDirectories(Path))
            {
                drinfo = new Directory(info, disk);
                drinfo.CopyTo(targetDirectroy + System.IO.Path.AltDirectorySeparatorChar + drinfo.Name);
            }

            return ((Directory)dir).Refresh();
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public void Delete()
        {
            if (!DirectoryInfo.Exists)
            {
                return;
            }
            DirectoryInfo.Delete(true);
            Refresh();
        }

        /// <summary>
        /// 删除子目录文件夹
        /// </summary>
        /// <param name="directoryPath">子目录相对路径</param>
        public void Delete(string directoryPath)
        {
            var directoryToDelete = this[directoryPath];
            directoryToDelete.Delete();
        }

        /// <summary>
        /// 当前文件夹是否存在
        /// </summary>
        /// <returns>文件夹是否存在</returns>
        public bool Exists()
        {
            return DirectoryInfo.Exists;
        }

        /// <summary>
        /// 获取文件夹内的指定文件
        /// </summary>
        /// <returns>文件</returns>
        public IFile File(string path)
        {
            return disk.File(this.path + System.IO.Path.AltDirectorySeparatorChar + path, PathTypes.Absolute);
        }

        /// <summary>
        /// 子文件夹是否存在
        /// </summary>
        /// <param name="directoryPath">子文件夹的相对路径</param>
        /// <returns>是否存在</returns>
        public bool Exists(string directoryPath)
        {
            LocalDisk.GuardValidatePath(directoryPath);
            directoryPath = LocalDisk.NormalizePath(directoryPath);
            var dir = new DirectoryInfo(path + directoryPath);
            return dir.Exists;
        }

        /// <summary>
        /// 获取当前目录所有的文件
        /// </summary>
        /// <param name="option">搜索选项</param>
        /// <returns>文件数组</returns>
        public IFile[] GetFiles(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return GetFiles("*", option);
        }

        /// <summary>
        /// 获取当前目录顶层所有的文件
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <param name="option">搜索选项</param>
        /// <returns>文件数组</returns>
        public IFile[] GetFiles(string filter, SearchOption option)
        {
            if (!DirectoryInfo.Exists) { return new File[] { }; }
            var files = DirectoryInfo.GetFiles(filter, option);
            var returnData = new IFile[files.Length];
            for (var i = 0; i < files.Length; i++)
            {
                returnData[i] = new File(files[i].FullName, disk);
            }
            return returnData;
        }

        /// <summary>
        /// 将当前目录移动到目标目录
        /// </summary>
        /// <param name="targetDirectory">目标文件夹</param>
        public void MoveTo(string targetDirectory)
        {
            var start = targetDirectory.LastIndexOf(System.IO.Path.AltDirectorySeparatorChar) + 1;
            var length = targetDirectory.Length - start;
            var name = targetDirectory.Substring(start, length);

            if (!LocalDisk.IsValidFileName(name))
            {
                throw new ArgumentException("the name '" + name + "' contains invalid characters");
            }

            IDirectory moveToDirectory = new Directory(targetDirectory.Substring(0, targetDirectory.Length - name.Length - 1), disk);
            moveToDirectory.Create();

            DirectoryInfo.MoveTo(targetDirectory);
            path = targetDirectory;
            Refresh();
        }

        /// <summary>
        /// 重命名当前文件夹
        /// </summary>
        /// <param name="newName">新的文件夹名字</param>
        public void Rename(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException("you can't send a empty or null string to rename an asset. trying to rename " + path);
            }

            if (!LocalDisk.IsValidFileName(newName))
            {
                throw new ArgumentException("the name '" + newName + "' contains invalid characters");
            }

            if (newName.Contains(System.IO.Path.AltDirectorySeparatorChar.ToString()))
            {
                throw new ArgumentException("rename can't be used to change a files location use Move(string newPath) instead.");
            }

            var subPath = Path.Substring(0, Path.LastIndexOf(System.IO.Path.AltDirectorySeparatorChar) + 1);
            var newPath = subPath + newName;

            DirectoryInfo.MoveTo(newPath);
            path = newPath;
            Refresh();
        }

        /// <summary>
        /// 查找文件并调用指定callback
        /// </summary>
        /// <param name="callBack">回调</param>
        /// <param name="option">搜索选项</param>
        public void Walk(Action<IFile> callBack, SearchOption option)
        {
            Walk(callBack, "*", option);
        }

        /// <summary>
        /// 查找文件并调用指定callback
        /// </summary>
        /// <param name="callBack">回调</param>
        /// <param name="filter">过滤</param>
        /// <param name="option">搜索选项</param>
        public void Walk(Action<IFile> callBack, string filter = "*", SearchOption option = SearchOption.AllDirectories)
        {
            var files = GetFiles(filter, option);
            for (var i = 0; i < files.Length; i++)
            {
                callBack.Invoke(files[i]);
            }
        }

        /// <summary>
        /// 当转换为字符串时，它将返回它的完整路径。
        /// </summary>
        /// <returns>文件夹完整路径</returns>
        public override string ToString()
        {
            return Path;
        }

        /// <summary>
        /// 比较这2个目录路径
        /// </summary>
        /// <param name="left">左侧文件夹</param>
        /// <param name="right">右侧文件夹</param>
        /// <returns>比较结果</returns>
        public int Compare(IDirectory left, IDirectory right)
        {
            return string.CompareOrdinal(left.Path, right.Path);
        }

        /// <summary>
        /// 检查这2个目录是否指向同一个目录
        /// </summary>
        /// <param name="other">目录</param>
        /// <returns>是否相等</returns>
        public bool Equals(IDirectory other)
        {
            return string.CompareOrdinal(Path, other.Path) == 0;
        }

        /// <summary>
        /// 与object比较是否相等
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            var directory = obj as IDirectory;
            if (directory != null)
            {
                return string.CompareOrdinal(directory.Path, Path) == 0;
            }
            return false;
        }

        /// <summary>
        /// 返回文件夹的 hash code
        /// </summary>
        /// <returns>hash值</returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}