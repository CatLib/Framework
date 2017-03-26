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
using CatLib.API.Container;

namespace CatLib.Routing
{
    
    /// <summary>
    /// 路由条目
    /// </summary>
    public class Route : IRoute
    {

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
        /// 路由配置
        /// </summary>
        protected RouteOptions options;

        /// <summary>
        /// 路由配置
        /// </summary>
        public RouteOptions Options { get { return options; } }

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

        protected IContainer container;

        /// <summary>
        /// 路由行为
        /// </summary>
        protected RouteAction action;

        /// <summary>
        /// 创建一个新的路由条目
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        public Route(Uri uri , RouteAction action)
        {
            this.uri = uri;
            this.action = action;
            options = new RouteOptions();
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
        /// 设定过滤器链生成器
        /// </summary>
        /// <param name="filterChain"></param>
        /// <returns></returns>
        public Route SetFilterChain(IFilterChain filterChain)
        {
            options.SetFilterChain(filterChain);
            return this;
        }

        /// <summary>
        /// 获取参数默认值
        /// </summary>
        /// <param name="name">参数名</param>
        public string GetDefaults(string name , string defaultValue = null){

            return options.GetDefaults(name , defaultValue);

        }

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public string GetWhere(string varName)
        {
            return options.GetWhere(varName);
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
            options.Where(name, pattern);
            return this;
        }

        /// <summary>
        /// 设定默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">默认值</param>
        public IRoute Defaults(string name, string val){

            options.Defaults(name, val);
            return this;

        }

        /// <summary>
        /// 将当前路由条目追加到指定路由组中
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IRoute Group(string name)
        {
            router.Group(name).AddRoute(this);
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRoute OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> middleware)
        {
            options.OnError(middleware);
            return this;
        }

        /// <summary>
        /// 路由中间件
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRoute Middleware(Action<IRequest, IResponse , IFilterChain<IRequest, IResponse>> middleware)
        {
            options.Middleware(middleware);
            return this;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public IResponse Run(Request request, Response response)
        {
            RouteParameterBinder.Parameters(this , request as Request);
            if (action.Type == RouteAction.RouteTypes.CallBack)
            {
                action.Action(request, response);
            }
            else if (action.Type == RouteAction.RouteTypes.ControllerCall)
            {
                ControllerCall(request, response);
            }
            return response;
        }

        /// <summary>
        /// 获取路由的中间件
        /// </summary>
        /// <returns></returns>
        public IFilterChain<IRequest, IResponse> GatherMiddleware()
        {
            return options.GatherMiddleware();
        }

        /// <summary>
        /// 获取当出现错误时的过滤器链
        /// </summary>
        /// <returns></returns>
        public IFilterChain<IRequest, Exception> GatherOnError()
        {
            return options.GatherOnError();
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
            UnityEngine.Debug.Log(Compiled.ToString());
            for (int i = 0; i < validators.Length; i++)
            {
                if (!validators[i].Matches(this, request))
                {
                    return false;
                }
            }

            //UnityEngine.Debug.Log(Compiled.ToString());

            return true;

        }

        
        /// <summary>
        /// 控制器调用
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        protected void ControllerCall(Request request, Response response)
        {
            object controller = container.Make(action.Controller);
            container.Call(controller, action.Func, request, response);
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