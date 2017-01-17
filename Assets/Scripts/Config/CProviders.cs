using UnityEngine;
using System.Collections;
using System;
using CatLib.UpdateSystem;

public class CProviders{

    /// <summary>
    /// 服务提供者
    /// </summary>
	public static Type[] ServiceProviders
    {
        get
        {
            return new Type[] {

                typeof(CAutoUpdateProvider)

            };
        }
    }
}
