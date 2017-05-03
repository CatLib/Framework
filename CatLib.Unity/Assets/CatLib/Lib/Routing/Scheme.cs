﻿/*
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
    internal sealed class Scheme
    {
        /// <summary>
        /// 路由条目列表
        /// </summary>
        private readonly List<Route> routes = new List<Route>();

        /// <summary>
        /// 路由器
        /// </summary>
        private Router router;

        /// <summary>
        /// 方案名
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Scheme Name
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// 新建一个方案
        /// </summary>
        /// <param name="name">方案名</param>
        public Scheme(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 设定路由器
        /// </summary>
        /// <param name="router">路由器</param>
        public void SetRouter(Router router)
        {
            this.router = router;
        }

        /// <summary>
        /// 增加一个路由
        /// </summary>
        /// <param name="route">路由条目</param>
        public void AddRoute(Route route)
        {
            routes.Add(route);
        }

        /// <summary>
        /// 匹配一个路由
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>匹配到的路由条目</returns>
        public Route Match(Request request)
        {
            var route = MatchAgainstRoutes(request);
            if (route != null)
            {
                return route;
            }

            throw new NotFoundRouteException("can not find route: " + request.RouteUri.FullPath);
        }

        /// <summary>
        /// 匹配路由
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>匹配到的路由条目</returns>
        private Route MatchAgainstRoutes(Request request)
        {
            for (var i = 0; i < routes.Count; i++)
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