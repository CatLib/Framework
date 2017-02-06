
using CapLib.Base;
using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using XLua;

namespace CapLib.Base
{

    [LuaCallCSharp]
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
        /// 事件
        /// </summary>
        public class Events
        {
            public static readonly string ON_INITING_CALLBACK = "application.initing.callback";

            public static readonly string ON_INITED_CALLBACK = "application.inited.callback";
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

            base.Event.Trigger(Events.ON_INITING_CALLBACK);

            foreach (CServiceProvider serviceProvider in serviceProviders.ToArray())
            {
                serviceProvider.Init();
            }

            this.inited = true;

            base.Event.Trigger(Events.ON_INITED_CALLBACK);
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

    }

}