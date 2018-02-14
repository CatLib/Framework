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
using System.Reflection;

namespace CatLib.API.Routing
{
    /// <summary>
    /// 路由接口
    /// </summary>
    public interface IRouter
    {
        /// <summary>
        /// 根据回调行为注册一个路由
        /// </summary>
        /// <param name="uri">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns>当前实例</returns>
        IRoute Reg(string uri, Action action);

        /// <summary>
        /// 根据回调行为注册一个路由
        /// </summary>
        /// <param name="uri">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns>当前实例</returns>
        IRoute Reg<T>(string uri, Action<T> action);

        /// <summary>
        /// 根据回调行为注册一个路由
        /// </summary>
        /// <param name="uri">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns>当前实例</returns>
        IRoute Reg<T1, T2>(string uri, Action<T1, T2> action);

        /// <summary>
        /// 根据回调行为注册一个路由
        /// </summary>
        /// <param name="uri">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns>当前实例</returns>
        IRoute Reg<T1, T2, T3>(string uri, Action<T1, T2, T3> action);

        /// <summary>
        /// 根据回调行为注册一个路由
        /// </summary>
        /// <param name="uri">统一资源标识符</param>
        /// <param name="action">行为</param>
        /// <returns>当前实例</returns>
        IRoute Reg<T1, T2, T3, T4>(string uri, Action<T1, T2, T3, T4> action);

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="uris">统一资源标识符</param>
        /// <param name="target">行为调度目标</param>
        /// <param name="methodInfo">调度方法</param>
        IRoute Reg(string uris, object target, MethodInfo methodInfo);

        /// <summary>
        /// 根据控制器的type和调用的方法名字注册一个路由
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="controller">控制器类型</param>
        /// <param name="method">调用的方法名</param>
        /// <returns>当前实例</returns>
        IRoute Reg(string uri, Type controller, string method);

        /// <summary>
        /// 设定默认的scheme
        /// </summary>
        /// <param name="scheme">默认的scheme</param>
        /// <returns>当前实例</returns>
        IRouter SetDefaultScheme(string scheme);

        /// <summary>
        /// 当路由没有找到时
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前实例</returns>
        IRouter OnNotFound(Action<IRequest, Action<IRequest>> middleware, int priority = int.MaxValue);

        /// <summary>
        /// 全局路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前路由器实例</returns>
        IRouter Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware, int priority = int.MaxValue);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware">错误处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前实例</returns>
        IRouter OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> middleware, int priority = int.MaxValue);

        /// <summary>
        /// 调度路由
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="context">上下文</param>
        /// <returns>请求响应</returns>
        IResponse Dispatch(string uri, object context = null);

        /// <summary>
        /// 建立或者获取一个已经建立的路由组
        /// </summary>
        /// <param name="name">路由组名字</param>
        /// <returns>当前实例</returns>
        IRouteGroup Group(string name);

        /// <summary>
        /// 建立匿名路由组，调用的闭包内为路由组有效范围, 允许给定一个名字来显示命名路由组
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="name">路由组名字</param>
        /// <returns>当前实例</returns>
        IRouteGroup Group(Action area, string name = null);
    }
}