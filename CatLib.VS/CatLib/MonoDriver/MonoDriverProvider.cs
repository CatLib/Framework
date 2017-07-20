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
using CatLib.API.MonoDriver;

namespace CatLib.MonoDriver
{
    /// <summary>
    /// Mono驱动器服务
    /// </summary>
    public sealed class MonoDriverProvider : ServiceProvider
    {
        /// <summary>
        /// Mono驱动器初始化
        /// </summary>
        /// <returns>迭代器</returns>
        [Priority(1)]
        public override void Init()
        {
            App.Make<IMonoDriver>();
        }

        /// <summary>
        /// 注册路由条目
        /// </summary>
        public override void Register()
        {
            App.Singleton<MonoDriver>().Alias<IMonoDriver>();
        }
    }
}
#endif