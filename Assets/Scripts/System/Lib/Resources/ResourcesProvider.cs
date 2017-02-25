using CatLib.API.IO;
using CatLib.API.Resources;
using System;

namespace CatLib.Resources
{

    /// <summary>
    /// 资源服务提供商
    /// </summary>
    public class ResourcesProvider : ServiceProvider
    {

        public override Type[] ProviderDepend { get { return new Type[] { typeof(IIO) }; } }

        public override void Register()
        {
            App.Singleton<AssetBundleLoader>().Alias<IAssetBundle>();
            App.Singleton<Resources>().Alias<IResources>();
        }

    }

}