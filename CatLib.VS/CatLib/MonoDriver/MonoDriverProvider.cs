﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib.API;
using CatLib.API.MonoDriver;

#if CATLIB


namespace CatLib.Routing
{
    /// <summary>
    /// Mono驱动器服务
    /// </summary>
    public sealed class MonoDriverProvider : ServiceProvider
    {
        /// <summary>
        /// 执行路由编译，路由编译总是最后进行的
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
            App.Singleton<MonoDriver.MonoDriver>().Alias<IMonoDriver>();
        }
    }
}
#endif