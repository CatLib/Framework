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

            if (!Path.IsPathRooted(root))
            {
                throw new ArgumentException("Path need rooted! " + root, "root");
            }

            this.root = root;
        }

        /// <summary>
        /// 文件或文件夹是否存在
        /// </summary>
        /// <param name="path">文件或文件夹是否存在</param>
        /// <returns>是否存在</returns>
        public bool Exists(string path)
        {
            Guard.NotEmptyOrNull(path, "path");

            path = Normalize(path);
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
        public void Write(string path, byte[] contents)
        {
            Guard.NotEmptyOrNull(path, "path");
            Guard.NotNull(contents, "contents");

            path = Normalize(path);
            GuardLimitedRoot(path);

            EnsureDirectory(Path.GetDirectoryName(path));
            SIO.File.WriteAllBytes(path, contents);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>读取的数据</returns>
        public byte[] Read(string path)
        {
            Guard.NotEmptyOrNull(path, "path");

            path = Normalize(path);
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
        public void Rename(string path, string newPath)
        {
            Guard.NotEmptyOrNull(path, "path");
            Guard.NotEmptyOrNull(newPath, "newPath");

            path = Normalize(path);
            GuardLimitedRoot(path);

            newPath = Normalize(newPath);
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
        }

        /// <summary>
        /// 复制文件或文件夹到指定路径
        /// </summary>
        /// <param name="path">文件或文件夹路径(应该包含文件夹或者文件名)</param>
        /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
        /// <returns>是否成功</returns>
        public void Copy(string path, string copyPath)
        {
            Guard.NotEmptyOrNull(path, "path");
            Guard.NotEmptyOrNull(copyPath, "copyPath");

            path = Normalize(path);
            GuardLimitedRoot(path);

            copyPath = Normalize(copyPath);
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
        }

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否成功</returns>
        public void Delete(string path)
        {
            Guard.NotEmptyOrNull(path, "path");

            path = Normalize(path);
            GuardLimitedRoot(path);

            if (IsDir(path))
            {
                SIO.Directory.Delete(path, true);
            }
            else
            {
                SIO.File.Delete(path);
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>是否成功</returns>
        public void CreateDir(string path)
        {
            Guard.NotEmptyOrNull(path, "path");

            path = Normalize(path);
            GuardLimitedRoot(path);

            EnsureDirectory(path);
        }

        /// <summary>
        /// 获取文件/文件夹属性
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹属性</returns>
        public FileAttributes GetAttributes(string path)
        {
            Guard.NotEmptyOrNull(path, "path");

            path = Normalize(path);
            GuardLimitedRoot(path);

            return SIO.File.GetAttributes(path);
        }

        /// <summary>
        /// 获取列表（不会迭代子文件夹）
        /// </summary>
        /// <param name="path">要获取列表的路径</param>
        /// <returns>指定目录下的文件夹和文件列表</returns>
        public string[] GetList(string path = null)
        {
            path = Normalize(path ?? string.Empty);
            GuardLimitedRoot(path);

            var result = new List<string>();

            if (IsDir(path))
            {
                var files = SIO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    result.Add(file);
                }

                foreach (var info in SIO.Directory.GetDirectories(path))
                {
                    result.Add(info);
                }
            }
            else
            {
                result.Add(path);
            }

            return result.ToArray();
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
        /// 标准化路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string Normalize(string path)
        {
            return Path.GetFullPath(Path.Combine(root, path));
        }

        /// <summary>
        /// 判断限定范围是否在root下
        /// </summary>
        /// <param name="path">绝对路径</param>
        protected void GuardLimitedRoot(string path)
        {
            if (!path.Contains(root))
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

            SIO.Directory.CreateDirectory(root);
        }
    }
}