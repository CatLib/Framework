using CatLib.API.IO;
using CatLib.API.AutoUpdate;
using System;
using System.Collections;
using CatLib.API.Config;

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 自动更新服务提供商
    /// </summary>
    public class AutoUpdateProvider : ServiceProvider
    {

        public override Type[] ProviderDepend { get { return new Type[] { typeof(IIOFactory) }; } }

        public override ProviderProcess ProviderProcess
        {
            get
            {
                return ProviderProcess.ResourcesAutoUpdate;
            }
        }

        public override IEnumerator OnProviderProcess()
        {
            yield return App.Make<IAutoUpdate>().UpdateAsset();
        }

        public override void Register()
        {
            App.Singleton<AutoUpdate>().Alias<IAutoUpdate>().Resolving((app , bind , obj)=>{
                
                IConfigStore config = app.Make<IConfigStore>();
                AutoUpdate autoupdate = obj as AutoUpdate;

                autoupdate.SetUpdateAPI(config.Get(typeof(AutoUpdate) , "update.api" , null));
                autoupdate.SetUpdateURL(config.Get(typeof(AutoUpdate) , "update.url" , null));

                return obj;

            });
        }

    }

}