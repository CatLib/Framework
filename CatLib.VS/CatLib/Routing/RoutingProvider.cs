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
using CatLib.API.Config;
using CatLib.API.Routing;
using System.Collections.Generic;
using System.Threading;
using CatLib.API.MonoDriver;

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
        public override void Init()
        {
            InitMainThreadGroup();
            var router = App.Make<Router>();
            router.RouterCompiler();
        }

        /// <summary>
        /// 注册路由条目
        /// </summary>
        public override void Register()
        {
            App.Singleton<Router>((app, param) =>
            {
                var router = new Router(App, App);
                router.SetDefaultScheme("catlib");
                return router;
            }).Alias<IRouter>().Alias("catlib.routing.router");

            RegisterAttrRouteCompiler();
        }

        /// <summary>
        /// 初始化主线程组
        /// </summary>
        private void InitMainThreadGroup()
        {
            var router = App.Make<IRouter>();
            var driver = App.Make<IMonoDriver>();

            if (driver != null)
            {
                router.Group("MainThreadCall").Middleware((request, response, next) =>
                {
                    var wait = new AutoResetEvent(false);
                    driver.MainThread(() =>
                    {
                        try
                        {
                            next(request, response);
                        }
                        finally
                        {
                            wait.Set();
                        }
                    });
                    wait.WaitOne();
                });
            }
        }

        /// <summary>
        /// 注册属性路由编译器
        /// </summary>
        private void RegisterAttrRouteCompiler()
        {
            App.Bind<AttrRouteCompiler>().OnResolving((bind, obj) =>
            {
                var compiler = obj as AttrRouteCompiler;

                var containList = new List<string>()
                {
                    "Assembly-CSharp", "Assembly-CSharp-Editor-firstpass", "Assembly-CSharp-Editor", "CatLib", "CatLib.Tests"
                };

                var config = App.Make<IConfigManager>();
                if (config != null)
                {
                    var reserved = config.Default.Get("routing.stripping.reserved", string.Empty);
                    containList.AddRange(reserved.Split(';'));
                }

                compiler.OnStripping((assembly) =>
                {
                    return !containList.Contains(assembly.GetName().Name);
                });

                return obj;
            });
        }
    }
}
#endif