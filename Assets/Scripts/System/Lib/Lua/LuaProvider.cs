using System.Collections;
using CatLib.Contracts.Lua;
using CatLib.Contracts.IO;
using System;

namespace CatLib.Lua
{

    public class LuaProvider : ServiceProvider
    {

        public override Type[] ProviderDepend { get { return new Type[] { typeof(IDirectory) }; } }

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
