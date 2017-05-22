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
    /// 存储
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// 获取一个文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>文件系统</returns>
        IFileSystem Disk(string name = null);

        /// <summary>
        /// 自定义文件系统(磁盘)
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="resolve">文件系统解决方案</param>
        void Extend(string name, Func<IFileSystem> resolve);
    }
}
