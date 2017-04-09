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
        IRoute Reg(string uri, Action<IRequest, IResponse> action);

        /// <summary>
        /// 注册一个路由方案
        /// </summary>
        /// <param name="url">统一资源定位符</param>
        /// <param name="controller">控制器类型</param>
        /// <param name="func">控制器方法名</param>
        /// <returns></returns>
        IRoute Reg(string uri, Type controller, string func);

        /// <summary>
        /// 设定默认的Scheme
        /// </summary>
        /// <param name="scheme">方案名</param>
        /// <returns></returns>
        IRouter SetDefaultScheme(string scheme);

        /// <summary>
        /// 当路由没有找到时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRouter OnNotFound(Action<IRequest, Action<IRequest>> middleware);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRouter OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse , Exception>> middleware);

        /// <summary>
        /// 调度路由
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        IResponse Dispatch(string uri, object context = null);

        /// <summary>
        /// 路由组
        /// </summary>
        /// <param name="name">组名</param>
        /// <returns></returns>
        IRouteGroup Group(string name);

        /// <summary>
        /// 建立匿名路由组，调用的闭包内为路由组有效范围, 允许给定一个名字来显示命名路由组
        /// </summary>
        /// <param name="area">作用范围</param>
        /// <param name="name">路由组名</param>
        /// <returns></returns>
        IRouteGroup Group(Action area, string name = null);

    }

}