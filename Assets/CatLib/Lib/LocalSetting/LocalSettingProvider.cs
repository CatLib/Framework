
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