using System.Collections;
using CatLib.Contracts.Lua;

namespace CatLib.Lua
{

    public class LuaProvider : ServiceProvider
    {

        public override ProviderProcess ProviderProcess
        {
            get
            {
                return ProviderProcess.CODE_AUTO_LOAD;
            }
        }

        public override IEnumerator OnProviderProcess()
        {
            yield return (App.Make<ILua>() as LuaStore).LoadHotFix();
        }


        public override void Register()
        {
            App.Singleton<LuaStore>().Alias<ILua>();
        }
    }
}
