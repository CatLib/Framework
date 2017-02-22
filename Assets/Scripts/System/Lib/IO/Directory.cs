using CatLib.API.IO;
using System;
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
        public DirectoryInfo DirectoryInfo
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
                return DirectoryInfo.Name;
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
                directoryPath = IO.NormalizePath(directoryPath);
                if (Exists(directoryPath))
                {
                    return new Directory(path + directoryPath);
                }
                else
                {
                    throw new DirectoryNotFoundException("a directory was not found at " + directoryPath);
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public IDirectory Refresh()
        {
            dir = null;
            return this;
        }

        /// <summary>
        /// 创建当前目录文件夹
        /// </summary>
        /// <returns></returns>
        public void Create()
        {
            if (!Exists())
            {
                DirectoryInfo.Create();
                Refresh();
            }
        }

        /// <summary>
        /// 创建子目录文件夹,如果文件夹已经存在那么不会进行任何操作
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        /// <returns></returns>
        public IDirectory Create(string directoryPath)
        {
            IO.ValidatePath(directoryPath);
            directoryPath = IO.NormalizePath(directoryPath);
            IDirectory directory = null;
            if (!Exists(path + directoryPath))
            {
                DirectoryInfo dir = new DirectoryInfo(path + directoryPath);
                dir.Create();
            }
            directory = new Directory(path + directoryPath);
            return directory;
        }

        /// <summary>
        /// 文件夹是否是空的
        /// </summary>
        public bool IsEmpty{

            get{

                int fileLength = DirectoryInfo.GetFiles().Length;
                if(fileLength > 0){ return false; }

                IDirectory drinfo;
                string[] infos = System.IO.Directory.GetDirectories(Path);
                for(int i = 0; i < infos.Length ; i++)
                {
                    drinfo = new Directory(infos[i]);
                    if(!drinfo.IsEmpty){ return false; }
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
            IO.ValidatePath(targetDirectroy);
            IDirectory dir = new Directory(targetDirectroy);
            dir.Create();

            string[] files = System.IO.Directory.GetFiles(Path);

            string fileName;
            string destFile;

            foreach (string s in files)
            {
                fileName = System.IO.Path.GetFileName(s);
                destFile = targetDirectroy + IO.PATH_SPLITTER +  fileName;
                System.IO.File.Copy(s, destFile, true);
            }

            IDirectory drinfo;
            foreach (string info in System.IO.Directory.GetDirectories(Path))
            {
                drinfo = new Directory(info);
                drinfo.CopyTo(targetDirectroy + IO.PATH_SPLITTER + drinfo.Name);
            }

            return dir.Refresh();
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public void Delete()
        {
            if(DirectoryInfo.Exists){
                DirectoryInfo.Delete(true);
                Refresh();
            }
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
        {   
            return DirectoryInfo.Exists;
        }

        /// <summary>
        /// 子文件夹是否存在
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public bool Exists(string directoryPath)
        {
            IO.ValidatePath(directoryPath);
            directoryPath = IO.NormalizePath(directoryPath);
            DirectoryInfo dir = new DirectoryInfo(path + directoryPath);
            return dir.Exists;
        }

        /// <summary>
        /// 获取当前目录所有的文件
        /// </summary>
        /// <returns></returns>
        public IFile[] GetFiles(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return GetFiles("*" , option);
        }

        /// <summary>
        /// 获取当前目录顶层所有的文件
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <returns></returns>
        public IFile[] GetFiles(string filter , SearchOption option)
        {
            if(!DirectoryInfo.Exists){ return new File[]{}; }
            FileInfo[] files = DirectoryInfo.GetFiles(filter, option);
            IFile[] returnData = new IFile[files.Length];
            for(int i = 0; i < files.Length; i++){
                returnData[i] = IO.MakeFile(files[i].FullName);
            }
            return returnData;
        }


        /// <summary>
        /// 将当前目录移动到目标目录
        /// </summary>
        /// <param name="targetDirectory">目标文件夹</param>
        public void MoveTo(string targetDirectory)
        {
            int start = targetDirectory.LastIndexOf(IO.PATH_SPLITTER) + 1;
            int length = targetDirectory.Length - start;
            string name = targetDirectory.Substring(start, length);

            if (!IO.IsValidFileName(name))
            {
                throw new ArgumentException("the name '" + name + "' contains invalid characters");
            }

            IDirectory moveToDirectory = new Directory(targetDirectory.Substring(0,targetDirectory.Length - name.Length - 1));
            moveToDirectory.Create();

            DirectoryInfo.MoveTo(targetDirectory);
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

            if (newName.Contains(IO.PATH_SPLITTER.ToString()))
            {
                throw new ArgumentException("rename can't be used to change a files location use Move(string newPath) instead.");
            }

            string subPath = Path.Substring(0, Path.LastIndexOf(IO.PATH_SPLITTER) + 1);
            string newPath = subPath + newName;

            DirectoryInfo.MoveTo(newPath);
            path = newPath;
            Refresh();
        }

        public void Walk(Action<IFile> callBack , SearchOption option)
        {
            Walk(callBack , "*" , option);
        }

        public void Walk(Action<IFile> callBack , string filter = "*" , SearchOption option = SearchOption.AllDirectories)
        {
            IFile[] files = GetFiles(filter , option);
            for(int i = 0 ; i < files.Length ; i++){

                callBack.Invoke(files[i]);

            }
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

    }

}