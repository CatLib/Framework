using System.Collections;
using CatLib.API.Lua;
using CatLib.API.IO;
using System;
using CatLib.API.Config;

namespace CatLib.Lua
{

    public class LuaProvider : ServiceProvider
    {

        public override Type[] ProviderDepend { get { return new Type[] { typeof(IIOFactory) }; } }

        public override ProviderProcess ProviderProcess
        {
            get
            {
                return ProviderProcess.CodeAutoLoad;
            }
        }

        public override IEnumerator OnProviderProcess()
        {
            yield return (App.Make<ILua>() as LuaStore).LoadHotFix();
        }


        public override void Register()
        {
            App.Singleton<LuaStore>().Alias<ILua>().Resolving((app , bind , obj)=>{

                IConfigStore config = app.Make<IConfigStore>();
                LuaStore store = obj as LuaStore;

                store.SetHotfixPath(config.Get<string[]>(typeof(LuaStore) , "lua.hotfix.path" , null));

                return obj;

            });
        }
    }
}
