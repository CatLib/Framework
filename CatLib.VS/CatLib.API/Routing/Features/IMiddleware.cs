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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 中间件
    /// </summary>
    public interface IMiddleware
    {
        /// <summary>
        /// 路由请求过滤链
        /// </summary>
        IFilterChain<IRequest, IResponse> Middleware { get; }
    }
}