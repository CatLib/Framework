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
    public sealed class AutoUpdateProvider : ServiceProvider
    {
        /// <summary>
        /// 自动更新流程
        /// </summary>
        public override ProviderProcess ProviderProcess
        {
            get { return ProviderProcess.ResourcesAutoUpdate; }
        }

        /// <summary>
        /// 当触发服务提供商执行进程
        /// </summary>
        /// <returns></returns>
        public override IEnumerator OnProviderProcess()
        {
            yield return App.Make<AutoUpdate>().UpdateAsset();
        }

        /// <summary>
        /// 当服务提供商注册时
        /// </summary>
        public override void Register()
        {
            App.Singleton<AutoUpdate>().Alias<IAutoUpdate>().OnResolving((obj) =>
            {
                var config = App.Make<IConfigStore>();
                var autoupdate = obj as AutoUpdate;

                autoupdate.SetUpdateAPI(config.Get(typeof(AutoUpdate), "update.api", null));
                autoupdate.SetUpdateURL(config.Get(typeof(AutoUpdate), "update.url", null));

                return obj;
            });
        }
    }
}