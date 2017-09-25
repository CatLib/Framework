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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 路由组
    /// </summary>
    public interface IRouteGroup
    {
        /// <summary>
        /// 增加路由条目到路由组中
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <returns>当前路由组实例</returns>
        IRouteGroup AddRoute(IRoute route);

        /// <summary>
        /// 设定参数的默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">参数值</param>
        /// <returns>当前路由组实例</returns>
        IRouteGroup Defaults(string name, string val);

        /// <summary>
        /// 约束指定参数必须符合正则表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="pattern">约束的正则表达式</param>
        /// <returns>当前路由组实例</returns>
        IRouteGroup Where(string name, string pattern);

        /// <summary>
        /// 添加路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前路由组实例</returns>
        IRouteGroup Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware, int priority = int.MaxValue);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="onError">错误处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前路由组实例</returns>
        IRouteGroup OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> onError, int priority = int.MaxValue);
    }
}