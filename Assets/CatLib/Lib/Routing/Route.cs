using System;
using CatLib.API.Routing;
using CatLib.API.Container;
using CatLib.API.FilterChain;

namespace CatLib.Routing
{
    
    /// <summary>
    /// 路由条目
    /// </summary>
    public class Route : IRoutingBind
    {

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
        /// 路由请求过滤链
        /// </summary>
        protected IFilterChain<IRequest, IResponse> middleware;

        /// <summary>
        /// 当路由出现异常时的过滤器链
        /// </summary>
        protected IFilterChain<IRequest, Exception> onError;

        /// <summary>
        /// 创建一个新的路由条目
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        public Route(string url , Action<IRequest, IResponse> action)
        {

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
        /// 别名，与别名相匹配的路由也将会路由到指定的绑定中
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IRoutingBind Alias(string name)
        {
            return this;
        }

        /// <summary>
        /// 约束指定参数必须符合指定模式才会被路由
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IRoutingBind Where(string name, string pattern)
        {
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRoutingBind OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> middleware)
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
        public IRoutingBind Middleware(Action<IRequest, IResponse , IFilterChain<IRequest, IResponse>> middleware)
        {
            if (this.middleware == null)
            {
                this.middleware = filterChain.Create<IRequest , IResponse>();
            }
            this.middleware.Add(middleware);
            return this;
        }

        /// <summary>
        /// 为当前路由定义一个名字，允许通过名字来路由
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IRoutingBind Name(string name)
        {
            return this;
        }

        /// <summary>
        /// 以异步的方式进行路由
        /// </summary>
        /// <returns></returns>
        public IRoutingBind Async()
        {
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
            return false;
        }

    }

}