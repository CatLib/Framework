
using System;

namespace CatLib.API.Routing
{

    public interface IRoutingBind
    {

        /// <summary>
        /// 别名，与别名相匹配的路由也将会路由到指定的绑定中
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IRoutingBind Alias(string name);

        /// <summary>
        /// 约束指定参数必须符合指定模式才会被路由
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IRoutingBind Where(string name, string pattern);

        /// <summary>
        /// 在路由之前
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns></returns>
        IRoutingBind OnBefore(Func<IRequest, bool> middleware);

        /// <summary>
        /// 在路由之后
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns></returns>
        IRoutingBind OnAfter(Func<IResponse> middleware);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRoutingBind OnError(Func<IRequest, System.Exception, bool> middleware);

        /// <summary>
        /// 为当前路由定义一个名字，允许通过名字来路由
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IRoutingBind Name(string name);

        /// <summary>
        /// 以异步的方式进行路由
        /// </summary>
        /// <returns></returns>
        IRoutingBind Async();

    }

}