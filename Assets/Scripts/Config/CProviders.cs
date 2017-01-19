using UnityEngine;
using System.Collections;
using System;
using CatLib.UpdateSystem;
using CatLib.ResourcesSystem;
using App;
using CatLib.Event;
using CatLib.Lua;

public class CProviders{

    /// <summary>
    /// 服务提供者
    /// </summary>
	public static Type[] ServiceProviders
    {
        get
        {
            return new Type[] {

                typeof(CAutoUpdateProvider),
                typeof(CResourcesProvider),
                typeof(CDispatcherProvider),
                typeof(CLuaProvider),
                typeof(CBootstrapProvider),

            };
        }
    }
}
