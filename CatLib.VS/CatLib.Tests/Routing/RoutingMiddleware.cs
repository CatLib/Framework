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

using CatLib.API.FilterChain;
using CatLib.API.Routing;

namespace CatLib.Tests.Routing
{
    [Routed("rm://")]
    public class RoutingMiddleware : IMiddleware
    {
        /// <summary>
        /// 中间件
        /// </summary>
        public IFilterChain<IRequest, IResponse> Middleware
        {
            get
            {
                var filterChain = App.Instance.Make<IFilterChain>();
                var filter = filterChain.Create<IRequest, IResponse>();
                filter.Add((req, res, next) =>
                {
                    next(req, res);
                    res.SetContext(res.GetContext() + "[with middleware]");
                });
                return filter;
            }
        }

        [Routed("call")]
        public void Call(IRequest request, IResponse response)
        {
            response.SetContext("RoutingMiddleware.Call");
        }
    }
}
