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
using CatLib.API.Routing;
using CatLib.API.FilterChain;
using System.Collections.Generic;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由配置
    /// </summary>
    public class RouteOptions
    {

        /// <summary>
        /// 过滤器链生成器
        /// </summary>
        protected IFilterChain filterChain;

        /// <summary>
        /// 路由请求过滤链
        /// </summary>
        protected IFilterChain<IRequest, IResponse> middleware;

        /// <summary>
        /// 当路由出现异常时的过滤器链
        /// </summary>
        protected IFilterChain<IRequest, Exception> onError;

        /// <summary>
        /// 筛选条件
        /// </summary>
        protected Dictionary<string, string> wheres;

        /// <summary>
        /// 默认值
        /// </summary>
        protected Dictionary<string, string> defaults;

        /// <summary>
        /// 设定过滤器链生成器
        /// </summary>
        /// <param name="filterChain"></param>
        /// <returns></returns>
        public RouteOptions SetFilterChain(IFilterChain filterChain)
        {
            this.filterChain = filterChain;
            return this;
        }

        /// <summary>
        /// 获取参数默认值
        /// </summary>
        /// <param name="name">参数名</param>
        public string GetDefaults(string name, string defaultValue = null)
        {

            if (defaults == null) { return defaultValue; }
            if (!defaults.ContainsKey(name)) { return defaultValue; }
            return defaults[name];

        }

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public string GetWhere(string varName)
        {
            if (wheres == null) { return null; }
            if (wheres.ContainsKey(varName))
            {
                return wheres[varName];
            }
            return null;
        }

        /// <summary>
        /// 约束指定参数必须符合指定模式才会被路由
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public RouteOptions Where(string name, string pattern, bool overrided = true)
        {
            if (wheres == null) { wheres = new Dictionary<string, string>(); }
            if (wheres.ContainsKey(name))
            {
                if (!overrided) { return this; }
                wheres.Remove(name);
            }
            wheres[name] = pattern;
            return this;
        }

        /// <summary>
        /// 设定默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">默认值</param>
        public RouteOptions Defaults(string name, string val, bool overrided = true)
        {

            if (defaults == null)
            {
                defaults = new Dictionary<string, string>();
            }
            if (defaults.ContainsKey(name))
            {
                if (!overrided) { return this; }
                defaults.Remove(name);
            }
            defaults.Add(name, val);

            return this;

        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public RouteOptions OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> middleware)
        {
            if (onError == null)
            {
                onError = filterChain.Create<IRequest, Exception>();
            }
            onError.Add(middleware);
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public RouteOptions OnError(IFilter<IRequest, Exception> middleware)
        {
            if (onError == null)
            {
                onError = filterChain.Create<IRequest, Exception>();
            }
            onError.Add(middleware);
            return this;
        }

        /// <summary>
        /// 路由中间件
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public RouteOptions Middleware(Action<IRequest, IResponse, IFilterChain<IRequest, IResponse>> middleware)
        {
            if (this.middleware == null)
            {
                this.middleware = filterChain.Create<IRequest, IResponse>();
            }
            this.middleware.Add(middleware);
            return this;
        }

        /// <summary>
        /// 路由中间件
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public RouteOptions Middleware(IFilter<IRequest ,IResponse> middleware)
        {
            if (this.middleware == null)
            {
                this.middleware = filterChain.Create<IRequest, IResponse>();
            }
            this.middleware.Add(middleware);
            return this;
        }

        /// <summary>
        /// 获取路由的中间件
        /// </summary>
        /// <returns></returns>
        public IFilterChain<IRequest, IResponse> GatherMiddleware()
        {
            return middleware;
        }

        /// <summary>
        /// 获取当出现错误时的过滤器链
        /// </summary>
        /// <returns></returns>
        public IFilterChain<IRequest, Exception> GatherOnError()
        {
            return onError;
        }

        /// <summary>
        /// 将当前路由配置中的信息合并到给定的路由配置中
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public RouteOptions Merge(RouteOptions options)
        {

            //合并的过程是一个平级的过程，所以遵循以下原则：
            //当key不允许重复时，如果已经有值则不进行赋值
            //当key是允许重复时，所有的合并操作追加在原始key list之后

            MergeMiddleware(options);
            MergeOnError(options);
            MergeWhere(options);
            MergeDefaults(options);
            return this;
        }

        /// <summary>
        /// 合并中间件
        /// </summary>
        /// <param name="options"></param>
        protected void MergeMiddleware(RouteOptions options)
        {
            if (middleware == null) { return; }
            IFilter<IRequest, IResponse>[] filters = middleware.FilterList;
            for (int i = 0; i< filters.Length; i++)
            {
                options.Middleware(filters[i]);
            }
        }


        /// <summary>
        /// 合并错误时的调度
        /// </summary>
        /// <param name="options"></param>
        protected void MergeOnError(RouteOptions options)
        {
            if (onError == null) { return; }
            IFilter<IRequest, Exception>[] filters = onError.FilterList;
            for (int i = 0; i < filters.Length; i++)
            {
                options.OnError(filters[i]);
            }
        }

        /// <summary>
        /// 合并where
        /// </summary>
        /// <param name="options"></param>
        protected void MergeWhere(RouteOptions options)
        {
            if (wheres == null) { return; }

            foreach(var kv in wheres)
            {
                options.Where(kv.Key, kv.Value , false);
            }
        }

        /// <summary>
        /// 合并默认值
        /// </summary>
        /// <param name="options"></param>
        protected void MergeDefaults(RouteOptions options)
        {
            if (defaults == null) { return; }

            foreach (var kv in defaults)
            {
                options.Defaults(kv.Key, kv.Value , false);
            }
        }

    }

}