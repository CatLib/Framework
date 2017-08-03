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

using CatLib.API.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CatLib
{
    /// <summary>
    /// CatLib程序
    /// </summary>
    public class Application : Container, IApplication
    {
        /// <summary>
        /// 版本号
        /// </summary>
        private readonly Version version = new Version("1.0.0");

        /// <summary>
        /// 框架启动流程
        /// </summary>
        public enum StartProcess
        {
            /// <summary>
            /// 构建阶段
            /// </summary>
            Construct = 0,

            /// <summary>
            /// 引导流程
            /// </summary>
            Bootstrap = 1,

            /// <summary>
            /// 引导流程结束
            /// </summary>
            Bootstraped = 2,

            /// <summary>
            /// 初始化中
            /// </summary>
            Initing = 3,

            /// <summary>
            /// 初始化完成
            /// </summary>
            Inited = 4,
        }

        /// <summary>
        /// 服务提供者
        /// </summary>
        private readonly SortSet<IServiceProvider, int> serviceProviders = new SortSet<IServiceProvider, int>();

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        private readonly HashSet<Type> serviceProviderTypes = new HashSet<Type>();

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
        private StartProcess process = StartProcess.Construct;

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
        /// 增量Id
        /// </summary>
        private long incrementId;

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
        [ExcludeFromCodeCoverage]
        public Application()
        {
            App.Handler = this;
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
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

            process = StartProcess.Bootstraped;
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
                throw new RuntimeException("You must call Bootstrap() first.");
            }

            if (inited)
            {
                return;
            }

            process = StartProcess.Initing;

            foreach (var provider in serviceProviders)
            {
                provider.Init();
            }

            inited = true;
            process = StartProcess.Inited;

            Trigger(ApplicationEvents.OnStartCompleted, this);
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="provider">注册服务提供者</param>
        /// <exception cref="RuntimeException">服务提供者被重复注册时触发</exception>
        public void Register(IServiceProvider provider)
        {
            Guard.Requires<ArgumentNullException>(provider != null);

            if (IsRegisted(provider))
            {
                throw new RuntimeException("Provider [" + provider.GetType() + "] is already register.");
            }

            provider.Register();
            serviceProviders.Add(provider, GetPriority(provider.GetType(), "Init"));
            serviceProviderTypes.Add(GetProviderBaseType(provider));

            if (inited)
            {
                provider.Init();
            }
        }

        /// <summary>
        /// 服务提供者是否已经注册过
        /// </summary>
        /// <param name="provider">服务提供者</param>
        /// <returns>服务提供者是否已经注册过</returns>
        public bool IsRegisted(IServiceProvider provider)
        {
            return serviceProviderTypes.Contains(GetProviderBaseType(provider));
        }

        /// <summary>
        /// 获取运行时唯一Id
        /// </summary>
        /// <returns>应用程序内唯一id</returns>
        public long GetRuntimeId()
        {
            return Interlocked.Increment(ref incrementId);
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">识别的类型</param>
        /// <param name="method">识别的方法</param>
        /// <returns>优先级</returns>
        public int GetPriority(Type type, string method = null)
        {
            return Util.GetPriority(type, method);
        }

        /// <summary>
        /// 触发一个事件,并获取事件的返回结果
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="payload">载荷</param>
        /// <returns>事件结果</returns>
        public object[] Trigger(string eventName, object payload = null)
        {
            GuardDispatcher();
            return Dispatcher.Trigger(eventName, payload);
        }

        /// <summary>
        /// 触发一个事件,遇到第一个事件存在处理结果后终止,并获取事件的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="payload">载荷</param>
        /// <returns>事件结果</returns>
        public object TriggerHalt(string eventName, object payload = null)
        {
            GuardDispatcher();
            return Dispatcher.TriggerHalt(eventName, payload);
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
            GuardDispatcher();
            return Dispatcher.On(eventName, handler, life);
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
            GuardDispatcher();
            return Dispatcher.On(eventName, handler, life);
        }

        /// <summary>
        /// CatLib版本(遵循semver)
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string Version
        {
            get
            {
                return version.ToString();
            }
        }

        /// <summary>
        /// 比较CatLib版本(遵循semver)
        /// <para>输入版本大于当前版本则返回<code>-1</code></para>
        /// <para>输入版本等于当前版本则返回<code>0</code></para>
        /// <para>输入版本小于当前版本则返回<code>1</code></para>
        /// </summary>
        /// <param name="major">主版本号</param>
        /// <param name="minor">次版本号</param>
        /// <param name="revised">修订版本号</param>
        /// <returns>比较结果</returns>
        [ExcludeFromCodeCoverage]
        public int Compare(int major, int minor, int revised)
        {
            return Compare(string.Format("{0}.{1}.{2}", major, minor, revised));
        }

        /// <summary>
        /// 比较CatLib版本(遵循semver)
        /// <para>输入版本大于当前版本则返回<code>-1</code></para>
        /// <para>输入版本等于当前版本则返回<code>0</code></para>
        /// <para>输入版本小于当前版本则返回<code>1</code></para>
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>比较结果</returns>
        [ExcludeFromCodeCoverage]
        public int Compare(string version)
        {
            return this.version.Compare(version);
        }

        /// <summary>
        /// 获取服务提供者基础类型
        /// </summary>
        /// <param name="provider">服务提供者</param>
        /// <returns>基础类型</returns>
        private Type GetProviderBaseType(IServiceProvider provider)
        {
            var providerType = provider as IServiceProviderType;
            return providerType == null ? provider.GetType() : providerType.BaseType;
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
                typeof(IApplication),
                typeof(App),
                typeof(IContainer)
            })
            {
                Alias(Type2Service(type), application);
            }
        }

        /// <summary>
        /// 验证调度器是否有效
        /// </summary>
        private void GuardDispatcher()
        {
            if (Dispatcher == null)
            {
                throw new RuntimeException("You need register EventsProvider to supported dispatcher");
            }
        }
    }
}