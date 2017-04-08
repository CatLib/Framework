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
 
using CatLib.API.Resources;
using CatLib.API.Config;

namespace CatLib.Resources
{

    /// <summary>
    /// 资源服务提供商
    /// </summary>
    public class ResourcesProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<AssetBundleLoader>().Alias<IAssetBundle>();
            App.Singleton<Resources>().Alias<IResources>().OnResolving((app , bind, obj)=>{

                IConfigStore config = app.Make<IConfigStore>();

                if (config != null)
                {
                    Resources resources = obj as Resources;
                    bool useHosted = config.Get(typeof(Resources), "hosted", true);
                    if (useHosted)
                    {
                        resources.SetHosted(new ResourcesHosted());
                    }
                }

                return obj;

            });
        }

    }

}