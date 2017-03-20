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
using CatLib.API.Routing;
using CatLib.API.Container;
using CatLib.API.FilterChain;

namespace CatLib.Routing
{
    
    /// <summary>
    /// 路由条目
    /// </summary>
    public class Route : IRoute
    {

        /// <summary>
        /// 路由行为
        /// </summary>
        protected class RouteAction
        {

            /// <summary>
            /// 路由行为类型
            /// </summary>
            public enum RouteTypes
            {
                CallBack,
            }

            /// <summary>
            /// 类型
            /// </summary>
            public RouteTypes Type { get; set; }

            /// <summary>
            /// 回调行为
            /// </summary>
            public Action<IRequest, IResponse> Action { get; set; }
        }

        /// <summary>
        /// 验证器
        /// </summary>
        protected static IValidators[] validators;

        /// <summary>
        /// 统一资源标识
        /// </summary>
        protected Uri uri;

        /// <summary>
        /// 统一资源标识
        /// </summary>
        public Uri Uri { get { return uri; } }

        /// <summary>
        /// 路由器
        /// </summary>
        protected Router router;

        /// <summary>
        /// 方案
        /// </summary>
        protected Scheme scheme;

        /// <summary>
        /// 容器
        /// </summary>
        protected IContainer container;

        /// <summary>
        /// 过滤器链生成器
        /// </summary>
        protected IFilterChain filterChain;

        /// <summary>
        /// 编译后的路由器信息
        /// </summary>
        protected CompiledRoute compiled;

        /// <summary>
        /// 编译后的路由器信息
        /// </summary>
        public CompiledRoute Compiled{

            get{

                CompileRoute();
                return compiled;

            }

        }

        /// <summary>
        /// 路由请求过滤链
        /// </summary>
        protected IFilterChain<IRequest, IResponse> middleware;

        /// <summary>
        /// 当路由出现异常时的过滤器链
        /// </summary>
        protected IFilterChain<IRequest, Exception> onError;

        /// <summary>
        /// 路由行为
        /// </summary>
        protected RouteAction action;

        /// <summary>
        /// 筛选条件
        /// </summary>
        protected Dictionary<string, string> wheres;

        /// <summary>
        /// 创建一个新的路由条目
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        public Route(Uri uri , Action<IRequest, IResponse> action)
        {
            this.uri = uri;
            this.action = new RouteAction() { Type = RouteAction.RouteTypes.CallBack, Action = action };
        }

        /// <summary>
        /// Host
        /// </summary>
        public string GetHost(){

            return Uri.Host;

        }

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public string GetWhere(string varName)
        {
            if (wheres == null) { return null; }
            if (wheres.ContainsKey(varName))
            {
                return wheres[varName];
            }
            return null;
        }

        /// <summary>
        /// 设定方案
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public Route SetScheme(Scheme scheme)
        {
            this.scheme = scheme;
            return this;
        }

        /// <summary>
        /// 设定路由器
        /// </summary>
        /// <param name="router">路由器</param>
        /// <returns></returns>
        public Route SetRouter(Router router)
        {
            this.router = router;
            return this;
        }

        /// <summary>
        /// 设定容器
        /// </summary>
        /// <param name="container">容器</param>
        /// <returns></returns>
        public Route SetContainer(IContainer container)
        {
            this.container = container;
            return this;
        }

        /// <summary>
        /// 设定过滤器链生成器
        /// </summary>
        /// <param name="filterChain"></param>
        /// <returns></returns>
        public Route SetFilterChain(IFilterChain filterChain)
        {
            this.filterChain = filterChain;
            return this;
        }

        /// <summary>
        /// 获取验证器列表
        /// </summary>
        /// <returns></returns>
        public static IValidators[] GetValidators()
        {
            if(validators != null)
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
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IRoute Where(string name, string pattern)
        {
            if (wheres == null) { wheres = new Dictionary<string, string>(); }
            if (wheres.ContainsKey(name)) { wheres.Remove(name); }
            wheres[name] = pattern;
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRoute OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> middleware)
        {
            if (onError == null)
            {
                onError = filterChain.Create<IRequest , Exception>();
            }
            onError.Add(middleware);
            return this;
        }

        /// <summary>
        /// 路由中间件
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRoute Middleware(Action<IRequest, IResponse , IFilterChain<IRequest, IResponse>> middleware)
        {
            if (this.middleware == null)
            {
                this.middleware = filterChain.Create<IRequest , IResponse>();
            }
            this.middleware.Add(middleware);
            return this;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public IResponse Run(IRequest request, IResponse response)
        {
            RouteParameterBinder.Parameters(this , request);
            if (action.Type == RouteAction.RouteTypes.CallBack)
            {
                action.Action(request, response);
            }
            return response;
        }

        /// <summary>
        /// 获取路由的中间件
        /// </summary>
        /// <returns></returns>
        public IFilterChain<IRequest, IResponse> GatherMiddleware()
        {
            return middleware;
        }

        /// <summary>
        /// 获取当出现错误时的过滤器链
        /// </summary>
        /// <returns></returns>
        public IFilterChain<IRequest, Exception> GatherOnError()
        {
            return onError;
        }

        /// <summary>
        /// 当前路由条目是否符合请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool Matches(Request request)
        {

            CompileRoute();
            GetValidators();
 
            for (int i = 0; i < validators.Length; i++)
            {
                if (!validators[i].Matches(this, request))
                {
                    return false;
                }
            }

            UnityEngine.Debug.Log(Compiled.ToString());

            return true;

        }

        /// <summary>
        /// 编译路由条目
        /// </summary>
        protected void CompileRoute()
        {
            if(compiled == null)
            {
                compiled = RouteCompiler.Compile(this);
            }
        }

    }

}