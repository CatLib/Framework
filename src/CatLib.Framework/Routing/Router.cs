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
using System;
using System.Collections.Generic;

namespace CatLib.Routing
{
    /// <summary>
    /// 路由服务
    /// </summary>
    public sealed class Router : IRouter
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        private const char Separator = '/';

        /// <summary>
        /// 全局调度器
        /// </summary>
        private readonly IDispatcher dispatcher;

        /// <summary>
        /// 容器
        /// </summary>
        private readonly IContainer container;

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
        /// 路由条目调用堆栈
        /// </summary>
        private readonly Stack<Route> routeStack;

        /// <summary>
        /// 响应堆栈
        /// </summary>
        private readonly Stack<IResponse> responseStack;

        /// <summary>
        /// 默认的scheme
        /// </summary>
        private string defaultScheme;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 创建一个新的路由器
        /// </summary>
        /// <param name="dispatcher">事件调度器</param>
        /// <param name="container">容器</param>
        public Router(IDispatcher dispatcher, IContainer container)
        {
            this.dispatcher = dispatcher;
            this.container = container;
            schemes = new Dictionary<string, Scheme>();
            routeGroupStack = new Stack<IRouteGroup>();
            routeStack = new Stack<Route>();
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
            lock (syncRoot)
            {
                defaultScheme = scheme;
            }
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
            Guard.NotEmptyOrNull(uris, "uris");
            Guard.Requires<ArgumentNullException>(action != null);
            lock (syncRoot)
            {
                return RegisterRoute(uris, new RouteAction
                {
                    Type = RouteAction.RouteTypes.CallBack,
                    Action = action,
                });
            }
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
            Guard.NotEmptyOrNull(uris, "uris");
            Guard.Requires<ArgumentNullException>(controller != null);
            Guard.NotEmptyOrNull(func, "func");
            lock (syncRoot)
            {
                return RegisterRoute(uris, new RouteAction
                {
                    Type = RouteAction.RouteTypes.ControllerCall,
                    Controller = controller,
                    Func = func
                });
            }
        }

        /// <summary>
        /// 当路由没有找到时
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前实例</returns>
        public IRouter OnNotFound(Action<IRequest, Action<IRequest>> middleware, int priority = int.MaxValue)
        {
            Guard.Requires<ArgumentNullException>(middleware != null);
            lock (syncRoot)
            {
                if (onNotFound == null)
                {
                    onNotFound = new FilterChain<IRequest>();
                }
                onNotFound.Add(middleware);
                return this;
            }
        }

        /// <summary>
        /// 全局路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前路由器实例</returns>
        public IRouter Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware, int priority = int.MaxValue)
        {
            Guard.Requires<ArgumentNullException>(middleware != null);
            lock (syncRoot)
            {
                if (this.middleware == null)
                {
                    this.middleware = new FilterChain<IRequest, IResponse>();
                }
                this.middleware.Add(middleware);
                return this;
            }
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="onError">错误处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前实例</returns>
        public IRouter OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> onError, int priority = int.MaxValue)
        {
            Guard.Requires<ArgumentNullException>(onError != null);
            lock (syncRoot)
            {
                if (this.onError == null)
                {
                    this.onError = new FilterChain<IRequest, IResponse, Exception>();
                }
                this.onError.Add(onError);
                return this;
            }
        }

        /// <summary>
        /// 调度路由
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="context">上下文</param>
        /// <returns>请求响应</returns>
        public IResponse Dispatch(string uri, object context = null)
        {
            lock (syncRoot)
            {
                Route route = null;
                Request request = null;
                var response = new Response();
                try
                {
                    request = MakeRequest(uri, context);

                    try
                    {
                        route = FindRoute(request);
                    }
                    catch (NotFoundRouteException)
                    {
                        if (ThrowOnNotFound(request))
                        {
                            return null;
                        }
                        throw;
                    }

                    GuardCircularDependency(route, request);

                    try
                    {
                        EnvironmentPreparation(route, request, response, false);
                        return RunRouteWithMiddleware(route, request, response);
                    }
                    finally
                    {
                        EnvironmentPreparation(route, request, response, true);
                    }
                }
                catch (NotFoundRouteException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (ThrowException(route, request ?? (IRequest)new ExceptionRequest(uri, context), response, ex))
                    {
                        return null;
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// 建立或者获取一个已经建立的路由组
        /// </summary>
        /// <param name="name">路由组名字</param>
        /// <returns>当前实例</returns>
        public IRouteGroup Group(string name)
        {
            lock (syncRoot)
            {
                if (routeGroup == null)
                {
                    routeGroup = new Dictionary<string, RouteGroup>();
                }
                if (name == null)
                {
                    return new RouteGroup();
                }
                if (!routeGroup.ContainsKey(name))
                {
                    routeGroup.Add(name, new RouteGroup());
                }

                return routeGroup[name];
            }
        }

        /// <summary>
        /// 建立匿名路由组，调用的闭包内为路由组有效范围, 允许给定一个名字来显示命名路由组
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="name">路由组名字</param>
        /// <returns>当前实例</returns>
        public IRouteGroup Group(Action area, string name = null)
        {
            Guard.Requires<ArgumentNullException>(area != null);
            lock (syncRoot)
            {
                var group = Group(name);

                routeGroupStack.Push(group);
                area.Invoke();
                routeGroupStack.Pop();

                return group;
            }
        }

        /// <summary>
        /// 路由器编译
        /// </summary>
        /// <returns>迭代器</returns>
        public void RouterCompiler()
        {
            lock (syncRoot)
            {
                var compiler = container.Make<AttrRouteCompiler>();
                if (compiler != null)
                {
                    if (dispatcher != null)
                    {
                        dispatcher.Trigger(RouterEvents.OnBeforeRouterAttrCompiler, this);
                    }
                    compiler.Complie();
                }
            }
        }

        /// <summary>
        /// 保证不处于循环依赖调用
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        private void GuardCircularDependency(Route route, Request request)
        {
            foreach (var r in routeStack)
            {
                if (r.Equals(route))
                {
                    throw new RuntimeException("A circular dependency call was detected , uri [" + request.Uri + "].");
                }
            }
        }

        /// <summary>
        /// 环境预备
        /// </summary>
        /// <param name="route">路由</param>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="isRollback">是否回滚</param>
        private void EnvironmentPreparation(Route route, Request request, Response response, bool isRollback)
        {
            if (!isRollback)
            {
                routeStack.Push(route);
                responseStack.Push(response);
                requestStack.Push(request);
                request.SetRoute(route);

                container.Instance(container.Type2Service(typeof(IRequest)), request);
                container.Instance(container.Type2Service(typeof(IResponse)), response);
            }
            else
            {
                routeStack.Pop();
                requestStack.Pop();
                responseStack.Pop();
                container.Instance(container.Type2Service(typeof(IResponse)),
                    responseStack.Count > 0 ? responseStack.Peek() : null);
                container.Instance(container.Type2Service(typeof(IRequest)),
                    requestStack.Count > 0 ? requestStack.Peek() : null);
            }
        }

        /// <summary>
        /// 触发异常冒泡
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">当前请求</param>
        /// <param name="response">当前响应</param>
        /// <param name="ex">异常</param>
        /// <returns>冒泡是否已经被拦截</returns>
        private bool ThrowException(Route route, IRequest request, IResponse response, Exception ex)
        {
            var chain = route != null ? route.GatherOnError() : null;
            var throwBubble = (chain == null);
            if (!throwBubble)
            {
                // 触发局部异常冒泡(如果局部被捕获则不会向全局冒泡)
                chain.Do(request, response, ex, (req, res, error) =>
                {
                    throwBubble = true;
                });
            }

            return !throwBubble || ThrowOnError(request, response, ex);
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
                MakeScheme(uri.Scheme);
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
            route.SetContainer(container);
            return route;
        }

        /// <summary>
        /// 触发没有找到路由的过滤器链
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>冒泡是否已经被拦截</returns>
        private bool ThrowOnNotFound(IRequest request)
        {
            var isIntercept = (onNotFound != null);
            if (isIntercept)
            {
                onNotFound.Do(request, (req) =>
               {
                   isIntercept = false;
               });
            }
            return isIntercept;
        }

        /// <summary>
        /// 触发异常
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="ex">异常</param>
        /// <returns>冒泡是否已经被拦截</returns>
        private bool ThrowOnError(IRequest request, IResponse response, Exception ex)
        {
            var isIntercept = (onError != null);
            if (isIntercept)
            {
                onError.Do(request, response, ex, (req, res, exception) =>
                {
                    isIntercept = false;
                });
            }

            return isIntercept;
        }

        /// <summary>
        /// 通过中间件后执行路由请求
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <returns>响应</returns>
        private IResponse RunRouteWithMiddleware(Route route, Request request, Response response)
        {
            if (dispatcher != null)
            {
                dispatcher.Trigger(RouterEvents.OnDispatcher, new DispatchEventArgs(this, route, request));
            }

            if (middleware != null)
            {
                middleware.Do(request, response, (req, res) =>
                {
                    route.Run(request, response);
                });
            }
            else
            {
                route.Run(request, response);
            }
            return response;
        }

        /// <summary>
        /// 增加一个处理方案
        /// </summary>
        /// <param name="name">scheme名字</param>
        /// <returns>当前路由实例</returns>
        private IRouter MakeScheme(string name)
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
            if (!schemes.ContainsKey(request.RouteUri.Scheme))
            {
                throw new NotFoundRouteException("Can not find scheme [" + request.RouteUri.Scheme + "].");
            }
            var route = schemes[request.RouteUri.Scheme].Match(request);
            return route;
        }

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="context">上下文</param>
        /// <returns>请求</returns>
        private Request MakeRequest(string uri, object context)
        {
            uri = GuardUri(uri);
            uri = Prefix(uri);
            return new Request(uri, context);
        }

        /// <summary>
        /// 处理uri为符合规则的url
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>处理后的url</returns>
        private string Prefix(string url)
        {
            return (GetLastGroupPrefix().Trim(Separator) + Separator + url.Trim(Separator)).Trim(Separator);
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
            Guard.NotEmptyOrNull(uri, "uri");

            if (uri.IndexOf(@"://") >= 0)
            {
                return uri;
            }
            if (string.IsNullOrEmpty(defaultScheme))
            {
                throw new UndefinedDefaultSchemeException("Undefined default scheme , Please call SetDefaultScheme(string) first.");
            }
            uri = defaultScheme + "://" + uri;
            return uri;
        }
    }
}
