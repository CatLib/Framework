
using System.Collections.Generic;
using CatLib.API.Routing;

namespace CatLib.Routing
{

    /// <summary>
    /// 方案
    /// </summary>
    public abstract class Scheme
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
        /// Scheme Name
        /// </summary>
        public abstract string Name { get; }

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