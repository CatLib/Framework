/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.Text;
using CatLib.API;
using CatLib.API.Lua;
using CatLib.API.IO;

namespace CatLib.Demo.Lua
{

    public class LuaDemo : ServiceProvider
    {
        
        public override void Init()
        {
            Env env = App[typeof(Env).ToString()] as Env;
//            env.SetDebugLevel(DebugLevels.Prod);

            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
                {
                    string scriptCode = "CS.UnityEngine.Debug.Log('hello world')";

                    UnityEngine.Debug.Log("Lua code : " + scriptCode);

                    ILua luaEngine = App.Make<ILua>();
                    luaEngine.DoString("require 'HelloworldLua' ");
//                    luaEngine.ExecuteScriptByFile("text/lua","good.lua");
                });
        }

        public override void Register() { }
    }
}