using System;
using CatLib.Event;
using CatLib.Lua;
using CatLib.Network;
using CatLib.NetPackage;
using CatLib.AutoUpdate;
using CatLib.Resources;
using CatLib.Buffer;
using CatLib.Thread;
using CatLib.Time;
using CatLib.IO;
using CatLib.Secret;

public class Providers{

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
                typeof(IOProvider),
                typeof(BufferProvider),
                typeof(ThreadProvider),
                typeof(TimeProvider),
                typeof(SecretProvider),


                //以下是测试的提供商框架本体并不会带有这些提供商
                typeof(Bootstrap),
                typeof(AssetDecryptedProvider),

            };
        }
    }
}
