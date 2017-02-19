using CatLib.Contracts.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace CatLib.IO
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public class Directory : IDirectory
    {

        /// <summary>
        /// 当前目录文件夹
        /// </summary>
        private string path;

        private DirectoryInfo dir;
        /// <summary>
        /// 当前目录文件夹
        /// </summary>
        private DirectoryInfo Dir
        {
            get
            {
                if(dir == null)
                {
                    dir = new DirectoryInfo(path);
                }
                return dir;
            }
        }

        /// <summary>
        /// 文件夹根路径
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
            get
            {
                return Dir.Name;
            }
        }

        /// <summary>
        /// 文件夹服务
        /// </summary>
        /// <param name="directoryPath">默认根路径</param>
        public Directory(string directoryPath)
        {
            IO.ValidatePath(directoryPath);
            path = directoryPath;
        }

        /// <summary>
        /// 返回此目录的子目录（如果存在，反之抛出一个异常）
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        /// <returns></returns>
        public IDirectory this[string directoryPath]
        {
            get
            {
                IO.ValidatePath(directoryPath);
                if (ExistsNew(directoryPath))
                {
                    return new Directory(path + IO.PATH_SPLITTER + directoryPath);
                }
                else
                {
                    throw new DirectoryNotFoundException("a directory was not found at " + directoryPath);
                }
            }
        }

        /// <summary>
        /// 创建当前目录文件夹
        /// </summary>
        /// <returns></returns>
        public void Create()
        {
            if (!Exists())
            {
                Dir.Create();
                Refresh();
            }
        }

        public IDirectory Refresh()
        {
            dir = null;
            return this;
        }

        /// <summary>
        /// 创建子目录文件夹,如果文件夹已经存在那么不会进行任何操作
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        /// <returns></returns>
        public IDirectory Create(string directoryPath)
        {
            IDirectory directory = null;
            if (!ExistsNew(path + IO.PATH_SPLITTER + directoryPath))
            {
                DirectoryInfo dir = new DirectoryInfo(path + IO.PATH_SPLITTER + directoryPath);
                dir.Create();
            }
            directory = new Directory(path + IO.PATH_SPLITTER + directoryPath);
            return directory;
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="copyDirectroy"></param>
        public IDirectory Duplicate(string copyDirectroy)
        {
            IDirectory dir = new Directory(copyDirectroy);
            dir.Create();

            string[] files = System.IO.Directory.GetFiles(this);

            string fileName;
            string destFile;

            foreach (string s in files)
            {
                fileName = System.IO.Path.GetFileName(s);
                destFile = copyDirectroy + IO.PATH_SPLITTER +  fileName;
                System.IO.File.Copy(s, destFile, true);
            }

            IDirectory drinfo;
            foreach (string info in System.IO.Directory.GetDirectories(this))
            {
                drinfo = new Directory(info);
                drinfo.Duplicate(copyDirectroy + IO.PATH_SPLITTER + drinfo.Name);
            }

            return dir.Refresh();
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public void Delete()
        {
            Dir.Delete(true);
            Refresh();
        }

        /// <summary>
        /// 删除子目录文件夹
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        public void Delete(string directoryPath)
        {
            IDirectory directoryToDelete = this[directoryPath];
            directoryToDelete.Delete();
        }

        /// <summary>
        /// 当前文件夹是否存在
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {   //todo 需要改为属性
            return Dir.Exists;
        }

        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public bool ExistsNew(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(path + IO.PATH_SPLITTER + path);
            return dir.Exists;
        }

        /// <summary>
        /// 获取当前目录所有的文件
        /// </summary>
        /// <returns></returns>
        public IFile[] GetFiles(bool recursive = false)
        {
            return null;
        }

        /// <summary>
        /// 获取当前目录顶层所有的文件
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <returns></returns>
        public IFile[] GetFiles(string filter , bool recursive = false)
        {
            return null;
        }


        /// <summary>
        /// 将当前目录移动到目标目录
        /// </summary>
        /// <param name="targetDirectory">目标文件夹</param>
        public void Move(string targetDirectory)
        {
            int start = targetDirectory.LastIndexOf('/') + 1;
            int length = targetDirectory.Length - start;
            string name = targetDirectory.Substring(start, length);

            if (!IO.IsValidFileName(name))
            {
                throw new ArgumentException("the name '" + name + "' contains invalid characters");
            }

            Dir.MoveTo(targetDirectory);
            path = targetDirectory;
            Refresh();
        }

        /// <summary>
        /// 重命名当前文件夹
        /// </summary>
        /// <param name="newName"></param>
        public void Rename(string newName)
        {

            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException("you can't send a empty or null string to rename an asset. trying to rename " + path);
            }

            if (!IO.IsValidFileName(newName))
            {
                throw new ArgumentException("the name '" + newName + "' contains invalid characters");
            }

            if (newName.Contains("/"))
            {
                throw new ArgumentException("rename can't be used to change a files location use Move(string newPath) instead.");
            }

            string subPath = Path.Substring(0, Path.LastIndexOf(IO.PATH_SPLITTER) + 1);
            string newPath = subPath + newName;

            Dir.MoveTo(newPath);
            path = newPath;
            Refresh();
        }

        /// <summary>
        /// 当转换为字符串时，它将返回它的完整路径。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Path;
        }

        /// <summary>
        /// 比较这2个目录
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(IDirectory x, IDirectory y)
        {
            return string.Compare(x.Path, y.Path);
        }

        /// <summary>
        /// 检查这2个目录是否指向同一个目录
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IDirectory other)
        {
            return string.CompareOrdinal(Path, other.Path) == 0;
        }

        /// <summary>
        /// 允许2个文件夹类型比较
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Directory lhs, Directory rhs)
        {
            return string.CompareOrdinal(lhs.Path, rhs.Path) == 0;
        }

        /// <summary>
        /// 允许2个文件夹类型比较
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Directory lhs, Directory rhs)
        {
            return string.CompareOrdinal(lhs.Path, rhs.Path) == 0;
        }

        /// <summary>
        /// 允许由字符串转为文件夹
        /// </summary>
        /// <param name="directory"></param>
        public static explicit operator Directory(string directory)
        {
            return new Directory(directory);
        }

        /// <summary>
        /// 允许由文件夹转为字符串
        /// </summary>
        /// <param name="directory"></param>
        public static implicit operator string(Directory directory)
        {
            return directory.Path;
        }

        /// <summary>
        /// 与object比较是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is IDirectory)
            {
                return string.Compare(((IDirectory)obj).Path, Path) == 0;
            }
            return false;
        }

        /// <summary>
        /// 返回这个类的 hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }


        //以下为被淘汰的代码







        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        public bool Exists(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.Exists;
        }



        /// <summary>遍历目录及其子目录复制到目标路径</summary>
		/// <param name="path">起始路径</param>
		/// <param name="copyPath">目标路径</param>
		public void CopyTo(string path, string targetPath)
        {
            if (Exists(path))
            {
                CopyTo(path, path, targetPath);
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isOverride">是否覆盖</param>
		public void Create(string path, bool isOverride = false)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                if (!isOverride) { return; }
                dir.Delete(true);
            }
            dir.Create();
        }

        /// <summary>
		/// 遍历文件夹中的所有文件
		/// </summary>
        public FileInfo[] Walk(string path)
        {
            List<FileInfo> returnList = new List<FileInfo>();
            if (!Exists(path)) { return returnList.ToArray(); }
            DirectoryInfo folder = new DirectoryInfo(path);
            FileSystemInfo[] files = folder.GetFileSystemInfos();
            int length = files.Length;
            for (int i = 0; i < length; i++)
            {

                if (files[i] is DirectoryInfo)
                {

                    returnList.AddRange(Walk(files[i].FullName));

                }
                else
                {

                    returnList.Add(new FileInfo(files[i].FullName));

                }

            }
            return returnList.ToArray();
        }


        /// <summary>
		/// 遍历文件夹中的所有文件
		/// </summary>
		public void Walk(string path, Action<FileInfo> callBack)
        {

            DirectoryInfo folder = new DirectoryInfo(path);
            FileSystemInfo[] files = folder.GetFileSystemInfos();
            int length = files.Length;
            for (int i = 0; i < length; i++)
            {

                if (files[i] is DirectoryInfo)
                {

                    Walk(files[i].FullName, callBack);

                }
                else
                {

                    callBack(new FileInfo(files[i].FullName));

                }

            }

        }


        /// <summary>遍历目录及其子目录</summary>
		/// <param name="root">起始结点</param>
		/// <param name="path">起始路径</param>
		/// <param name="copyPath">复制路径</param>
		private void CopyTo(string root, string path, string copyPath)
        {
            root = root.Replace('\\', '/');
            path = path.Replace('\\', '/');
            copyPath = copyPath.Replace('\\', '/');

            string[] names = System.IO.Directory.GetFiles(path);
            string[] dirs = System.IO.Directory.GetDirectories(path);

            foreach (string filename in names)
            {
                string name = filename.Replace('\\', '/');
                string targetPath = copyPath + name.Replace(root, string.Empty);
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(targetPath)))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetPath));
                }
                System.IO.File.Copy(filename, targetPath);
            }
            foreach (string dir in dirs)
            {
                CopyTo(root, dir, copyPath);
            }
        }
    }

}