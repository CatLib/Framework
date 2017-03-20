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

using System.Collections.Generic;
using CatLib.API.Routing;

namespace CatLib.Routing
{

    /// <summary>
    /// 方案
    /// </summary>
    public class Scheme
    {

        /// <summary>
        /// 路由条目列表
        /// </summary>
        protected List<Route> routes = new List<Route>();

        /// <summary>
        /// 路由器
        /// </summary>
        protected Router router;

        /// <summary>
        /// 方案名
        /// </summary>
        protected string name;

        /// <summary>
        /// Scheme Name
        /// </summary>
        public string Name { get{ return name; } }

        public Scheme(string name){

            this.name = name;

        }

        /// <summary>
        /// 设定路由器
        /// </summary>
        /// <param name="router"></param>
        public void SetRouter(Router router)
        {
            this.router = router;
        }

        /// <summary>
        /// 增加一个路由
        /// </summary>
        /// <param name="route"></param>
        public void AddRoute(Route route)
        {
            routes.Add(route);
        }

        /// <summary>
        /// 匹配一个路由
        /// </summary>
        /// <param name="request"></param>
        public Route Match(Request request) {

            Route route = MatchAgainstRoutes(request);
            if (route != null) { return route; }

            throw new NotFoundRouteException("can not find route: " + request.Uri);

        }

        /// <summary>
        /// 匹配路由
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        protected Route MatchAgainstRoutes(Request request)
        {
            for(int i = 0; i < routes.Count; i++)
            {
                if (routes[i].Matches(request))
                {
                    return routes[i];
                }
            }
            return null;
        }

    }

}