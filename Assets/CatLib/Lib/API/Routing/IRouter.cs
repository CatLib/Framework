
using System;
using CatLib.API.FilterChain;

namespace CatLib.API.Routing
{

    /// <summary>
    /// 路由接口
    /// </summary>
    public interface IRouter
    {

        /// <summary>
        /// 注册一个路由方案
        /// </summary>
        /// <param name="url">统一资源定位符</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        IRoutingBind Reg(string url, Action<IRequest, IResponse> action);

        /// <summary>
        /// 当路由没有找到时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRouter OnNotFound(Action<IRequest, IFilterChain<IRequest>> middleware);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRouter OnError(Action<IRequest, Exception, IFilterChain<IRequest , Exception>> middleware);

        /// <summary>
        /// 调度路由
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        IResponse Dispatch(string uri, object context = null);

    }

}