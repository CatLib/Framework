using UnityEngine;
using System;
using CatLib;
using CatLib.Contracts;

public class Program : MonoBehaviour {

    /// <summary>
    /// 初始化程序
    /// </summary>
    public void Awake()
    {
        IApplication application = gameObject.AddComponent<CatLib.Application>();
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
                typeof(RegisterProviders)
            };
        }

    }

}
