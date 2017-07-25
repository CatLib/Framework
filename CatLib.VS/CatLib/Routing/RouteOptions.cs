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

using CatLib.API.Routing;
using System;
using System.Collections.Generic;

namespace CatLib.Routing
{
    /// <summary>
    /// 路由配置
    /// </summary>
    internal sealed class RouteOptions
    {
        /// <summary>
        /// 路由请求过滤链
        /// </summary>
        private IFilterChain<IRequest, IResponse> middleware;

        /// <summary>
        /// 当路由出现异常时的过滤器链
        /// </summary>
        private IFilterChain<IRequest, IResponse, Exception> onError;

        /// <summary>
        /// 筛选条件
        /// </summary>
        private Dictionary<string, string> wheres;

        /// <summary>
        /// 默认值
        /// </summary>
        private Dictionary<string, string> defaults;

        /// <summary>
        /// 当被编译的内容发生改变时
        /// </summary>
        public event Action OnCompiledChange;

        /// <summary>
        /// 获取参数默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>默认值</returns>
        public string GetDefaults(string name, string defaultValue = null)
        {
            if (defaults == null)
            {
                return defaultValue;
            }
            return !defaults.ContainsKey(name) ? defaultValue : defaults[name];
        }

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <returns>筛选条件,如果不存在则返回null</returns>
        public string GetWhere(string varName)
        {
            if (wheres == null)
            {
                return null;
            }
            return wheres.ContainsKey(varName) ? wheres[varName] : null;
        }

        /// <summary>
        /// 约束指定参数必须符合指定模式才会被路由
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="pattern">约束条件</param>
        /// <param name="overrided">是否覆盖配置</param>
        /// <returns>当前路由配置实例</returns>
        public RouteOptions Where(string name, string pattern, bool overrided = true)
        {
            if (wheres == null)
            {
                wheres = new Dictionary<string, string>();
            }
            if (wheres.ContainsKey(name))
            {
                if (!overrided)
                {
                    return this;
                }
            }
            wheres[name] = pattern;
            if (OnCompiledChange != null)
            {
                OnCompiledChange.Invoke();
            }
            return this;
        }

        /// <summary>
        /// 设定默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">默认值</param>
        /// <param name="overrided">是否覆盖配置</param>
        /// <returns>当前路由配置实例</returns>
        public RouteOptions Defaults(string name, string val, bool overrided = true)
        {
            if (defaults == null)
            {
                defaults = new Dictionary<string, string>();
            }
            if (defaults.ContainsKey(name))
            {
                if (!overrided)
                {
                    return this;
                }
            }
            defaults[name] = val;

            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="onError">错误处理函数</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前路由配置实例</returns>
        public RouteOptions OnError(Action<IRequest, IResponse, Exception, Action<IRequest, IResponse, Exception>> onError, int priority = int.MaxValue)
        {
            if (this.onError == null)
            {
                this.onError = new FilterChain<IRequest, IResponse, Exception>();
            }
            this.onError.Add(onError, priority);
            return this;
        }

        /// <summary>
        /// 路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>当前路由配置实例</returns>
        public RouteOptions Middleware(Action<IRequest, IResponse, Action<IRequest, IResponse>> middleware, int priority = int.MaxValue)
        {
            if (this.middleware == null)
            {
                this.middleware = new FilterChain<IRequest, IResponse>();
            }
            this.middleware.Add(middleware, priority);
            return this;
        }

        /// <summary>
        /// 获取路由的中间件
        /// </summary>
        /// <returns>中间件过滤器链</returns>
        public IFilterChain<IRequest, IResponse> GatherMiddleware()
        {
            return middleware;
        }

        /// <summary>
        /// 获取当出现错误时的过滤器链
        /// </summary>
        /// <returns>错误处理过滤器链</returns>
        public IFilterChain<IRequest, IResponse, Exception> GatherOnError()
        {
            return onError;
        }

        /// <summary>
        /// 将当前路由配置中的信息合并到给定的路由配置中
        /// </summary>
        /// <param name="options">路由配置</param>
        /// <returns>当前路由配置实例</returns>
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
        /// <param name="options">外部路由配置</param>
        private void MergeMiddleware(RouteOptions options)
        {
            if (middleware == null)
            {
                return;
            }
            var filters = middleware.FilterList;
            for (var i = 0; i < filters.Length; i++)
            {
                options.Middleware(filters[i]);
            }
        }

        /// <summary>
        /// 合并错误时的调度
        /// </summary>
        /// <param name="options">外部路由配置</param>
        private void MergeOnError(RouteOptions options)
        {
            if (onError == null)
            {
                return;
            }
            var filters = onError.FilterList;
            for (var i = 0; i < filters.Length; i++)
            {
                options.OnError(filters[i]);
            }
        }

        /// <summary>
        /// 合并where
        /// </summary>
        /// <param name="options">外部路由配置</param>
        private void MergeWhere(RouteOptions options)
        {
            if (wheres == null)
            {
                return;
            }
            foreach (var kv in wheres)
            {
                options.Where(kv.Key, kv.Value, false);
            }
        }

        /// <summary>
        /// 合并默认值
        /// </summary>
        /// <param name="options">外部路由配置</param>
        private void MergeDefaults(RouteOptions options)
        {
            if (defaults == null)
            {
                return;
            }
            foreach (var kv in defaults)
            {
                options.Defaults(kv.Key, kv.Value, false);
            }
        }
    }
}