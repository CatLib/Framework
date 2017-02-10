using UnityEngine;
using System.Collections;
using CatLib.Base;
using System;
using CatLib;
using CatLib.Contracts.Base;
using CatLib.Base;

public class CProgram : MonoBehaviour {

    /// <summary>
    /// 初始化程序
    /// </summary>
    public void Awake()
    {
        IApplication application = gameObject.AddComponent<CApplication>();
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
