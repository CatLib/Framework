
using CatLib.Base;
using CatLib.Container;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CatLib程序
/// </summary>
public class CApplication : CContainer {

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
    protected Dictionary<Type , CServiceProvider> serviceProviders = new Dictionary<Type , CServiceProvider>();
    
    /// <summary>
    /// 更新
    /// </summary>
    protected List<IUpdate> update = new List<IUpdate>();

    /// <summary>
    /// 延后更新
    /// </summary>
    protected List<ILateUpdate> lateUpdate = new List<ILateUpdate>();

    /// <summary>
    /// 是否已经完成引导程序
    /// </summary>
    protected bool bootstrapped = false;

    /// <summary>
    /// 是否已经完成初始化
    /// </summary>
    protected bool inited = false;

    protected static CApplication instance;

    public static CApplication Instance
    {
        get
        {
            if (instance == null)
            {
                throw new CNullReferenceException("application not instance");
            }
            return instance;
        }
    }

    public CApplication()
    {
        Decorator((container , bindData, obj) =>
        {
            
            if (bindData.IsStatic)
            {
                if (obj is IUpdate)
                {
                    update.Add(obj as IUpdate);
                }

                if(obj is ILateUpdate)
                {
                    lateUpdate.Add(obj as ILateUpdate);
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
    public CApplication Bootstrap(Type[] bootstraps)
    {
        instance = this;
        Instances(typeof(CApplication).ToString() , this);

        IBootstrap bootstrap;
        foreach (Type t in bootstraps)
        {
            bootstrap = this.Make<IBootstrap>(t);
            if(bootstrap != null)
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

        foreach(CServiceProvider serviceProvider in serviceProviders.ToArray())
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
        if(serviceProvider != null)
        {
            serviceProvider.Register();
            this.serviceProviders.Add(t, serviceProvider);
            if (this.inited) { serviceProvider.Init(); }
        }

    }

    public void Update()
    {
        for(int i = 0; i < update.Count; i++)
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

}
