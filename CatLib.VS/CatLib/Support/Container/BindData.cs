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

namespace CatLib
{
    /// <summary>
    /// 服务绑定数据
    /// </summary>    
    internal sealed class BindData : IBindData
    {
        /// <summary>
        /// 当前绑定服务的服务名
        /// </summary>
        public string Service { get; private set; }

        /// <summary>
        /// 服务实现，执行这个委托将会获得服务实例
        /// </summary>
        public Func<IContainer, object[], object> Concrete { get; private set; }

        /// <summary>
        /// 当前绑定的服务是否是静态服务
        /// </summary>
        public bool IsStatic { get; private set; }

        /// <summary>
        /// 服务关系上下文
        /// 当前服务需求某个服务时可以指定给与什么服务
        /// </summary>
        private Dictionary<string, string> contextual;

        /// <summary>
        /// 父级容器
        /// </summary>
        private readonly Container container;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private List<Func<IBindData, object, object>> resolving;

        /// <summary>
        /// 服务构造修饰器
        /// </summary>
        private List<Action<IBindData, object>> release;

        /// <summary>
        /// 是否被释放
        /// </summary>
        private bool isDestroy;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 给与数据
        /// </summary>
        private GivenData given;

        /// <summary>
        /// 构建一个绑定数据
        /// </summary>
        /// <param name="container">服务父级容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        internal BindData(Container container, string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            this.container = container;
            Service = service;
            Concrete = concrete;
            IsStatic = isStatic;
            isDestroy = false;
        }

        /// <summary>
        /// 当需求某个服务                                                                                                                                                                                                                                                                                                                                                                                  
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>绑定关系临时数据</returns>
        public IGivenData Needs(string service)
        {
            Guard.NotEmptyOrNull(service, "service");
            lock (syncRoot)
            {
                GuardIsDestroy();
                if (given == null)
                {
                    given = new GivenData(container, this);
                }
                given.Needs(service);
            }
            return given;
        }

        /// <summary>
        /// 当需求某个服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>绑定关系临时数据</returns>
        public IGivenData Needs<T>()
        {
            return Needs(container.Type2Service(typeof(T)));
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="T">别名</typeparam>
        /// <returns>服务绑定数据</returns>
        public IBindData Alias<T>()
        {
            return Alias(container.Type2Service(typeof(T)));
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns>服务绑定数据</returns>
        public IBindData Alias(string alias)
        {
            lock (syncRoot)
            {
                GuardIsDestroy();
                Guard.NotEmptyOrNull(alias, "alias");
                container.Alias(alias, Service);
                return this;
            }
        }

        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="func">解决事件</param>
        /// <returns>服务绑定数据</returns>
        public IBindData OnResolving(Func<IBindData, object, object> func)
        {
            Guard.NotNull(func, "func");
            lock (syncRoot)
            {
                GuardIsDestroy();
                if (resolving == null)
                {
                    resolving = new List<Func<IBindData, object, object>>();
                }
                resolving.Add(func);
            }
            return this;
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="action">处理事件</param>
        /// <returns>服务绑定数据</returns>
        public IBindData OnRelease(Action<IBindData, object> action)
        {
            Guard.NotNull(action, "action");
            if (!IsStatic)
            {
                throw new RuntimeException("Service [" + Service + "] is not Singleton(Static) Bind , Can not call OnRelease().");
            }
            lock (syncRoot)
            {
                GuardIsDestroy();
                if (release == null)
                {
                    release = new List<Action<IBindData, object>>();
                }
                release.Add(action);
            }
            return this;
        }

        /// <summary>
        /// 移除绑定服务 , 在解除绑定时如果是静态化物体将会触发释放
        /// </summary>
        public void UnBind()
        {
            lock (syncRoot)
            {
                isDestroy = true;
                container.UnBind(Service);
            }
        }

        /// <summary>
        /// 获取上下文的需求关系
        /// </summary>
        /// <param name="needs">需求的服务</param>
        /// <returns>给与的服务</returns>
        internal string GetContextual(string needs)
        {
            if (contextual == null)
            {
                return needs;
            }

            string contextualNeeds;
            return contextual.TryGetValue(needs, out contextualNeeds) ? contextualNeeds : needs;
        }

        /// <summary>
        /// 执行服务修饰器
        /// </summary>
        /// <param name="obj">服务实例</param>
        /// <returns>修饰后的服务实例</returns>
        internal object ExecResolvingDecorator(object obj)
        {
            if (resolving == null)
            {
                return obj;
            }
            foreach (var func in resolving)
            {
                obj = func.Invoke(this, obj);
            }
            return obj;
        }

        /// <summary>
        /// 执行服务释放处理器
        /// </summary>
        /// <param name="obj">服务实例</param>
        internal void ExecReleaseDecorator(object obj)
        {
            if (release == null)
            {
                return;
            }
            foreach (var action in release)
            {
                action.Invoke(this, obj);
            }
        }

        /// <summary>
        /// 为服务增加上下文
        /// </summary>
        /// <param name="needs">需求什么服务</param>
        /// <param name="given">给与什么服务</param>
        /// <returns>服务绑定数据</returns>
        internal BindData AddContextual(string needs, string given)
        {
            lock (syncRoot)
            {
                GuardIsDestroy();
                if (contextual == null)
                {
                    contextual = new Dictionary<string, string>();
                }
                if (contextual.ContainsKey(needs))
                {
                    throw new RuntimeException("Needs [" + needs + "] is already exist.");
                }
                contextual.Add(needs, given);
                return this;
            }
        }

        /// <summary>
        /// 守卫是否被释放
        /// </summary>
        private void GuardIsDestroy()
        {
            if (isDestroy)
            {
                throw new RuntimeException("Current Instance has be mark Destroy.");
            }
        }
    }
}