using System;
using CatLib.Event;
using CatLib.Lua;
using CatLib.Network;
using CatLib.NetPackage;
using CatLib.UpdateSystem;
using CatLib.ResourcesSystem;

public class CProviders{

    /// <summary>
    /// 服务提供者
    /// </summary>
	public static Type[] ServiceProviders
    {
        get
        {
            return new Type[] {

                typeof(NetPackageProvider),
                typeof(AutoUpdateProvider),
                typeof(ResourcesProvider),
                typeof(DispatcherProvider),
                typeof(NetworkProvider),
                typeof(LuaProvider),
                typeof(CBootstrap),

            };
        }
    }
}
