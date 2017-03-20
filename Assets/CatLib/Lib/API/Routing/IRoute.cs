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