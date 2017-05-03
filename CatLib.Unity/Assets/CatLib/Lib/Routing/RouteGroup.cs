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

using System;
using CatLib.API.FilterChain;
using CatLib.API.Routing;
using System.Collections.Generic;

namespace CatLib.Routing
{
    /// <summary>
    /// 路由组
    /// </summary>
    internal sealed class RouteGroup : IRouteGroup
    {
        /// <summary>
        /// 路由配置
        /// </summary>
        private readonly RouteOptions options;

        /// <summary>
        /// 在路由组中的路由条目
        /// </summary>
        private readonly List<IRoute> routes;

        /// <summary>
        /// 路由组
        /// </summary>
        public RouteGroup()
        {
            options = new RouteOptions();
            routes = new List<IRoute>();
        }

        /// <summary>
        /// 设定过滤器链生成器
        /// </summary>
        /// <param name="filterChain">过滤器链</param>
        /// <returns>路由组</returns>
        public RouteGroup SetFilterChain(IFilterChain filterChain)
        {
            options.SetFilterChain(filterChain);
            return this;
        }

        /// <summary>
        /// 增加路由条目到路由组中
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <returns>当前路由组实例</returns>
        public IRouteGroup AddRoute(IRoute route)
        {
            if (route is Route)
            {
                options.Merge(((Route)route).Options);
            }
            routes.Add(route);
            return this;
        }

        /// <summary>
        /// 设定参数的默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">参数值</param>
        /// <returns>当前路由组实例</returns>
        public IRouteGroup Defaults(string name, string val)
        {
            options.Defaults(name, val);
            for (var i = 0; i < routes.Count; i++)
            {
                routes[i].Defaults(name, val, false);
            }
            return this;
        }

        /// <summary>
        /// 约束指定参数必须符合正则表达式
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="pattern">约束的正则表达式</param>
        /// <returns>当前路由组实例</returns>
        public IRouteGroup Where(string name, string pattern)
        {
            options.Where(name, pattern);
            for (var i = 0; i < routes.Count; i++)
            {
                routes[i].Where(name, pattern, false);
            }
            return this;
        }

        /// <summary>
        /// 添加路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns>当前路由组实例</returns>
        public IRouteGroup Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware)
        {
            options.Middleware(middleware);
            for (var i = 0; i < routes.Count; i++)
            {
                routes[i].Middleware(middleware);
            }
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="onError">错误处理函数</param>
        /// <returns>当前路由组实例</returns>
        public IRouteGroup OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> onError)
        {
            options.OnError(onError);
            for (var i = 0; i < routes.Count; i++)
            {
                routes[i].OnError(onError);
            }
            return this;
        }
    }
}