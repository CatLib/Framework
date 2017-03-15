using CatLib.API.IO;
using CatLib.API.Resources;
using System;
using CatLib.API.Config;

namespace CatLib.Resources
{

    /// <summary>
    /// 资源服务提供商
    /// </summary>
    public class ResourcesProvider : ServiceProvider
    {

        public override Type[] ProviderDepend { get { return new Type[] { typeof(IIOFactory) }; } }

        public override void Register()
        {
            App.Singleton<ResourcesHosted>().Alias<IResourcesHosted>();
            App.Singleton<AssetBundleLoader>().Alias<IAssetBundle>();
            App.Singleton<Resources>().Alias<IResources>().Resolving((app , bind, obj)=>{

                IConfigStore config = app.Make<IConfigStore>();
                Resources resources = obj as Resources;

                string service = config.Get<string>(typeof(Resources) , "hosted" , typeof(IResourcesHosted).ToString());
                if(!string.IsNullOrEmpty(service)){

                    resources.SetHosted(service);

                }

                return obj;

            });
        }

    }

}