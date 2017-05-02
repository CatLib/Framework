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
 
using System.Collections;
using CatLib.API;
using CatLib.API.Config;

namespace CatLib.Lua
{

    public class LuaProvider : ServiceProvider
    {
        [Priority(20)]
        public override IEnumerator OnProviderProcess()
        {
            yield return (App.Make<ILua>() as LuaStore).LoadHotFix();
        }


        public override void Register()
        {
            App.Singleton<LuaStore>().Alias<ILua>().OnResolving((bind, obj) =>{

                IConfigStore config = App.Make<IConfigStore>();
                LuaStore store = obj as LuaStore;

                store.SetHotfixPath(config.Get<string[]>(typeof(LuaStore) , "lua.hotfix.path" , null));

                return obj;

            });
        }
    }
}
