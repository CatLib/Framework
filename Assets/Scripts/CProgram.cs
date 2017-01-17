using UnityEngine;
using System.Collections;
using CatLib.Base;
using System;
using CatLib;

public class CProgram : CComponentBase {

    /// <summary>
    /// 初始化程序
    /// </summary>
    public override void Awake()
    {

        base.Awake();
        CApplication application = base.GameObject.AddComponent<CApplication>();
        application.Bootstrap(BootStrap).Init();

    }

    /// <summary>
    /// 引导程序
    /// </summary>
    protected Type[] BootStrap
    {
        get
        {
            return new Type[]
            {
                typeof(CRegisterProviders)
            };
        }

    }

}
