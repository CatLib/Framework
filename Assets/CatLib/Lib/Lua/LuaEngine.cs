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

using System;
using CatLib.API.Lua;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua引擎
    /// </summary>
    internal sealed class LuaEngine : ILua
    {
        /// <summary>
        /// lua引擎
        /// </summary>
        public ILuaEngineAdapter LuaEngineAdapter { get; set; }
        /// <summary>
        /// 构造方法注入
        /// </summary>
        /// <param name="luaAdapter">Lua适配器</param>
        public LuaEngine(ILuaEngineAdapter luaAdapter)
        {
            if (luaAdapter == null)
            {
                throw new ArgumentNullException("ILuaEngineAdapter is Null, please check it!");
            }
            LuaEngineAdapter = luaAdapter;
        }

        public T GetLuaEngine<T>()
        {
            return (T)LuaEngineAdapter;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <returns>
        /// 执行结果
        /// 如果只有一个返回值那么返回的将是结果值
        /// 反之则是一个结果数组
        /// </returns>
        public object DoString(byte[] script)
        {
            return LuaEngineAdapter.DoString(script);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <returns>
        /// 执行结果
        /// 如果只有一个返回值那么返回的将是结果值
        /// 反之则是一个结果数组
        /// </returns>
        public object DoString(string script)
        {
            return LuaEngineAdapter.DoString(script);
        }

        /// <summary>
        /// 增加加载器
        /// </summary>
        /// <param name="callback">加载器回调</param>
        public void AddLoader(Func<string,byte[]> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            LuaEngineAdapter.AddLoader(callback);
        }

        /// <summary>
        /// 移除加载器
        /// </summary>
        /// <param name="callback">加载器回调</param>
        /// <returns>是否成功</returns>
        public bool RemoveLoader(Func<string,byte[]> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            return LuaEngineAdapter.RemoveLoader(callback);
        }

        /// <summary>
        /// 释放时
        /// </summary>
        public void OnDestroy()
        {
            LuaEngineAdapter.OnDestroy();
        }
    }
}


