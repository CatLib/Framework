/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *File.
 * Document: http://catlib.io/
 */

using System.IO;
using CatLib.API;
using CatLib.Stl;
using SIO = System.IO;

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
            return SIO.File.Exists(path) || SIO.Directory.Exists(path);
        }

        /// <summary>
        /// 写入数据
        /// 如果数据已经存在则覆盖
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
            SIO.File.WriteAllBytes(path, contents);
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
            if (SIO.File.Exists(path))
            {
                return SIO.File.ReadAllBytes(path);
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

            if (SIO.File.Exists(newPath))
            {
                throw new IOException("duplicate name:" + newFileName);
            }

            if (SIO.Directory.Exists(newPath))
            {
                throw new IOException("duplicate name:" + newFileName);
            }

            if (rootPath != Path.GetDirectoryName(newPath))
            {
                throw new IOException("rename can't be used to change a files/dir location use Move(...) instead.");
            }

            if (isDir)
            {
                SIO.Directory.Move(path, newPath);
            }
            else
            {
                SIO.File.Move(path, newPath);
            }

            return true;
        }

        /// <summary>
        /// 复制文件或文件夹到指定路径
        /// </summary>
        /// <param name="path">文件或文件夹路径(应该包含文件夹或者文件名)</param>
        /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
        /// <returns>是否成功</returns>
        public bool Copy(string path, string copyPath)
        {
            Guard.NotEmptyOrNull(path, "path");
            Guard.NotEmptyOrNull(copyPath, "copyPath");

            path = Path.Combine(root, path);
            GuardLimitedRoot(path);

            copyPath = Path.Combine(root, copyPath);
            GuardLimitedRoot(copyPath);

            EnsureDirectory(copyPath);

            if (IsDir(path))
            {
                var files = SIO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    SIO.File.Copy(file, Path.Combine(copyPath, fileName), true);
                }

                foreach (var info in SIO.Directory.GetDirectories(path))
                {
                    Copy(info, Path.Combine(copyPath, Path.GetFileName(info)));
                }
            }
            else
            {
                var fileName = Path.GetFileName(path);
                SIO.File.Copy(path, Path.Combine(copyPath, fileName), true);
            }

            return true;
        }

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否成功</returns>
        public bool Delete(string path)
        {
            Guard.NotEmptyOrNull(path, "path");

            path = Path.Combine(root, path);
            GuardLimitedRoot(path);

            if (IsDir(path))
            {
                SIO.Directory.Delete(path, true);
            }
            else
            {
                SIO.File.Delete(path);
            }
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
        /// 获取文件/文件夹属性
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹属性</returns>
        public FileAttributes GetAttributes(string path)
        {
            Guard.NotEmptyOrNull(path, "path");
            path = Path.Combine(root, path);
            return SIO.File.GetAttributes(path);
        }

        /// <summary>
        /// 是否是文件夹
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>是否是文件夹</returns>
        protected bool IsDir(string path)
        {
            return (SIO.File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
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
            if (SIO.Directory.Exists(root))
            {
                return;
            }

            var info = SIO.Directory.CreateDirectory(root);

            if (!info.Exists)
            {
                throw new IOException("Impossible to create thr root directory " + root);
            }
        }
    }
}