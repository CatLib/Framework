using UnityEngine;
using System.Collections;
using System;
using CatLib.UpdateSystem;
using CatLib.ResourcesSystem;
using App;

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
                typeof(CBootstrapProvider),

            };
        }
    }
}
