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
    /// 路由条目
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// 将当前路由条目追加到指定路由组中
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>路由条目实例</returns>
        IRoute Group(string name);

        /// <summary>
        /// 设定默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">默认值</param>
        /// <param name="overrided">是否覆盖</param>
        /// <returns>路由条目实例</returns>
        IRoute Defaults(string name, string val, bool overrided = true);

        /// <summary>
        /// 约束指定参数必须符合指定模式才会被路由
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="pattern">约束参数</param>
        /// <param name="overrided">是否覆盖</param>
        /// <returns>路由条目实例</returns>
        IRoute Where(string name, string pattern, bool overrided = true);

        /// <summary>
        /// 路由中间件
        /// </summary>
        /// <param name="middleware">执行的处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>路由条目实例</returns>
        IRoute Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware, int priority = int.MaxValue);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="onError">执行的处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>路由条目实例</returns>
        IRoute OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> onError, int priority = int.MaxValue);
    }
}