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

using CatLib.API.LocalSetting;

namespace CatLib.LocalSetting
{
    /// <summary>
    /// 本地配置服务提供商
    /// </summary>
    public sealed class LocalSettingProvider : ServiceProvider
    {
        /// <summary>
        /// 当注册本地配置服务时
        /// </summary>
        public override void Register()
        {
            App.Singleton<LocalSetting>().Alias<ILocalSetting>().Alias("local-setting").OnResolving((obj) =>
            {
                (obj as LocalSetting).SetSettingStore(new UnitySetting());
                return obj;
            });
        }
    }
}