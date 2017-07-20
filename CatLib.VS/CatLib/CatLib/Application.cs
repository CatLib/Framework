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

using CatLib.API;
using CatLib.API.Events;
using CatLib.API.Support;
using CatLib.Support;
using System;
using System.Threading;
using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// CatLib程序
    /// </summary>
    public class Application : Container, IApplication
    {
        /// <summary>
        /// CatLib版本号
        /// </summary>
        public string Version
        {
            get
            {
                return "1.0.0";
            }
        }

        /// <summary>
        /// 框架启动流程
        /// </summary>
        public enum StartProcess
        {
            /// <summary>
            /// 引导流程
            /// </summary>
            Bootstrap = 1,

            /// <summary>
            /// 初始化中
            /// </summary>
            Initing = 2,

            /// <summary>
            /// 初始化完成
            /// </summary>
            Inited = 4,
        }

        /// <summary>
        /// 服务提供者
        /// </summary>
        private readonly SortSet<API.IServiceProvider, int> serviceProviders = new SortSet<API.IServiceProvider , int>();

        /// <summary>
        /// 是否已经完成引导程序
        /// </summary>
        private bool bootstrapped;

        /// <summary>
        /// 是否已经完成初始化
        /// </summary>
        private bool inited;

        /// <summary>
        /// 启动流程
        /// </summary>
        private StartProcess process = StartProcess.Bootstrap;

        /// <summary>
        /// 启动流程
        /// </summary>
        public StartProcess Process
        {
            get
            {
                return process;
            }
        }

        /// <summary>
        /// 全局唯一自增
        /// </summary>
        private long guid;

        /// <summary>
        /// 主线程ID
        /// </summary>
        private readonly int mainThreadId;

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public bool IsMainThread
        {
            get
            {
                return mainThreadId == Thread.CurrentThread.ManagedThreadId;
            }
        }

        /// <summary>
        /// 事件系统
        /// </summary>
        private IDispatcher dispatcher;

        /// <summary>
        /// 事件系统
        /// </summary>
        private IDispatcher Dispatcher
        {
            get
            {
                return dispatcher ?? (dispatcher = this.Make<IDispatcher>());
            }
        }

        /// <summary>
        /// 构建一个CatLib实例
        /// </summary>
        /// <param name="baseComponent">基础组件</param>
        [ExcludeFromCodeCoverage]
        public Application(Component baseComponent = null)
        {
            App.Instance = this;
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            Instance(Type2Service(typeof(Component)), baseComponent);
            RegisterCoreAlias();
        }

        /// <summary>
        /// 引导程序
        /// </summary>
        /// <param name="bootstraps">引导程序</param>
        /// <returns>CatLib实例</returns>
        /// <exception cref="ArgumentNullException">当引导类型为null时引发</exception>
        public IApplication Bootstrap(params IBootstrap[] bootstraps)
        {
            Guard.Requires<ArgumentNullException>(bootstraps != null);

            if (bootstrapped)
            {
                return this;
            }

            process = StartProcess.Bootstrap;

            foreach (var bootstrap in bootstraps)
            {
                if (bootstrap != null)
                {
                    bootstrap.Bootstrap();
                }
            }

            bootstrapped = true;

            return this;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <exception cref="RuntimeException">没有调用<c>Bootstrap(...)</c>就尝试初始化时触发</exception>
        public void Init()
        {
            if (!bootstrapped)
            {
                throw new RuntimeException("Must call Bootstrap() first.");
            }
            if (process != StartProcess.Bootstrap)
            {
                throw new RuntimeException("StartProcess is not Bootstrap.");
            }
            
            process = StartProcess.Initing;

            foreach (var provider in serviceProviders)
            {
                provider.Init();
            }

            inited = true;
            process = StartProcess.Inited;

            Trigger(ApplicationEvents.OnStartComplete, this);
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="provider">注册服务提供者</param>
        /// <exception cref="RuntimeException">服务提供者被重复注册或者服务提供者没有继承自<see cref="ServiceProvider"/></exception>
        public void Register(API.IServiceProvider provider)
        {
            Guard.Requires<ArgumentNullException>(provider != null);

            if (inited)
            {
                throw new RuntimeException("Register() Only be called before Init()");
            }

            if (serviceProviders.Contains(provider))
            {
                throw new RuntimeException("Provider [" + provider + "] is already register.");
            }

            provider.Register();
            serviceProviders.Add(provider, GetPriorities(provider.GetType(), "Init"));
        }

        /// <summary>
        /// 获取一个唯一id
        /// </summary>
        /// <returns>应用程序内唯一id</returns>
        public long GetGuid()
        {
            return Interlocked.Increment(ref guid);
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">识别的类型</param>
        /// <param name="method">识别的方法</param>
        /// <returns>优先级</returns>
        public int GetPriorities(Type type, string method = null)
        {
            return Util.GetPriorities(type, method);
        }

        /// <summary>
        /// 触发一个事件,并获取事件的返回结果
        /// <para>如果<paramref name="halt"/>为<c>true</c>那么返回的结果是事件的返回结果,没有一个事件进行处理的话返回<c>null</c>
        /// 反之返回一个事件处理结果数组(<c>object[]</c>)</para>
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="payload">载荷</param>
        /// <param name="halt">是否只触发一次就终止</param>
        /// <returns>事件结果</returns>
        public object Trigger(string eventName, object payload = null, bool halt = false)
        {
            if (Dispatcher != null)
            {
                return Dispatcher.Trigger(eventName, payload, halt);
            }
            return halt ? null : new object[] { };
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, Action<object> handler, int life = 0)
        {
            return Dispatcher != null ? Dispatcher.On(eventName, handler, life) : null;
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, Func<object, object> handler, int life = 0)
        {
            return Dispatcher != null ? Dispatcher.On(eventName, handler, life) : null;
        }

        /// <summary>
        /// 注册核心别名
        /// </summary>
        private void RegisterCoreAlias()
        {
            var application = Type2Service(typeof(Application));
            Instance(application, this);
            foreach (var type in new[]
            {
                typeof(IApplication) ,
                typeof(App) ,
                typeof(IContainer)
            })
            {
                Alias(Type2Service(type), application);
            }
        }
    }
}