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

using CatLib.API.Lua;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua服务
    /// </summary>
    internal class LuaProvider : ServiceProvider
    {
        /// <summary>
        /// 注册Lua适配器服务时
        /// </summary>
        public override void Register()
        {
            RegisterAdapter();
            App.Singleton<LuaEngine>().Alias<ILua>().Alias("LuaEngine");
        }

        /// <summary>
        /// 注册Lua的适配器
        /// </summary>
        private void RegisterAdapter()
        {
            App.Singleton<ILuaEngineAdapter>((app, param) => new XLuaEngine()).Alias("LuaEngine.adapter");
        }
    }
}
