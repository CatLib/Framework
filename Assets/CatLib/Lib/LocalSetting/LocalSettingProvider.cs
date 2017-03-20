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

    public class LocalSettingProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<LocalSetting>().Alias<ILocalSetting>().Alias("local-setting").Resolving((app, bind, obj) =>
            {
                (obj as LocalSetting).SetSettingStore(new UnitySetting());
                return obj;
            });
        }

    }

}