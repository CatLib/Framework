
using CatLib.Container;
using CatLib.Contracts.Base;
using CatLib.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CatLib.Base
{

    /// <summary>
    /// CatLib程序
    /// </summary>
    public class CApplication : CContainer, IApplication
    {

        /// <summary>
        /// CatLib框架版本
        /// </summary>
        public const string VERSION = "0.0.1";

        /// <summary>
        /// 框架启动流程
        /// </summary>
        public enum StartProcess
        {

            /// <summary>
            /// 引导流程
            /// </summary>
            ON_BOOTSTRAP = 1,

            /// <summary>
            /// 依赖检测流程
            /// </summary>
            ON_DEPEND = 3,

            /// <summary>
            /// 初始化流程
            /// </summary>
            ON_INITED = 4,

            /// <summary>
            /// 服务提供商启动流程
            /// </summary>
            ON_PROVIDER_PROCESS = 5,

            /// <summary>
            /// 启动完成
            /// </summary>
            ON_COMPLETE = 6,

        }

        /// <summary>
        /// 服务提供商
        /// </summary>
        protected Dictionary<Type, CServiceProvider> serviceProviders = new Dictionary<Type, CServiceProvider>();

        /// <summary>
        /// 更新
        /// </summary>
        protected List<IUpdate> update = new List<IUpdate>();

        /// <summary>
        /// 延后更新
        /// </summary>
        protected List<ILateUpdate> lateUpdate = new List<ILateUpdate>();

        /// <summary>
        /// 释放时需要调用的
        /// </summary>
        protected List<IDestroy> destroy = new List<IDestroy>();

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
        protected StartProcess process = StartProcess.ON_BOOTSTRAP;

        /// <summary>
        /// 全局唯一自增
        /// </summary>
        protected long guid = 0;

        public CApplication()
        {
            Decorator((container, bindData, obj) =>
            {

                if (bindData.IsStatic)
                {
                    if (obj is IUpdate)
                    {
                        update.Add(obj as IUpdate);
                    }

                    if (obj is ILateUpdate)
                    {
                        lateUpdate.Add(obj as ILateUpdate);
                    }
                    if (obj is IDestroy)
                    {
                        destroy.Add(obj as IDestroy);
                    }
                }
                return obj;
            });
        }


        /// <summary>
        /// 引导程序
        /// </summary>
        /// <param name="bootstraps">引导文件</param>
        /// <returns></returns>
        public IApplication Bootstrap(Type[] bootstraps)
        {
            process = StartProcess.ON_BOOTSTRAP;

            CApp.Instance = this;
            Instances(typeof(CApplication).ToString(), this);
            Alias(typeof(IApplication).ToString(), typeof(CApplication).ToString());
            Alias(typeof(CApp).ToString(), typeof(CApplication).ToString());

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
        /// <param name="provider"></param>
        public void Init()
        {
            if (inited) { return; }
            if (!bootstrapped) { return; }

            CServiceProvider[] providers = serviceProviders.ToArray();

            process = StartProcess.ON_DEPEND;

            base.Event.Trigger(CApplicationEvents.ON_DEPENDING);

            foreach (CServiceProvider provider in providers)
            {

                foreach (Type type in provider.ProviderDepend)
                {

                    if (!HasDepend(type.ToString()))
                    {
                        throw new CException("service provider [" + provider.GetType().ToString() + "] depend service provider [" + type.ToString() + "]");
                    }
                }

            }

            base.Event.Trigger(CApplicationEvents.ON_DEPENDED);

            process = StartProcess.ON_INITED;

            base.Event.Trigger(CApplicationEvents.ON_INITING);

            foreach (CServiceProvider serviceProvider in providers)
            {
                serviceProvider.Init();
            }

            this.inited = true;

            base.Event.Trigger(CApplicationEvents.ON_INITED);

            StartCoroutine(StartProviderPorcess());

        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="t"></param>
        public void Register(Type t)
        {
            if (this.serviceProviders.ContainsKey(t)) { return; }

            CServiceProvider serviceProvider = this.Make<CServiceProvider>(t);
            if (serviceProvider != null)
            {
                serviceProvider.Register();
                this.serviceProviders.Add(t, serviceProvider);
                if (this.inited) { serviceProvider.Init(); }
            }

        }

        public void Update()
        {
            for (int i = 0; i < update.Count; i++)
            {
                update[i].Update();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < lateUpdate.Count; i++)
            {
                lateUpdate[i].LateUpdate();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            for (int i = 0; i < destroy.Count; i++)
            {
                destroy[i].OnDestroy();
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

            process = StartProcess.ON_PROVIDER_PROCESS;

            Event.Trigger(CApplicationEvents.ON_PROVIDER_PROCESSING);

            List<CServiceProvider> providers = new List<CServiceProvider>(serviceProviders.Values);
            providers.Sort((left, right) => ((int)left.ProviderProcess).CompareTo((int)right.ProviderProcess) );

            foreach(CServiceProvider provider in providers)
            {
                yield return provider.OnProviderProcess();
            }

            Event.Trigger(CApplicationEvents.ON_PROVIDER_PROCESSED);

            process = StartProcess.ON_COMPLETE;

            Event.Trigger(CApplicationEvents.ON_APPLICATION_START_COMPLETE);

        }

    }

}