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
 
using System.Collections;
using CatLib.API.Config;
using CatLib.API.Lua;

namespace CatLib.Lua
{
    // ===============================================================================
    // File Name           :    LuaProvider.cs
    // Class Description   :    Lua服务提供商
    // Author              :    Mingming
    // Create Time         :    2017-04-22 17:46:58
    // ===============================================================================
    // Copyright © Mingming . All rights reserved.
    // ===============================================================================
    public class LuaProvider : ServiceProvider
    {

        public override ProviderProcess ProviderProcess
        {
            get
            {
                return ProviderProcess.CodeAutoLoad;
            }
        }

        public override IEnumerator OnProviderProcess()
        {
            yield return (App.Make<ILua>() as LuaEngine).LoadProviderProcess();
        }

        public override void Register()
        {
            RegisterAdapter();
            App.Singleton<LuaEngine>().Alias<ILua>().Alias("LuaEngine").OnResolving((bind, obj) =>{

                LuaEngine luaEngine = obj as LuaEngine;
                IConfigStore config = App.Make<IConfigStore>();
                luaEngine.SetHotfixPath(config.Get<string[]>(typeof(XLuaEngine) , "lua.hotfix.path" , null));
                return obj;

            });
        }

        /// <summary>
        /// 注册Lua的适配器
        /// </summary>
        private void RegisterAdapter()
        {
            //此处默认的Lua引擎为XLua，如果需要替换，请自行实现对应的LuaProvider
            App.Singleton<ILuaEngineAdapter>((app, param) => new XLuaEngine()).Alias("LuaEngine.adapter");
        }
    }
}
