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
using CatLib.API.FileSystem;
using CatLib.Stl;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件/文件夹句柄
    /// </summary>
    public abstract class Handler : IHandler
    {
        /// <summary>
        /// 文件/文件夹路径
        /// </summary>
        private string path;

        /// <summary>
        /// 文件/文件夹路径
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// 文件系统
        /// </summary>
        private FileSystem fileSystem;

        /// <summary>
        /// 文件系统
        /// </summary>
        protected FileSystem FileSystem
        {
            get { return fileSystem; }
        }

        /// <summary>
        /// 构造一个文件文件夹句柄
        /// </summary>
        /// <param name="fileSystem">文件系统</param>
        /// <param name="path">文件/文件夹路径</param>
        internal Handler(FileSystem fileSystem, string path)
        {
            Guard.NotNull(fileSystem, "fileSystem");
            Guard.NotEmptyOrNull(path, "path");
            this.path = path;
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// 复制文件或文件夹到指定路径
        /// </summary>
        /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
        /// <returns>是否成功</returns>
        public void Copy(string copyPath)
        {
            FileSystem.Copy(Path, copyPath);
        }

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <returns>是否成功</returns>
        public void Delete()
        {
            FileSystem.Delete(Path);
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="newName">新的文件/文件夹名字</param>
        /// <returns>是否成功</returns>
        public void Rename(string newName)
        {
            var newPath = System.IO.Path.GetDirectoryName(Path);
            newPath = System.IO.Path.Combine(newPath, newName);
            FileSystem.Rename(Path, newPath);
            path = newPath;
        }

        /// <summary>
        /// 文件/文件夹是否存在
        /// </summary>
        public bool IsExists
        {
            get { return FileSystem.Exists(Path); }
        }

        /// <summary>
        /// 获取文件/文件夹属性
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹属性</returns>
        public FileAttributes GetAttributes()
        {
            return FileSystem.GetAttributes(Path);
        }

        /// <summary>
        /// 是否是文件夹
        /// </summary>
        /// <returns>是否是文件夹</returns>
        public bool IsDir
        {
            get { return (GetAttributes() & FileAttributes.Directory) == FileAttributes.Directory; }
        }
    }
}
