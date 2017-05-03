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

using System.Collections.Generic;
using System.Text;
using System;
using XLua;
using CatLib.API;
using CatLib.API.Time;

namespace CatLib.Lua
{
    /// <summary>
    /// XLua引擎
    /// </summary>
    public sealed class XLuaEngine : IUpdate, ILuaEngineAdapter
    {
        /// <summary>
        /// 垃圾回收间隔
        /// </summary>
        private const float GC_INTERVAL = 1;

        /// <summary>
        /// XLua环境
        /// </summary>
        private readonly LuaEnv luaEnv;

        /// <summary>
        /// XLua环境
        /// </summary>
        private LuaTable scriptEnv;

        /// <summary>
        /// 特殊使用，通常情况下不需要获取XLua的执行环境
        /// 但在特殊情况下调用获取LuaEnv时，通过强制转换得到
        /// </summary>
        public LuaEnv LuaEnv
        {
            get { return luaEnv; }
        }

        /// <summary>
        /// 脚本环境
        /// </summary>
        public LuaTable ScriptEnv
        {
            get { return scriptEnv; }
        }

        /// <summary>
        /// 时间
        /// </summary>
        [Inject]
        public ITime Time { get; set; }

        /// <summary>
        /// 最后垃圾回收的时间
        /// </summary>
        private float lastGc;

        /// <summary>
        /// 用户定义的加载器
        /// </summary>
        private readonly IList<Func<string, byte[]>> customLoaders;

        /// <summary>
        /// XLua引擎
        /// </summary>
        public XLuaEngine()
        {
            luaEnv = new LuaEnv();
            scriptEnv = luaEnv.NewTable();
            customLoaders = new List<Func<string, byte[]>>();
            luaEnv.AddLoader(AutoLoader);
        }

        /// <summary>
        /// 增加加载器
        /// </summary>
        /// <param name="callback">用户定义的脚本加载器</param>
        public void AddLoader(Func<string, byte[]> callback)
        {
            customLoaders.Add(callback);
        }

        /// <summary>
        /// 移除加载器
        /// </summary>
        /// <param name="callback">要被移除的加载器</param>
        /// <returns>是否成功</returns>
        public bool RemoveLoader(Func<string, byte[]> callback)
        {
            return customLoaders.Remove(callback);
        }

        /// <summary>
        /// XLua自定义的加载器，通过此方法可以处理Lua的加密等情况
        /// </summary>
        /// <param name="fullPath">路径</param>
        /// <returns>Lua脚本字节流</returns>
        private byte[] AutoLoader(ref string fullPath)
        {
            foreach (var callback in customLoaders)
            {
                var callResult = callback(fullPath);
                if (callResult != null)
                {
                    return callResult;
                }
            }
            return null;
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            if (!(Time.Time - lastGc > GC_INTERVAL))
            {
                return;
            }
            luaEnv.Tick();
            lastGc = Time.Time;
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
            var results = luaEnv.DoString(Encoding.UTF8.GetString(script), "XLua code", ScriptEnv);
            object result;
            if (results != null && results.Length == 1)
            {
                result = results[0];
            }
            else
            {
                result = results;
            }
            return result;
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
            var results = luaEnv.DoString(script, "XLua code", ScriptEnv);
            object result;
            if (results != null && results.Length == 1)
            {
                result = results[0];
            }
            else
            {
                result = results;
            }
            return result;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void OnDestroy()
        {
            if (scriptEnv == null)
            {
                return;
            }
            scriptEnv.Dispose();
            scriptEnv = null;
        }
    }
}


