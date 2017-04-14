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
using System.Collections.Generic;
using CatLib.API.Container;

namespace CatLib.Container
{
    /// <summary>
    /// 服务绑定数据
    /// </summary>
    public sealed class BindData : IBindData
    {
        /// <summary>
        /// 服务名
        /// </summary>
        public string Service { get; private set; }

        /// <summary>
        /// 服务实现
        /// </summary>
        public Func<IContainer, object[], object> Concrete { get; private set; }

        /// <summary>
        /// 是否是静态服务
        /// </summary>
        public bool IsStatic { get; private set; }

        /// <summary>
        /// 服务关系上下文
        /// </summary>
        private Dictionary<string, string> contextual;

        /// <summary>
        /// 拦截器
        /// </summary>
        private List<IInterception> interception;

        /// <summary>
        /// 容器
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// 服务修饰器
        /// </summary>
        private List<Func<object, object>> decorator;

        /// <summary>
        /// 构建一个绑定数据
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        public BindData(IContainer container, string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            this.container = container;
            Service = service;
            Concrete = concrete;
            IsStatic = isStatic;
        }

        /// <summary>
        /// 当需求某个服务                                                                                                                                                                                                                                                                                                                                                                                  
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>绑定关系临时数据</returns>
        public IGivenData Needs(string service)
        {
            return new GivenData(this, service);
        }

        /// <summary>
        /// 当需求某个服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>绑定关系临时数据</returns>
        public IGivenData Needs<T>()
        {
            return Needs(typeof(T).ToString());
        }

        /// <summary>
        /// 拦截器
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务绑定数据</returns>
        public IBindData AddInterceptor<T>() where T : IInterception, new()
        {
            if (interception == null)
            {
                interception = new List<IInterception>();
            }
            interception.Add(new T());
            return this;
        }

        /// <summary>
        /// 获取服务的拦截器
        /// </summary>
        /// <returns>当前服务的拦截器</returns>
        public IInterception[] GetInterceptors()
        {
            return interception == null ? null : interception.ToArray();
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="T">别名</typeparam>
        /// <returns>服务绑定数据</returns>
        public IBindData Alias<T>()
        {
            return Alias(typeof(T).ToString());
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns>服务绑定数据</returns>
        public IBindData Alias(string alias)
        {
            container.Alias(alias, Service);
            return this;
        }

        /// <summary>
        /// 获取上下文的需求关系
        /// </summary>
        /// <param name="needs">需求的服务</param>
        /// <returns>给与的服务</returns>
        public string GetContextual(string needs)
        {
            if (contextual == null)
            {
                return needs;
            }
            return contextual.ContainsKey(needs) ? contextual[needs] : needs;
        }

        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="func">解决事件</param>
        public IBindData OnResolving(Func<object, object> func)
        {
            if (decorator == null)
            {
                decorator = new List<Func<object, object>>();
            }
            decorator.Add(func);
            return this;
        }

        /// <summary>
        /// 执行服务修饰器
        /// </summary>
        /// <param name="obj">服务实例</param>
        /// <returns>修饰后的服务实例</returns>
        public object ExecDecorator(object obj)
        {
            if (decorator == null)
            {
                return obj;
            }
            foreach (var func in decorator)
            {
                obj = func(obj);
            }
            return obj;
        }

        /// <summary>
        /// 为服务增加上下文
        /// </summary>
        /// <param name="needs">需求什么服务</param>
        /// <param name="given">给与什么服务</param>
        /// <returns>服务绑定数据</returns>
        public BindData AddContextual(string needs, string given)
        {
            if (contextual == null)
            {
                contextual = new Dictionary<string, string>();
            }
            contextual.Add(needs, given);
            return this;
        }
    }
}