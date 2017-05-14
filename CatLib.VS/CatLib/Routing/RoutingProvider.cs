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
using CatLib.API.Routing;
using CatLib.API.FilterChain;
using System.Collections;
using CatLib.API;

namespace CatLib.Routing
{
    /// <summary>
    /// 路由服务
    /// </summary>
    public sealed class RoutingProvider : ServiceProvider
    {
        /// <summary>
        /// 执行路由编译，路由编译总是最后进行的
        /// </summary>
        /// <returns>迭代器</returns>
        [Priority]
        public override IEnumerator OnProviderProcess()
        {
            return App.Make<Router>().RouterCompiler();
        }

        /// <summary>
        /// 注册路由条目
        /// </summary>
        public override void Register()
        {
            App.Singleton<Router>((app, param) =>
            {
                var router = new Router(App, app, app.Make<IFilterChain>());
                router.SetDefaultScheme("catlib");
                return router;

            }).Alias<IRouter>();
        }
    }
}