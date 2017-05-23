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
using CatLib.API;
using CatLib.API.Config;
using CatLib.API.FileSystem;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    public sealed class FileSystemManager : IStorage
    {
        /// <summary>
        /// 自定义解决器
        /// </summary>
        private readonly Dictionary<string, Func<IFileSystem>> customResolve;

        /// <summary>
        /// 文件系统字典
        /// </summary>
        private readonly Dictionary<string, IFileSystem> fileSystems;

        /// <summary>
        /// 配置
        /// </summary>
        private readonly IConfig config;

        /// <summary>
        /// 文件系统管理器
        /// </summary>
        public FileSystemManager(IConfig config)
        {
            customResolve = new Dictionary<string, Func<IFileSystem>>();
            fileSystems = new Dictionary<string, IFileSystem>();
            this.config = config;
        }

        /// <summary>
        /// 获取一个文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        public IFileSystem Disk(string name = null)
        {
            name = name ?? GetDefaultFileSystemName();
            return Get(name);
        }

        /// <summary>
        /// 自定义文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="resolve">文件系统解决方案</param>
        public void Extend(string name, Func<IFileSystem> resolve)
        {
            if (customResolve.ContainsKey(name))
            {
                throw new RuntimeException("Custom resolve [" + name + "] is already exists.");
            }
            customResolve.Add(name, resolve);
        }

        /// <summary>
        /// 获取文件系统
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        private IFileSystem Get(string name)
        {
            IFileSystem fileSystem;
            if (fileSystems.TryGetValue(name, out fileSystem))
            {
                return fileSystem;
            }
            fileSystem = Resolve(name);
            fileSystems.Add(name, fileSystem);
            return fileSystem;
        }

        /// <summary>
        /// 生成所需求的文件系统解决方案
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        private IFileSystem Resolve(string name)
        {
            Func<IFileSystem> fileSystemCustomResolve;
            if (customResolve.TryGetValue(name, out fileSystemCustomResolve))
            {
                return fileSystemCustomResolve.Invoke();
            }

            throw new RuntimeException("Can not find [" + name + "] FileSystem Extend.");
        }

        /// <summary>
        /// 获取默认的文件系统名字
        /// </summary>
        /// <returns>默认的文件系统名字</returns>
        private string GetDefaultFileSystemName()
        {
            return config == null ? "local" : config.Get("filesystems.default", "local");
        }
    }
}
