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

using System.Runtime.InteropServices;
using CatLib.API;
using CatLib.API.Routing;
using UnityEngine;

namespace CatLib.Demo.Routing
{

    public class RoutingDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.OnApplicationStartComplete, (sender, e) =>
            {
                IRouter router = App.Make<IRouter>();

                router.Dispatch("attr-routing-simple/call");
                router.Dispatch("attr-routing-simple/call-mtest");

                router.Dispatch("catlib://hello-world/call");

                router.Dispatch("attr-routing/class-static#10");
                Debug.Log(router.Dispatch("attr-routing/return-response/hello my name is anny").GetContext().ToString());
                Debug.Log(router.Dispatch("catlib://attr-routing/options-param").GetContext().ToString());
                router.Dispatch("attr-routing/other-param", "hello this is from context");

                //手动指向类的路由
                router.Reg("attr-routing/hand", typeof(AttrRouting), "RegRoute");
                router.Dispatch("attr-routing/hand");

                //基于回掉的路由
                router.Reg("callback-routing", (request, response) =>
               {
                   Debug.Log("this is call back routing");
               });
                router.Dispatch("catlib://callback-routing");

                //基于组的路由
                router.Group(() =>
                {
                    router.Reg("group-callback-routing/with-group-1/{param?}", (request, response) =>
                    {
                        Debug.Log("callback-routing/with-group-1 , " + request.Get("param"));
                    });

                    router.Reg("group-callback-routing/with-group-2/{param?}", (request, response) =>
                    {
                        Debug.Log("callback-routing/with-group-2 , " + request.Get("param"));
                    }).Defaults("param", "this is from route default param");

                    router.Reg("group-callback-routing/with-group-3/{param?}", (request, response) =>
                    {
                        Debug.Log("callback-routing/with-group-3 , " + request.Get("param"));
                    });

                    router.Reg("group-callback-routing/error", (request, response) =>
                    {
                        throw new System.Exception("this throw exception");
                    }).OnError((req, res , ex, next) =>
                    {
                        Debug.Log("on error , this is route");
                    //next.Do(req, ex); 
                    // 由于路由条目和路由组是同级关系，根据规则，将会出现2个onError的过滤器链，这里没有继续next 所以 组中定义的onerror将不会执行到
                    // 异常是冒泡执行的，所以一旦被临近filter chain拦截那么全局异常也不会触发。
                });

                }).Defaults("param", "this is group default param").OnError((request,res , ex, next) =>
                {
                    Debug.Log("on error , this is group");
                    next(request, res, ex);
                });

                router.OnError((request, res, ex, next) =>
                {
                    Debug.Log("on error , this is router");
                    next(request , res, ex);
                });

                router.Dispatch("catlib://group-callback-routing/with-group-1");
                router.Dispatch("group-callback-routing/with-group-2"); // 以上2中写法如果对于默认sechem 是等价的
                router.Dispatch("catlib://group-callback-routing/with-group-3");
                router.Dispatch("catlib://group-callback-routing/error");

                //命名路由组,明明路由组后在路由中可以通过名字给定路由组
                IRouteGroup group1 = router.Group("group-name-1").Defaults("param", "from name group");
                IRouteGroup group2 = router.Group(() => { /* do something */ }, "group-name-2");

                router.Reg("group-callback-routing/with-name-group-1/{param?}", (request, response) =>
                {
                    Debug.Log("callback-routing/with-name-group-1 , " + request.Get("param"));
                }).Group("group-name-1");

                router.Dispatch("catlib://group-callback-routing/with-name-group-1");

            });
        }

        public override void Register()
        {

        }
    }

}