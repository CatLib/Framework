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

namespace CatLib.Routing
{
    /// <summary>
    /// 路由条目
    /// </summary>
    internal sealed class Route : IRoute
    {
        /// <summary>
        /// 验证器
        /// </summary>
        private static IValidators[] validators;

        /// <summary>
        /// 统一资源标识
        /// </summary>
        private readonly Uri uri;

        /// <summary>
        /// 统一资源标识
        /// </summary>
        internal Uri Uri
        {
            get
            {
                return uri;
            }
        }

        /// <summary>
        /// 路由器
        /// </summary>
        private Router router;

        /// <summary>
        /// 路由配置
        /// </summary>
        private readonly RouteOptions options;

        /// <summary>
        /// 路由配置
        /// </summary>
        internal RouteOptions Options
        {
            get
            {
                return options;
            }
        }

        /// <summary>
        /// 编译后的路由器信息
        /// </summary>
        private CompiledRoute compiled;

        /// <summary>
        /// 编译后的路由器信息
        /// </summary>
        internal CompiledRoute Compiled
        {
            get
            {
                CompileRoute();
                return compiled;
            }
        }

        /// <summary>
        /// 容器
        /// </summary>
        private IContainer container;

        /// <summary>
        /// 路由行为
        /// </summary>
        private readonly RouteAction action;

        /// <summary>
        /// 创建一个新的路由条目
        /// </summary>
        /// <param name="uri">uri信息</param>
        /// <param name="action">路由行为</param>
        public Route(Uri uri, RouteAction action)
        {
            this.uri = uri;
            this.action = action;
            options = new RouteOptions();
            options.OnCompiledChange += ClearCompile;
        }

        /// <summary>
        /// 设定容器
        /// </summary>
        /// <param name="container">容器</param>
        /// <returns>当前路由条目</returns>
        public Route SetContainer(IContainer container)
        {
            this.container = container;
            return this;
        }

        /// <summary>
        /// 设定路由器
        /// </summary>
        /// <param name="router">路由器</param>
        /// <returns>当前路由条目</returns>
        public Route SetRouter(Router router)
        {
            this.router = router;
            return this;
        }

        /// <summary>
        /// 获取参数默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        public string GetDefaults(string name, string defaultValue = null)
        {
            return options.GetDefaults(name, defaultValue);
        }

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        /// <param name="varName">参数名</param>
        /// <returns>筛选条件</returns>
        public string GetWhere(string varName)
        {
            return options.GetWhere(varName);
        }

        /// <summary>
        /// 获取验证器列表
        /// </summary>
        /// <returns>验证器列表</returns>
        public static IValidators[] GetValidators()
        {
            if (validators != null)
            {
                return validators;
            }

            return validators = new IValidators[] {
                                                    new HostValidator(),
                                                    new UriValidator()
                                                };
        }

        /// <summary>
        /// 约束指定参数必须符合指定模式才会被路由
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="pattern">约束参数</param>
        /// <param name="overrided">是否覆盖</param>
        /// <returns>路由条目实例</returns>
        public IRoute Where(string name, string pattern, bool overrided = true)
        {
            Guard.Requires<ArgumentNullException>(name != null);
            options.Where(name, pattern, overrided);
            return this;
        }

        /// <summary>
        /// 设定默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">默认值</param>
        /// <param name="overrided">是否覆盖</param>
        /// <returns>路由条目实例</returns>
        public IRoute Defaults(string name, string val, bool overrided = true)
        {
            Guard.Requires<ArgumentNullException>(name != null);
            options.Defaults(name, val, overrided);
            return this;
        }

        /// <summary>
        /// 将当前路由条目追加到指定路由组中
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>路由条目实例</returns>
        public IRoute Group(string name)
        {
            Guard.Requires<ArgumentNullException>(name != null);
            router.Group(name).AddRoute(this);
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="onError">执行的处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>路由条目实例</returns>
        public IRoute OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> onError, int priority = int.MaxValue)
        {
            Guard.Requires<ArgumentNullException>(onError != null);
            options.OnError(onError, priority);
            return this;
        }

        /// <summary>
        /// 路由中间件
        /// </summary>
        /// <param name="middleware">执行的处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>路由条目实例</returns>
        public IRoute Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware, int priority = int.MaxValue)
        {
            Guard.Requires<ArgumentNullException>(middleware != null);
            options.Middleware(middleware, priority);
            return this;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <returns>响应</returns>
        public IResponse Run(Request request, Response response)
        {
            RouteParameterBinder.Parameters(this, request);

            DispatchToAction(request, response);

            return response;
        }

        /// <summary>
        /// 获取路由的中间件
        /// </summary>
        /// <returns>中间件过滤器链</returns>
        public IFilterChain<IRequest, IResponse> GatherMiddleware()
        {
            return options.GatherMiddleware();
        }

        /// <summary>
        /// 获取当出现错误时的过滤器链
        /// </summary>
        /// <returns>错误过滤器链</returns>
        public IFilterChain<IRequest, IResponse, Exception> GatherOnError()
        {
            return options.GatherOnError();
        }

        /// <summary>
        /// 当前路由条目是否符合请求
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>是否符合</returns>
        public bool Matches(Request request)
        {
            CompileRoute();
            GetValidators();

            for (var i = 0; i < validators.Length; i++)
            {
                if (!validators[i].Matches(this, request))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 调度到行为
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        private void DispatchToAction(Request request, Response response)
        {
            if (action.Type == RouteAction.RouteTypes.CallBack)
            {
                ThroughRouteMiddleware(request, response, null, ActionCall);
            }
            else if (action.Type == RouteAction.RouteTypes.ControllerCall)
            {
                ThroughControllerMiddleware(request, response, ControllerCallRouteMiddlewareWrap);
            }
            else
            {
                throw new RuntimeException("Undefine action type [" + action.Type + "].");
            }
        }

        /// <summary>
        /// 通过路由中间件
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="context">上下文</param>
        /// <param name="callback">完成中间件的回调</param>
        private void ThroughRouteMiddleware(Request request, Response response, object context, Action<Request, Response, object> callback)
        {
            var middleware = GatherMiddleware();
            if (middleware != null)
            {
                middleware.Do(request, response, (req, res) =>
                {
                    callback.Invoke(request, response, context);
                });
            }
            else
            {
                callback.Invoke(request, response, context);
            }
        }

        /// <summary>
        /// 行为调用
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="context">上下文</param>
        private void ActionCall(Request request, Response response, object context)
        {
            action.Action.Invoke(request, response);
        }

        /// <summary>
        /// 通过控制器中间件
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="callback">回调</param>
        private void ThroughControllerMiddleware(Request request, Response response, Action<Request, Response, object> callback)
        {
            var controller = container.Make(container.Type2Service(action.Controller));

            if (controller == null)
            {
                throw new RuntimeException("Can not Make [" + action.Controller + "] Please check cross assembly Or achieve Container.OnFindType().");
            }

            IMiddleware mid = null;
            if (controller is IMiddleware)
            {
                mid = (controller as IMiddleware);
            }

            if (mid != null && mid.Middleware != null)
            {
                mid.Middleware.Do(request, response, (req, res) =>
                {
                    callback.Invoke(request, response, controller);
                });
            }
            else
            {
                callback.Invoke(request, response, controller);
            }
        }

        /// <summary>
        /// 控制器调用路由中间件包装
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="context"></param>
        private void ControllerCallRouteMiddlewareWrap(Request request, Response response, object context)
        {
            ThroughRouteMiddleware(request, response, context, ControllerCall);
        }

        /// <summary>
        /// 控制器调用
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="context">上下文</param>
        private void ControllerCall(Request request, Response response, object context)
        {
            container.Call(context, action.Func);
        }

        /// <summary>
        /// 清空路由编译条目
        /// </summary>
        private void ClearCompile()
        {
            compiled = null;
        }

        /// <summary>
        /// 编译路由条目
        /// </summary>
        private void CompileRoute()
        {
            if (compiled == null)
            {
                compiled = RouteCompiler.Compile(this);
            }
        }
    }
}