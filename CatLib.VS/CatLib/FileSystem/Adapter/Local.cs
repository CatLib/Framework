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
using CatLib.API;
using CatLib.API.FileSystem;
using CatLib.Stl;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 本地驱动器
    /// </summary>
    public class Local : IFileSystemAdapter
    {
        /// <summary>
        /// 根目录
        /// </summary>
        private readonly string root;

        /// <summary>
        /// 构建一个本地驱动器
        /// </summary>
        /// <param name="root">根目录</param>
        public Local(string root)
        {
            Guard.NotEmptyOrNull(root, "root");
            this.root = root;
        }

        /// <summary>
        /// 文件或文件夹是否存在
        /// </summary>
        /// <param name="path">文件或文件夹是否存在</param>
        /// <returns>是否存在</returns>
        public bool Has(string path)
        {
            Guard.NotEmptyOrNull(path, "path");
            path = Path.Combine(root, path);
            GuardLimitedRoot(path);
            return File.Exists(path) || Directory.Exists(path);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="contents">写入数据</param>
        /// <returns>是否成功</returns>
        public bool Write(string path, byte[] contents)
        {
            Guard.NotEmptyOrNull(path, "path");
            Guard.NotNull(contents, "contents");
            path = Path.Combine(root, path);
            GuardLimitedRoot(path);
            EnsureDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, contents);
            return true;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>读取的数据</returns>
        public byte[] Read(string path)
        {
            Guard.NotEmptyOrNull(path, "path");
            path = Path.Combine(root, path);
            GuardLimitedRoot(path);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            throw new FileNotFoundException("File is not exists " + path);
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="path">旧的文件/文件夹路径</param>
        /// <param name="newPath">新的文件/文件夹路径</param>
        /// <returns>是否成功</returns>
        public bool Rename(string path, string newPath)
        {
            Guard.NotEmptyOrNull(path, "path");
            Guard.NotEmptyOrNull(newPath, "newPath");

            path = Path.Combine(root, path);
            GuardLimitedRoot(path);

            newPath = Path.Combine(root, newPath);
            GuardLimitedRoot(newPath);

            var rootPath = Path.GetDirectoryName(path);
            var newFileName = Path.GetFileNameWithoutExtension(newPath);
            var isDir = IsDir(path);

            if (!isDir && File.Exists(newPath))
            {
                throw new IOException("duplicate file name:" + newFileName);
            }

            if (isDir && Directory.Exists(newPath))
            {
                throw new IOException("duplicate file name:" + newFileName);
            }

            if (rootPath != Path.GetDirectoryName(newPath))
            {
                throw new IOException("rename can't be used to change a files/dir location use Move(...) instead.");
            }

            if (isDir)
            {
                Directory.Move(path, newPath);
            }
            else
            {
                File.Move(path, newPath);
            }
            
            return true;
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="copyPath">复制到的文件路径</param>
        /// <returns>是否成功</returns>
        public bool Copy(string path, string copyPath)
        {
            path = Path.Combine(root, path);
            copyPath = Path.Combine(root, copyPath);
            var isDir = IsDir(path);
            EnsureDirectory(copyPath);

            if (isDir)
            {
                var fileInfo = new FileInfo(path);
                fileInfo.CopyTo(copyPath);
            }
            return true;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否成功</returns>
        public bool Delete(string path)
        {
            return true;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>是否成功</returns>
        public bool CreateDir(string path)
        {
            Guard.NotEmptyOrNull(path, "path");
            path = Path.Combine(root, path);
            GuardLimitedRoot(path);
            EnsureDirectory(path);
            return true;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件</returns>
        public IFile GetFile(string path)
        {
            return null;
        }

        /// <summary>
        /// 获取文件夹
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件夹</returns>
        public IDirectory GetDir(string path)
        {
            return null;
        }

        /// <summary>
        /// 是否时文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected bool IsDir(string path)
        {
            if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断限定范围是否在root下
        /// </summary>
        /// <param name="path">绝对路径</param>
        protected void GuardLimitedRoot(string path)
        {
            var newPath = Path.GetFullPath(path);
            if (!newPath.Contains(root))
            {
                throw new RuntimeException("The path range is beyond root path " + path);
            }
        }

        /// <summary>
        /// 保证目录存在
        /// </summary>
        /// <param name="root">路径</param>
        protected void EnsureDirectory(string root)
        {
            if (Directory.Exists(root))
            {
                return;
            }

            var info = Directory.CreateDirectory(root);

            if (!info.Exists)
            {
                throw new IOException("Impossible to create thr root directory " + root);
            }
        }
    }
}
