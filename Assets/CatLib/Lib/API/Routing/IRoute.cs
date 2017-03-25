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

    public interface IRoute
    {

        /// <summary>
        /// 将当前路由条目追加到指定路由组中
        /// </summary>
        /// <param name="name">路由组名</param>
        /// <returns></returns>
        IRoute Group(string name);

        /// <summary>
        /// 设定参数的默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">参数值</param>
        IRoute Defaults(string name, string val);

        /// <summary>
        /// 约束指定参数必须符合正则表达式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IRoute Where(string name, string pattern);

        /// <summary>
        /// 在路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns></returns>
        IRoute Middleware(Action<IRequest, IResponse, IFilterChain<IRequest, IResponse>> middleware);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRoute OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> onError);

    }

}