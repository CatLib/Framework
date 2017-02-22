using CatLib.API.IO;
using CatLib.API.ResourcesSystem;
using System;

namespace CatLib.ResourcesSystem
{

    /// <summary>
    /// 资源服务提供商
    /// </summary>
    public class ResourcesProvider : ServiceProvider
    {

        public override Type[] ProviderDepend { get { return new Type[] { typeof(IIO) }; } }

        public override void Register()
        {
            App.Singleton<Resources>().Alias<IResources>();
        }

    }

}