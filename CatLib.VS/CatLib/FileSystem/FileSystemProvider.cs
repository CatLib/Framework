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
using System.Collections;
using CatLib.API;
using CatLib.API.FileSystem;
using CatLib.FileSystem.Adapter;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统服务提供者
    /// </summary>
    public sealed class FileSystemProvider : ServiceProvider
    {
        /// <summary>
        /// 服务提供者进程
        /// </summary>
        /// <returns>迭代器</returns>
        [Priority]
        public override IEnumerator Init()
        {
            InitRegisterLocalDriver();
            return base.Init();
        }

        /// <summary>
        /// 注册文件系统服务
        /// </summary>
        public override void Register()
        {
            RegisterManager();
            RegisterDefaultFileSystem();
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        private void RegisterManager()
        {
            App.Singleton<FileSystemManager>().Alias<IFileSystemManager>().Alias("catlib.filesystem.manager");
        }

        /// <summary>
        /// 注册默认的文件系统
        /// </summary>
        private void RegisterDefaultFileSystem()
        {
            App.Bind<IFileSystem>((container, @params) =>
            {
                return App.Make<IFileSystemManager>().Default;
            });
        }

        /// <summary>
        /// 初始化本地磁盘驱动
        /// </summary>
        private void InitRegisterLocalDriver()
        {
            var storage = App.Make<IFileSystemManager>();
            var env = App.Make<IEnv>();

            if (env != null)
            {
                storage.Extend(() => new FileSystem(new Local(env.AssetPath)));
            }
        }
    }
}
#endif