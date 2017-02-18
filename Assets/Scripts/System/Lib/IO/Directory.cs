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
		/// 文件夹是否存在
		/// </summary>
		public bool Exists(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.Exists;
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
                if (!System.IO.Directory.Exists(Path.GetDirectoryName(targetPath)))
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
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