using UnityEngine;
using System.Collections;
using CatLib.Event;

/// <summary>
/// 服务提供者
/// </summary>
public abstract class CServiceProvider : IEvent{

    protected CApplication application;

    public CServiceProvider(CApplication app)
    {
        application = app;
    }

    public virtual void Init() { }

    abstract public void Register();

    private CEvent cevent = null;
    /// <summary>
    /// 事件系统
    /// </summary>
    public CEvent Event
    {
        get
        {
            if (this.cevent == null) { this.cevent = new CEvent(); }
            return this.cevent;
        }
    }

}
