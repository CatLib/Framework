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
using System.Collections.Generic;
using CatLib.API.Event;
using CatLib.API.Container;
using CatLib.API.Routing;
using CatLib.API.FilterChain;
using System.Collections;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由服务
    /// </summary>
    public class Router : IRouter
    {

        /// <summary>
        /// 分隔符
        /// </summary>
        public const char SEPARATOR = '/';

        /// <summary>
        /// 全局调度器
        /// </summary>
        protected IEvent events;

        /// <summary>
        /// 容器
        /// </summary>
        protected IContainer container;

        /// <summary>
        /// 过滤器链生成器
        /// </summary>
        protected IFilterChain filterChain;

        /// <summary>
        /// 协议方案
        /// </summary>
        protected Dictionary<string, Scheme> schemes;

        /// <summary>
        /// 当路由没有找到时过滤链
        /// </summary>
        protected IFilterChain<IRequest> onNotFound;

        /// <summary>
        /// 当出现异常时的过滤器链
        /// </summary>
        protected IFilterChain<IRequest , Exception> onError;

        /// <summary>
        /// 路由组
        /// </summary>
        protected Dictionary<string , RouteGroup> routeGroup;

        /// <summary>
        /// 路由组堆栈
        /// </summary>
        protected Stack<IRouteGroup> routeGroupStack;

        /// <summary>
        /// 默认的scheme
        /// </summary>
        protected string defaultScheme;

        /// <summary>
        /// 创建一个新的路由器
        /// </summary>
        /// <param name="events"></param>
        /// <param name="container"></param>
        /// <param name="filterChain"></param>
        public Router(IEvent events , IContainer container , IFilterChain filterChain)
        {
            this.events = events;
            this.container = container;
            this.filterChain = filterChain;
            schemes = new Dictionary<string, Scheme>();
            routeGroupStack = new Stack<IRouteGroup>();
        }

        /// <summary>
        /// 设定默认的scheme
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public IRouter SetDefaultScheme(string scheme)
        {
            defaultScheme = scheme;
            return this;
        }

        /// <summary>
        /// 根据回调行为注册一个路由
        /// </summary>
        /// <param name="uris">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public IRoute Reg(string uris , Action<IRequest, IResponse> action)
        {
            return RegisterRoute(uris , new RouteAction()
            {
                Type = RouteAction.RouteTypes.CallBack,
                Action = action,
            });
        }

        /// <summary>
        /// 根据控制器的type和调用的方法名字注册一个路由
        /// </summary>
        /// <param name="uris"></param>
        /// <param name="controller"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public IRoute Reg(string uris , Type controller , string func)
        {
            return RegisterRoute(uris, new RouteAction()
            {
                Type = RouteAction.RouteTypes.ControllerCall,
                Controller = controller.ToString(),
                Func = func
            });
        }

        /// <summary>
        /// 当路由没有找到时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRouter OnNotFound(Action<IRequest , IFilterChain<IRequest>> middleware)
        {
            if(onNotFound == null)
            {
                onNotFound = filterChain.Create<IRequest>();
            }
            onNotFound.Add(middleware);
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRouter OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> middleware)
        {
            if (onError == null)
            {
                onError = filterChain.Create<IRequest , Exception>();
            }
            onError.Add(middleware);
            return this;
        }

        /// <summary>
        /// 调度路由
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public IResponse Dispatch(string uri, object context = null)
        {

            uri = GuardUri(uri);
            uri = Prefix(uri);

            Request request = CreateRequest(uri, context);

            if (!schemes.ContainsKey(request.Scheme))
            {
                ThrowOnNotFound(request);
                return null;
            }

            try
            {

                Route route = FindRoute(request);
                if (route == null) { return null; }

                request.SetRoute(route);

                //todo: dispatch event
         
                return RunRouteWithMiddleware(route, request);

            }catch(NotFoundRouteException)
            {
                ThrowOnNotFound(request);
                return null;
            }
            catch(Exception ex)
            {
                ThrowOnError(request, ex);
                return null;
            }

        }

        /// <summary>
        /// 建立或者获取一个已经建立的路由组
        /// </summary>
        public IRouteGroup Group(string name){

            if(routeGroup == null){ routeGroup = new Dictionary<string , RouteGroup>(); }
            if (name == null) { return (new RouteGroup()).SetFilterChain(filterChain); }
            if(!routeGroup.ContainsKey(name)){

                routeGroup.Add(name , new RouteGroup().SetFilterChain(filterChain));

            }

            return routeGroup[name];

        }

        /// <summary>
        /// 建立匿名路由组，调用的闭包内为路由组有效范围, 允许给定一个名字来显示命名路由组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public IRouteGroup Group(Action area , string name = null)
        {
            IRouteGroup group = Group(name);
    
            routeGroupStack.Push(group);
            area.Invoke();
            routeGroupStack.Pop();

            return group;
        }

        /// <summary>
        /// 路由器编译
        /// </summary>
        /// <returns></returns>
        public IEnumerator RouterCompiler()
        {
            (new AttrRouteCompiler(this , container)).Complie();
            yield break;
        }

        /// <summary>
        /// 注册一个路由方案
        /// </summary>
        /// <param name="uris">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        protected IRoute RegisterRoute(string uris, RouteAction action)
        {

            uris = GuardUri(uris);
            Uri uri = new Uri(uris);

            if (!schemes.ContainsKey(uri.Scheme))
            {
                CreateScheme(uri.Scheme);
            }

            var route = MakeRoute(uri, action);

            schemes[uri.Scheme].AddRoute(route);

            if (routeGroupStack.Count > 0)
            {
                routeGroupStack.Peek().AddRoute(route);
            }

            return route;

        }

        /// <summary>
        /// 产生一个路由条目
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="action">路由行为</param>
        /// <returns></returns>
        protected Route MakeRoute(Uri uri, RouteAction action)
        {
            var route = new Route(uri, action);
            route.SetRouter(this);
            route.SetScheme(schemes[uri.Scheme]);
            route.SetFilterChain(filterChain);
            route.SetContainer(container);
            return route;
        }

        /// <summary>
        /// 触发没有找到路由的过滤器链
        /// </summary>
        /// <param name="request"></param>
        protected void ThrowOnNotFound(Request request)
        {
            if(onNotFound != null)
            {
                onNotFound.Do(request);
            }
        }

        /// <summary>
        /// 触发异常
        /// </summary>
        /// <param name="request"></param>
        protected void ThrowOnError(Request request , Exception ex)
        {
            if (onError != null)
            {
                onError.Do(request , ex);
            }
        }

        /// <summary>
        /// 通过中间件后执行路由请求
        /// </summary>
        /// <param name="route"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected IResponse RunRouteWithMiddleware(Route route, Request request)
        {
            try
            {
                var response = new Response();
                var middleware = route.GatherMiddleware();
                if (middleware != null)
                {
                    middleware.Then((req, res) =>
                    {
                        PrepareResponse(req, route.Run(req as Request, res as Response));
                    }).Do(request, response);
                }
                else
                {
                    PrepareResponse(request, route.Run(request, response));
                }
                return response;

            }catch(Exception ex)
            {
                var chain = route.GatherOnError();
                if (chain != null)
                {
                    chain.Then((req, error) =>
                    {
                        ThrowOnError(request, ex);
                    }).Do(request, ex);
                } else
                {
                    ThrowOnError(request, ex);
                }
                throw ex;
            }
        }

        /// <summary>
        /// 准备响应的内容
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <returns></returns>
        protected void PrepareResponse(IRequest request, IResponse response)
        {
            //todo: 预留函数  
        }

        /// <summary>
        /// 增加一个处理方案
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        protected IRouter CreateScheme(string name)
        {
            schemes.Add(name.ToLower(), new Scheme(name));
            return this;
        }

        /// <summary>
        /// 查找一个合适的路由
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected Route FindRoute(Request request)
        {
            Route route = schemes[request.Scheme].Match(request);
            container.Instance("route.current", route);
            return route;
        }

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected Request CreateRequest(string uri , object context)
        {
            return new Request(uri, context);
        }

        /// <summary>
        /// 处理uri为符合规则的url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string Prefix(string url)
        {
            return (GetLastGroupPrefix().Trim(SEPARATOR) + SEPARATOR + url.Trim(SEPARATOR)).Trim(SEPARATOR) ?? SEPARATOR.ToString();
        }

        /// <summary>
        /// 获取最后的分组信息
        /// </summary>
        /// <returns></returns>
        protected string GetLastGroupPrefix()
        {
            return string.Empty;
        }

        /// <summary>
        /// uri 保护
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected string GuardUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new RouterConfigException("reg uri is null or empty");
            }

            if (uri.IndexOf(@"://") < 0)
            {
                if (string.IsNullOrEmpty(defaultScheme))
                {
                    throw new UndefinedDefaultSchemeException("undefined default scheme please call SetDefaultScheme(string)");
                }
                uri = defaultScheme + "://" + uri;
            }
            return uri;
        }

    }

}
