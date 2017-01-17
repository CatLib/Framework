using UnityEngine;
using System.Collections;

/// <summary>
/// 服务提供者
/// </summary>
public abstract class CServiceProvider{

    protected CApplication application;

    public CServiceProvider(CApplication app)
    {
        application = app;
    }

    public virtual void Init() { }

    abstract public void Register();

}
