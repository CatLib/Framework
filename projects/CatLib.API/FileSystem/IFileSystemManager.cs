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

namespace CatLib.API.FileSystem
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    public interface IFileSystemManager : ISingleManager<IFileSystem>
    {
        /// <summary>
        /// 获取一个文件系统解决方案(磁盘)
        /// </summary>
        /// <param name="name">解决方案名</param>
        /// <returns>文件系统</returns>
        IFileSystem Disk(string name = null);
    }
}
