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

using CatLib.API.FileSystem;
using System;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    internal sealed class FileSystemManager : SingleManager<IFileSystem>, IFileSystemManager
    {
        /// <summary>
        /// 默认名字
        /// </summary>
        private string name = "local";

        /// <summary>
        /// 设定默认驱动名字
        /// </summary>
        /// <param name="name">默认驱动名字</param>
        public void SetDefaultDevice(string name)
        {
            Guard.Requires<ArgumentNullException>(name != null);
            this.name = name;
        }

        /// <summary>
        /// 获取一个文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        public IFileSystem Disk(string name = null)
        {
            return Get(name);
        }

        /// <summary>
        /// 获取默认的文件系统名字
        /// </summary>
        /// <returns>默认的文件系统名字</returns>
        protected override string GetDefaultName()
        {
            return name;
        }
    }
}

