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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using CatLib.API;
using CatLib.API.Stl;
using CatLib.Stl;
using UnityEngine;

namespace CatLib.Core
{
    /// <summary>
    /// CatLib程序
    /// </summary>
    public sealed class Application : Driver, IApplication
    {
        /// <summary>
        /// CatLib框架版本
        /// </summary>
        private const string VERSION = "1.0.0";

        /// <summary>
        /// CatLib版本号
        /// </summary>
        public string Version
        {
            get { return VERSION; }
        }

        /// <summary>
        /// 框架启动流程
        /// </summary>
        public enum StartProcess
        {
            /// <summary>
            /// 引导流程
            /// </summary>
            OnBootstrap = 1,

            /// <summary>
            /// 初始化流程
            /// </summary>
            OnInit = 2,

            /// <summary>
            /// 服务提供商启动流程
            /// </summary>
            OnProviderProcess = 3,

            /// <summary>
            /// 启动完成
            /// </summary>
            OnComplete = 4,
        }

        /// <summary>
        /// 服务提供商
        /// </summary>
        private readonly Dictionary<Type, ServiceProvider> serviceProviders = new Dictionary<Type, ServiceProvider>();

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
        private StartProcess process = StartProcess.OnBootstrap;

        /// <summary>
        /// 启动流程
        /// </summary>
        public StartProcess Process
        {
            get { return process; }
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
        /// <exception cref="RuntimeException">当引导类型没有实现<see cref="IBootstrap"/>时引发</exception>
        public IApplication Bootstrap(params Type[] bootstraps)
        {
            Guard.Requires<ArgumentNullException>(bootstraps != null);

            if (bootstrapped)
            {
                return this;
            }

            process = StartProcess.OnBootstrap;

            App.Instance = this;

            Instance(typeof(Application).ToString(), this);
            Alias(typeof(IApplication).ToString(), typeof(Application).ToString());
            Alias(typeof(App).ToString(), typeof(Application).ToString());
            Alias(typeof(IContainer).ToString(), typeof(Application).ToString());

            foreach (var t in bootstraps)
            {
                if (!typeof(IBootstrap).IsAssignableFrom(t))
                {
                    throw new RuntimeException("Type [" + t + "] is not implements IBootstrap.");
                }
                var bootstrap = Make(t.ToString()) as IBootstrap;
                if (bootstrap == null)
                {
                    throw new RuntimeException("You need call OnFindType() To get the type of cross-assembly");
                }
                bootstrap.Bootstrap();
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
            if (inited)
            {
                return;
            }
            if (!bootstrapped)
            {
                throw new RuntimeException("Must call Bootstrap() first.");
            }

            var providers = new List<ServiceProvider>(serviceProviders.Values);

            process = StartProcess.OnInit;

            TriggerGlobal(ApplicationEvents.OnIniting, this).Trigger();

            foreach (var serviceProvider in providers)
            {
                serviceProvider.Init();
            }

            inited = true;

            TriggerGlobal(ApplicationEvents.OnInited, this).Trigger();

            StartCoroutine(StartProviderPorcess());
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="t">注册类型</param>
        /// <exception cref="RuntimeException">服务提供商被重复注册或者服务提供商没有继承自<see cref="ServiceProvider"/></exception>
        public void Register(Type t)
        {
            Guard.Requires<ArgumentNullException>(t != null);
            if (serviceProviders.ContainsKey(t))
            {
                throw new RuntimeException("Provider [" + t + "] is already register.");
            }

            if (!typeof(ServiceProvider).IsAssignableFrom(t))
            {
                throw new RuntimeException("Type [" + t + "] is not inherit ServiceProvider.");
            }

            var serviceProvider = Make(t.ToString()) as ServiceProvider;
            serviceProvider.Register();
            serviceProviders.Add(t, serviceProvider);
            if (inited)
            {
                serviceProvider.Init();
                StartCoroutine(serviceProvider.OnProviderProcess());
            }
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
        /// 启动服务提供商启动流程
        /// </summary>
        /// <returns>迭代器</returns>
        private IEnumerator StartProviderPorcess()
        {
            process = StartProcess.OnProviderProcess;

            TriggerGlobal(ApplicationEvents.OnProviderProcessing, this).Trigger();

            var providers = new List<ServiceProvider>(serviceProviders.Values);
            providers.Sort((left, right) =>
            {
                var leftPriorities = GetPriorities(left.GetType(), "OnProviderProcess");
                var rightPriorities = GetPriorities(right.GetType(), "OnProviderProcess");
                return leftPriorities.CompareTo(rightPriorities);
            });

            foreach (var provider in providers)
            {
                yield return provider.OnProviderProcess();
            }

            TriggerGlobal(ApplicationEvents.OnProviderProcessed, this).Trigger();

            process = StartProcess.OnComplete;

            TriggerGlobal(ApplicationEvents.OnApplicationStartComplete, this).Trigger();
        }
    }
}