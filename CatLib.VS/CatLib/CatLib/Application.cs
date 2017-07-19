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
using CatLib.API.Stl;
using CatLib.Stl;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// CatLib程序
    /// </summary>
    public sealed class Application : Driver, IApplication
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
        /// 当初始化完成时
        /// </summary>
        private Action onInited;

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
        /// 构建一个CatLib实例
        /// </summary>
        public Application()
        {
        }

        /// <summary>
        /// 构建一个CatLib实例
        /// </summary>
        /// <param name="behaviour">驱动脚本</param>
        [ExcludeFromCodeCoverage]
        public Application(Component behaviour)
            : base(behaviour)
        {
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
            App.Instance = this;

            RegisterCoreAlias();

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
        /// <param name="callback">初始化完成后的回调</param>
        /// <exception cref="RuntimeException">没有调用<c>Bootstrap(...)</c>就尝试初始化时触发</exception>
        public void Init(Action callback = null)
        {
            if (!bootstrapped)
            {
                throw new RuntimeException("Must call Bootstrap() first.");
            }
            if (process != StartProcess.Bootstrap)
            {
                throw new RuntimeException("StartProcess is not Bootstrap.");
            }

            onInited = callback;
            inited = true;
            process = StartProcess.Initing;
            StartCoroutine(InitPorcess());
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

        /// <summary>
        /// 启动服务提供者启动流程
        /// </summary>
        /// <returns>迭代器</returns>
        private IEnumerator InitPorcess()
        {
            foreach (var provider in serviceProviders)
            {
                yield return provider.Init();
            }

            process = StartProcess.Inited;

            Trigger(ApplicationEvents.OnStartComplete, this);

            if (onInited != null)
            {
                onInited.Invoke();
            }
        }
    }
}