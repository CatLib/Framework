using UnityEngine;
using System;
using CatLib;
using CatLib.API;

public class Program : MonoBehaviour {

    /// <summary>
    /// 初始化程序
    /// </summary>
    public void Awake()
    {
        IApplication application = gameObject.AddComponent<CatLib.Application>();
        application.Bootstrap(CatLib.Bootstrap.BootStrap).Init();
    }

}
