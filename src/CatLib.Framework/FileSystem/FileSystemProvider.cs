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

#if CATLIB
using CatLib.API.FileSystem;
using CatLib.FileSystem.Adapter;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统服务提供者
    /// </summary>
    public sealed class FileSystemProvider : IServiceProvider
    {
        /// <summary>
        /// 默认驱动名字
        /// </summary>
        public string DefaultDevice { get; set; }

        /// <summary>
        /// 默认路径
        /// </summary>
        public string DefaultPath { get; set; }

        /// <summary>
        /// 文件系统服务提供者
        /// </summary>
        public FileSystemProvider()
        {
            DefaultDevice = "local";
        }

        /// <summary>
        /// 服务提供者进程
        /// </summary>
        /// <returns>迭代器</returns>
        [Priority(10)]
        public void Init()
        {
            InitRegisterLocalDriver();
        }

        /// <summary>
        /// 注册文件系统服务
        /// </summary>
        public void Register()
        {
            RegisterManager();
            RegisterDefaultFileSystem();
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        private void RegisterManager()
        {
            App.Singleton<FileSystemManager>().Alias<IFileSystemManager>().OnResolving((_, obj) =>
            {
                var manager = (FileSystemManager) obj;
                manager.SetDefaultDevice(DefaultDevice);
                return obj;
            });
        }

        /// <summary>
        /// 注册默认的文件系统
        /// </summary>
        private void RegisterDefaultFileSystem()
        {
            App.Bind<IFileSystem>((container, _) => container.Make<IFileSystemManager>().Default);
        }

        /// <summary>
        /// 初始化本地磁盘驱动
        /// </summary>
        private void InitRegisterLocalDriver()
        {
            if (!string.IsNullOrEmpty(DefaultPath))
            {
                App.Make<IFileSystemManager>().Extend(() => new FileSystem(new Local(DefaultPath)));
            }
        }
    }
}
#endif