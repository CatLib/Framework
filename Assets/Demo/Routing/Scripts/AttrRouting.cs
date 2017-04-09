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

using CatLib.API;
using CatLib.API.FilterChain;
using CatLib.API.Routing;
using UnityEngine;

namespace CatLib.Demo.Routing
{

    /**
     * 这个Demo演示了属性路由
     */

    // 路由的最简使用方式
    // 如果类或者方法没有给定名字那么会自动使用类名或者方法名作为路由路径
    // 如下面的路由会被默认给定名字：attr-routing-simple/call 
    [Routed]
    public class AttrRoutingSimple
    {
        [Routed]
        public void Call(IRequest request, IResponse response)
        {
            Debug.Log("this is simple routed");
        }
    }

    // CatLib 路由系统允许添加多个路由名
    [Routed] // catlib://mult-attr-routing-simple
    [Routed("hello-world")] // catlib://hello-world
    [Routed("cat://mult-arrt-routing-simple")] // cat://mult-attr-routing-simple
    public class MultAttrRoutingSimple
    {
        // catlib://mult-attr-routing-simple/call
        // catlib://hello-world/call
        // cat://mult-attr-routing-simple/call
        [Routed]
        // catlib://mult-attr-routing-simple/my-hello
        // catlib://hello-world/my-hello
        // cat://mult-attr-routing-simple/my-hello
        [Routed("my-hello")]
        // dog://myname/call
        [Routed("dog://myname/call")]
        public void Call(IRequest request, IResponse response)
        {
            Debug.Log("this is mult simple routed");
        }
    }

    // 您可以为整个类制定统一的路由规则，他将会对类中的所有路由生效。这是一个可选的操作。
    // 来自函数路由条目的配置优先级总是高于来自全局的配置
    // 类必须被标记为Routed才能够进行路由
    [Routed("catlib://", Defaults = "desc=>desc from class,age=>18")]
    // 如果实现了 IMiddleware 接口那么所有指向位这个类的路由将会通过指定的中间件，中间件可以拦截，修改请求。 
    public class AttrRouting : IMiddleware
    {

        /// <summary>
        /// 中间件
        /// </summary>
        public IFilterChain<IRequest, IResponse> Middleware
        {
            get
            {
                //这里的代码只用作演示用途，现实代码请不要像这样生成中间件
                var filterChain = App.Instance.Make<IFilterChain>();
                var filter = filterChain.Create<IRequest, IResponse>();
                filter.Add((req, res, next) =>
                {
                    Debug.Log("through controller middleware in");
                    next.Do(req, res);
                    Debug.Log("through controller middleware out");
                });
                return filter;
            }
        }

        // 这是一个静态路由，没有通过uri接受任何参数
        // 路由接受函数的前2个参数一定是 IRequest 和 IResponse ，如果是同步路由您可以选择性的接受她们，如果是异步路由您必须严格填写顺序。
        [Routed("attr-routing/class-static")]
        public void ClassStatic(IRequest request , IResponse response)
        {
            Debug.Log("in class [" + typeof(AttrRouting).ToString() + "] , call function: ClassStatic() , fragment:" + request.Uri.Fragment);
        }

        //您也可以在路由方法中强制指定scheme，这样她会忽略来自class中定义的scheme。
        //这里我们定义了3个需要接受的参数，其中str表示位必填参数，如果您没有填写，那么路由将不能匹配。而desc和age是可选参数，如果您没有填写他会使用默认值。
        //与此同时我们还为age定义了正则约束，如果您填写了age这个参数那么他必须受到正则约束才能匹配
        [Routed("catlib://attr-routing/return-response/{str}/{desc?}/{age?}" , 
                    Defaults = "desc=>this is default desc" , 
                    Where = "age=>[0-9]+")]
        public void ReturnResponse(IRequest request, IResponse response)
        {
            Debug.Log("in class [" + typeof(AttrRouting).ToString() + "] , call function: ReturnResponse() , get params: " + request.Get("str") + " , " + request.Get("desc") + " , " + request.Get("age"));
            // request["str"] 等价于 request.Get("str")
            response.SetContext("this is callback response :" + request["str"]);
            // 您不需要给定返回值，因为 response 是引用的
        }

        // 如果您是一个同步路由那么你可以选填接受参数。静态函数和实例函数都可以接受路由。
        // 无论是否是静态的都会经过类中间件。
        [Routed("attr-routing/options-param")]
        public static void OptionsParam(IResponse response)
        {
            Debug.Log("in class [" + typeof(AttrRouting).ToString() + "] , call function: OptionsParam()");
            response.SetContext("this is callback from : OptionsParam()");
        }

        // 除了允许接受请求参数外您还可以接受其他在容器中注册过的元素
        // 外部也允许传入 context 
        [Routed("attr-routing/other-param")]
        public static void OtherParam(IRequest request, IApplication application)
        {
            Debug.Log("in class [" + typeof(AttrRouting).ToString() + "] , call function: OtherParam() , param : " + application.GetType().ToString());
            Debug.Log("this is from call in context : " + request.GetContext().ToString());
        }

        // 手动注册路由，这个函数没有Routed属性标记所以不会自动加入路由条目，需要手动注册
        public void RegRoute()
        {
            Debug.Log("this is reg route");
        }
    }

}