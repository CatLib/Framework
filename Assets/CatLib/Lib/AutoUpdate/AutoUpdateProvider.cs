/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using CatLib.API.AutoUpdate;
using System.Collections;
using CatLib.API.Config;

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 自动更新服务提供商
    /// </summary>
    public class AutoUpdateProvider : ServiceProvider
    {

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