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

using CatLib.API.Routing;
using CatLib.API.FilterChain;

namespace CatLib.Routing
{

    public class RoutingProvider : ServiceProvider
    {

        public override void Register()
        {

            RegisterRouter();

        }

        protected void RegisterRouter()
        {

            App.Singleton<Router>((app , param)=>
            {

                var router = new Router(App , app , app.Make<IFilterChain>());
                router.SetDefaultScheme("catlib");
                return router;

            }).Alias<IRouter>();

        }
    }

}