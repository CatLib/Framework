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

namespace CatLib.API.FileSystem
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// 获取一个文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        IFileSystem Disk(string name = null);

        /// <summary>
        /// 获取一个文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        IFileSystem Get(string name = null);

        /// <summary>
        /// 获取一个文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        IFileSystem this[string name] { get; }

        /// <summary>
        /// 自定义文件系统(磁盘)
        /// </summary>
        /// <param name="resolve">文件系统解决方案</param>
        /// <param name="name">名字</param>
        void Extend(Func<IFileSystem> resolve, string name = null);
    }
}
