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
using CatLib.API.Time;

namespace CatLib
{

    /// <summary>
    /// CatLib程序
    /// </summary>
    public class Application : Driver, IApplication
    {

        /// <summary>
        /// CatLib框架版本
        /// </summary>
        public const string VERSION = "1.0.0";

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
        protected Dictionary<Type, ServiceProvider> serviceProviders = new Dictionary<Type, ServiceProvider>();

        /// <summary>
        /// 是否已经完成引导程序
        /// </summary>
        protected bool bootstrapped = false;

        /// <summary>
        /// 是否已经完成初始化
        /// </summary>
        protected bool inited = false;

        /// <summary>
        /// 启动流程
        /// </summary>
        protected StartProcess process = StartProcess.OnBootstrap;

        /// <summary>
        /// 全局唯一自增
        /// </summary>
        protected long guid = 0;

        /// <summary>
        /// 时间系统
        /// </summary>
        private ITime time;

        /// <summary>
        /// 时间
        /// </summary>
        public ITime Time
        {
            get
            {
                if (process <= StartProcess.OnInit)
                {
                    throw new Exception("can not call Time , because framework is not inited");
                }
                if (time == null)
                {
                    time = Make(typeof(ITime).ToString()) as ITime;
                }
                return time;
            }
        }

        /// <summary>
        /// 新建一个应用程序
        /// </summary>
        public Application() : base()
        {

        }

        /// <summary>
        /// 新建一个应用程序
        /// </summary>
        /// <param name="behaviour"></param>
        public Application(UnityEngine.MonoBehaviour behaviour) 
            : base(behaviour)
        {

        }

        /// <summary>
        /// 引导程序
        /// </summary>
        /// <param name="bootstraps">引导文件</param>
        /// <returns></returns>
        public IApplication Bootstrap(Type[] bootstraps)
        {
            process = StartProcess.OnBootstrap;

            App.Instance = this;
            Instance(typeof(Application).ToString(), this);
            Alias(typeof(IApplication).ToString(), typeof(Application).ToString());
            Alias(typeof(App).ToString(), typeof(Application).ToString());
            IBootstrap bootstrap;
            foreach (Type t in bootstraps)
            {
                bootstrap = this.Make<IBootstrap>(t);
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
        public void Init()
        {
            if (inited)
            {
                return;
            }
            if (!bootstrapped)
            {
                return;
            }

            ServiceProvider[] providers = serviceProviders.ToArray();

            process = StartProcess.OnInit;

            Trigger(this).SetEventName(ApplicationEvents.ON_INITING).Trigger();

            foreach (ServiceProvider serviceProvider in providers)
            {
                serviceProvider.Init();
            }

            inited = true;

            Trigger(this).SetEventName(ApplicationEvents.ON_INITED).Trigger();

            StartCoroutine(StartProviderPorcess());

        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="t"></param>
        public void Register(Type t)
        {
            if (serviceProviders.ContainsKey(t))
            {
                return;
            }

            ServiceProvider serviceProvider = this.Make<ServiceProvider>(t);
            if (serviceProvider != null)
            {
                serviceProvider.Register();
                this.serviceProviders.Add(t, serviceProvider);
                if (this.inited)
                {
                    serviceProvider.Init();
                }
            }

        }

        /// <summary>
        /// 获取一个唯一id
        /// </summary>
        /// <returns></returns>
        public long GetGuid()
        {
            return Interlocked.Increment(ref guid);
        }

        /// <summary>
        /// 启动服务提供商启动流程
        /// </summary>
        /// <returns></returns>
        protected IEnumerator StartProviderPorcess()
        {
            process = StartProcess.OnProviderProcess;

            Trigger(this).SetEventName(ApplicationEvents.ON_PROVIDER_PROCESSING).Trigger();

            List<ServiceProvider> providers = new List<ServiceProvider>(serviceProviders.Values);
            providers.Sort((left, right) => ((int)left.ProviderProcess).CompareTo((int)right.ProviderProcess));

            foreach (ServiceProvider provider in providers)
            {
                yield return provider.OnProviderProcess();
            }

            Trigger(this).SetEventName(ApplicationEvents.ON_PROVIDER_PROCESSED).Trigger();

            process = StartProcess.OnComplete;

            Trigger(this).SetEventName(ApplicationEvents.ON_APPLICATION_START_COMPLETE).Trigger();

        }

    }

}