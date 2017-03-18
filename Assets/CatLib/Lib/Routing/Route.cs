using System;
using CatLib.API.Routing;
using CatLib.API.Container;

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
        /// 容器
        /// </summary>
        protected IContainer container;


        /// <summary>
        /// 创建一个新的路由条目
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        public Route(string url , Action<IRequest, IResponse> action)
        {

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
        /// 在路由之前
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns></returns>
        public IRoutingBind OnBefore(Func<IRequest, bool> middleware)
        {
            return this;
        }

        /// <summary>
        /// 在路由之后
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns></returns>
        public IRoutingBind OnAfter(Func<IResponse> middleware)
        {
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRoutingBind OnError(Func<IRequest, System.Exception, bool> middleware)
        {
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

    }

}