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

using CatLib.API.Config;
using CatLib.API.FileSystem;
using CatLib.Stl;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    public sealed class FileSystemManager : Manager<IFileSystem>, IFileSystemManager
    {
        /// <summary>
        /// 配置
        /// </summary>
        private readonly IConfigManager configManager;

        /// <summary>
        /// 文件系统管理器
        /// </summary>
        public FileSystemManager(IConfigManager configManager)
        {
            this.configManager = configManager;
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
            return configManager == null ? "local" : configManager.Default.Get("filesystems.default", "local");
        }
    }
}
