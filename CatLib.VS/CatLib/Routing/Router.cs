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
    internal sealed class Router : IRouter
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        public const char SEPARATOR = '/';

        /// <summary>
        /// 全局调度器
        /// </summary>
        private IEvent events;

        /// <summary>
        /// 容器
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// 过滤器链生成器
        /// </summary>
        private readonly IFilterChain filterChain;

        /// <summary>
        /// 协议方案
        /// </summary>
        private readonly Dictionary<string, Scheme> schemes;

        /// <summary>
        /// 当路由没有找到时过滤链
        /// </summary>
        private IFilterChain<IRequest> onNotFound;

        /// <summary>
        /// 路由请求中间件
        /// </summary>
        private IFilterChain<IRequest, IResponse> middleware;

        /// <summary>
        /// 当出现异常时的过滤器链
        /// </summary>
        private IFilterChain<IRequest, IResponse, Exception> onError;

        /// <summary>
        /// 路由组
        /// </summary>
        private Dictionary<string, RouteGroup> routeGroup;

        /// <summary>
        /// 路由组堆栈
        /// </summary>
        private readonly Stack<IRouteGroup> routeGroupStack;

        /// <summary>
        /// 请求堆栈
        /// </summary>
        private readonly Stack<IRequest> requestStack;

        /// <summary>
        /// 响应堆栈
        /// </summary>
        private readonly Stack<IResponse> responseStack;

        /// <summary>
        /// 默认的scheme
        /// </summary>
        private string defaultScheme;

        /// <summary>
        /// 创建一个新的路由器
        /// </summary>
        /// <param name="events">事件</param>
        /// <param name="container">容器</param>
        /// <param name="filterChain">过滤器链</param>
        public Router(IEvent events, IContainer container, IFilterChain filterChain)
        {
            this.events = events;
            this.container = container;
            this.filterChain = filterChain;
            schemes = new Dictionary<string, Scheme>();
            routeGroupStack = new Stack<IRouteGroup>();
            requestStack = new Stack<IRequest>();
            responseStack = new Stack<IResponse>();
        }

        /// <summary>
        /// 设定默认的scheme
        /// </summary>
        /// <param name="scheme">默认的scheme</param>
        /// <returns>当前实例</returns>
        public IRouter SetDefaultScheme(string scheme)
        {
            defaultScheme = scheme;
            return this;
        }

        /// <summary>
        /// 获取默认的scheme
        /// </summary>
        /// <returns>默认scheme</returns>
        public string GetDefaultScheme()
        {
            return defaultScheme;
        }

        /// <summary>
        /// 根据回调行为注册一个路由
        /// </summary>
        /// <param name="uris">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns>当前实例</returns>
        public IRoute Reg(string uris, Action<IRequest, IResponse> action)
        {
            return RegisterRoute(uris, new RouteAction()
            {
                Type = RouteAction.RouteTypes.CallBack,
                Action = action,
            });
        }

        /// <summary>
        /// 根据控制器的type和调用的方法名字注册一个路由
        /// </summary>
        /// <param name="uris">uri</param>
        /// <param name="controller">控制器类型</param>
        /// <param name="func">调用的方法名</param>
        /// <returns>当前实例</returns>
        public IRoute Reg(string uris, Type controller, string func)
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
        /// <param name="middleware">中间件</param>
        /// <returns>当前实例</returns>
        public IRouter OnNotFound(Action<IRequest, Action<IRequest>> middleware)
        {
            if (onNotFound == null)
            {
                onNotFound = filterChain.Create<IRequest>();
            }
            onNotFound.Add(middleware);
            return this;
        }

        /// <summary>
        /// 全局路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns>当前路由器实例</returns>
        public IRouter Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware)
        {
            if (this.middleware == null)
            {
                this.middleware = filterChain.Create<IRequest, IResponse>();
            }
            this.middleware.Add(middleware);
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="onError">错误处理函数</param>
        /// <returns>当前实例</returns>
        public IRouter OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> onError)
        {
            if (this.onError == null)
            {
                this.onError = filterChain.Create<IRequest, IResponse, Exception>();
            }
            this.onError.Add(onError);
            return this;
        }

        /// <summary>
        /// 调度路由
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="context">上下文</param>
        /// <returns>请求响应</returns>
        public IResponse Dispatch(string uri, object context = null)
        {
            uri = GuardUri(uri);
            uri = Prefix(uri);

            var request = CreateRequest(uri, context);

            if (!schemes.ContainsKey(request.RouteUri.Scheme))
            {
                ThrowOnNotFound(request);
                return null;
            }
            try
            {
                var route = FindRoute(request);
                try
                {
                    container.Instance(typeof(IRequest).ToString(), route);
                    requestStack.Push(request);

                    request.SetRoute(route);

                    //todo: dispatch event

                    return RunRouteWithMiddleware(route, request);
                }
                finally
                {
                    requestStack.Pop();
                    container.Instance(typeof(IRequest).ToString(), requestStack.Count > 0 ? requestStack.Peek() : null);
                }
            }
            catch(NotFoundRouteException)
            {
                ThrowOnNotFound(request);
                return null;
            }
        }

        /// <summary>
        /// 建立或者获取一个已经建立的路由组
        /// </summary>
        /// <param name="name">路由组名字</param>
        /// <returns>当前实例</returns>
        public IRouteGroup Group(string name)
        {
            if (routeGroup == null)
            {
                routeGroup = new Dictionary<string, RouteGroup>();
            }
            if (name == null)
            {
                return (new RouteGroup()).SetFilterChain(filterChain);
            }
            if (!routeGroup.ContainsKey(name))
            {
                routeGroup.Add(name, new RouteGroup().SetFilterChain(filterChain));
            }

            return routeGroup[name];
        }

        /// <summary>
        /// 建立匿名路由组，调用的闭包内为路由组有效范围, 允许给定一个名字来显示命名路由组
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="name">路由组名字</param>
        /// <returns>当前实例</returns>
        public IRouteGroup Group(Action area, string name = null)
        {
            var group = Group(name);

            routeGroupStack.Push(group);
            area.Invoke();
            routeGroupStack.Pop();

            return group;
        }

        /// <summary>
        /// 路由器编译
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator RouterCompiler()
        {
            (new AttrRouteCompiler(this)).Complie();
            yield break;
        }

        /// <summary>
        /// 注册一个路由方案
        /// </summary>
        /// <param name="uris">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns>当前实例</returns>
        private IRoute RegisterRoute(string uris, RouteAction action)
        {
            uris = GuardUri(uris);
            var uri = new Uri(uris);

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
        /// <returns>路由条目</returns>
        private Route MakeRoute(Uri uri, RouteAction action)
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
        /// <param name="request">请求</param>
        private void ThrowOnNotFound(IRequest request)
        {
            if (onNotFound != null)
            {
                onNotFound.Do(request);
            }
        }

        /// <summary>
        /// 触发异常
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="ex">异常</param>
        private void ThrowOnError(IRequest request, IResponse response, Exception ex)
        {
            if (onError != null)
            {
                onError.Do(request, response, ex);
            }
        }

        /// <summary>
        /// 通过中间件后执行路由请求
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        /// <returns>响应</returns>
        private IResponse RunRouteWithMiddleware(Route route, Request request)
        {
            var response = new Response();

            try
            {
                container.Instance(typeof(IResponse).ToString(), response);
                responseStack.Push(response);

                if (middleware != null)
                {
                    middleware.Do(request , response , (req, res) =>
                    {
                        RunInRoute(route, request, response);
                    });
                }
                else
                {
                    RunInRoute(route, request, response);
                }
                return response;
            }
            catch (Exception ex)
            {
                var chain = route.GatherOnError();
                if (chain != null)
                {
                    chain.Do(request, response, ex, (req, res, error) =>
                     {
                         ThrowOnError(request, response, ex);
                     });
                }
                else
                {
                    ThrowOnError(request, response, ex);
                }
                return null;
            }
            finally
            {
                responseStack.Pop();
                container.Instance(typeof(IResponse).ToString(), responseStack.Count > 0 ? responseStack.Peek() : null);
            }
        }

        /// <summary>
        /// 执行路由请求
        /// </summary>
        /// <param name="route">路由</param>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <returns>响应</returns>
        private IResponse RunInRoute(Route route , Request request , Response response)
        {
            var middleware = route.GatherMiddleware();
            if (middleware != null)
            {
                middleware.Do(request, response, (req, res) =>
                {
                    PrepareResponse(req, route.Run(req as Request, res as Response));
                });
            }
            else
            {
                PrepareResponse(request, route.Run(request, response));
            }
            return response;
        }

        /// <summary>
        /// 准备响应的内容
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        private void PrepareResponse(IRequest request, IResponse response)
        {
            //todo: 预留函数  
        }

        /// <summary>
        /// 增加一个处理方案
        /// </summary>
        /// <param name="name">scheme名字</param>
        /// <returns>当前路由实例</returns>
        private IRouter CreateScheme(string name)
        {
            schemes.Add(name.ToLower(), new Scheme(name));
            return this;
        }

        /// <summary>
        /// 查找一个合适的路由
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>命中的路由</returns>
        private Route FindRoute(Request request)
        {
            var route = schemes[request.RouteUri.Scheme].Match(request);
            return route;
        }

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="context">上下文</param>
        /// <returns>请求</returns>
        private Request CreateRequest(string uri, object context)
        {
            return new Request(uri, context);
        }

        /// <summary>
        /// 处理uri为符合规则的url
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>处理后的url</returns>
        private string Prefix(string url)
        {
            return (GetLastGroupPrefix().Trim(SEPARATOR) + SEPARATOR + url.Trim(SEPARATOR)).Trim(SEPARATOR) ?? SEPARATOR.ToString();
        }

        /// <summary>
        /// 获取最后的分组信息
        /// </summary>
        /// <returns></returns>
        private string GetLastGroupPrefix()
        {
            return string.Empty;
        }

        /// <summary>
        /// uri 保护
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>处理后的uri</returns>
        private string GuardUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new RouterConfigException("reg uri is null or empty");
            }

            if (uri.IndexOf(@"://") >= 0)
            {
                return uri;
            }
            if (string.IsNullOrEmpty(defaultScheme))
            {
                throw new UndefinedDefaultSchemeException("undefined default scheme please call SetDefaultScheme(string)");
            }
            uri = defaultScheme + "://" + uri;
            return uri;
        }
    }
}
