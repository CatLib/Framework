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

using System.Collections;
using CatLib.API;
using CatLib.API.FileSystem;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统服务提供商
    /// </summary>
    public sealed class FileSystemProvider : ServiceProvider
    {
        /// <summary>
        /// 服务提供商进程
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
            RegisterAdapter();
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        private void RegisterManager()
        {
            App.Singleton<FileSystemManager>().Alias<IFileSystemManager>().Alias("filesystem.manager");
        }

        /// <summary>
        /// 注册适配器
        /// </summary>
        private void RegisterAdapter()
        {
            App.Bind<Local>().Alias("filesystem.adapter.local");
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
