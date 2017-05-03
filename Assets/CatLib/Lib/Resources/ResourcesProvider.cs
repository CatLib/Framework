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
    public sealed class ResourcesProvider : ServiceProvider
    {
        /// <summary>
        /// 注册资源服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<ResourcesHosted>();
            App.Singleton<AssetBundleLoader>().Alias<IAssetBundle>();
            App.Singleton<Resources>().Alias<IResources>().OnResolving((bind, obj) =>
            {
                var resources = obj as Resources;
                var config = App.Make<IConfigStore>();

                if (config == null)
                {
                    resources.SetHosted(App.Make<ResourcesHosted>());
                    return obj;
                }

                var useHosted = config.Get(typeof(Resources), "hosted", true);
                if (useHosted)
                {
                    resources.SetHosted(App.Make<ResourcesHosted>());
                }

                return obj;
            });
        }
    }
}