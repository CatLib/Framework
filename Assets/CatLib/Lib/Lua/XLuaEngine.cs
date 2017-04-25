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
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using XLua;
using CatLib.API;
using CatLib.API.Resources;
using CatLib.API.IO;
using CatLib.API.Event;
using CatLib.API.Time;
using CatLib.API.Lua;

namespace CatLib.Lua
{
    // ===============================================================================
    // File Name           :    XLuaEngine.cs
    // Class Description   :    Xlua引擎
    // Author              :    Mingming
    // Create Time         :    2017-04-22 15:57:43
    // ===============================================================================
    // Copyright © Mingming . All rights reserved.
    // ===============================================================================
    public class XLuaEngine : IUpdate , ILuaEngineAdapter
    {
        /// <summary>
        /// XLua
        /// </summary>
        private LuaEnv luaEnv;
        /// <summary>
        /// XLua
        /// </summary>
        private LuaTable scriptEnv;

        /// <summary>
        /// 特殊使用，通常情况下不需要获取XLua的执行环境
        /// 但在特殊情况下调用获取LuaEnv时，通过强制转换得到
        /// </summary>
        /// <value>The X lua env.</value>
        public LuaEnv LuaEnv {get {return luaEnv;}}

        public LuaTable ScriptEnv { get{ return scriptEnv; }}

        [Inject]
        public IIOFactory IO{ get; set; }

        [Inject]
        public IEnv Env { get; set; }

        [Inject]
        public IResources Resources { get; set; }

        [Inject]
        public IEventImpl Event { get; set; }

        [Inject]
        public IApplication App { get; set; }

        [Inject]
        public ITime Time { get; set; }

        private IDisk disk;

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk Disk { get{ return disk ?? (disk = IO.Disk()); } }

        private string[] hotfixPath;
        /// <summary>
        /// 垃圾回收间隔
        /// </summary>
        protected const float GC_INTERVAL = 1;

        private float lastGC = 0;

        private IList<Func<string,byte[]>> customLoaders;

        public XLuaEngine() {
            luaEnv = new LuaEnv();
            scriptEnv = luaEnv.NewTable();
            customLoaders = new List<Func<string,byte[]>>();
        }

        public IEnumerator Init()
        {
            yield return LoadHotFix();
            //XLua默认设置自定义加载器，需要自定义加载器的时候直接调用Add方法添加即可
            luaEnv.AddLoader(AutoLoader);
        }

        public void SetHotfixPath(string[] path)
        {
            hotfixPath = path;
        }

        public void AddCustomLoader(Func<string,byte[]> callback)
        {
            customLoaders.Add(callback);
        }

        public bool RemoveCustomLoader(Func<string,byte[]> callback)
        {
            return customLoaders.Remove(callback);
        }

        /// <summary>
        /// XLua自定义的加载器，通过此方法可以处理Lua的加密等情况
        /// </summary>
        /// <returns>The loader.</returns>
        /// <param name="filepath">Filepath.</param>
        protected byte[] AutoLoader(ref string fullPath)
        {
            //这块使用框架自定义的Func来处理，方便做适配
            foreach (Func<string,byte[]> callback in customLoaders)
            {
                byte[] callResult = callback(fullPath);
                if (callResult != null)
                {
                    return callResult;
                }
            }

            return null;
        }

        public void Update()
        {
            if (Time.Time - lastGC > GC_INTERVAL)
            {
                luaEnv.Tick();
                lastGC = Time.Time;
            }
        }

        public object ExecuteScript(byte[] scriptCode)
        {
            object[] results = luaEnv.DoString(Encoding.UTF8.GetString(scriptCode), "XLua code", ScriptEnv);
            object ret = null;
            if(results != null && results.Length == 1)
            {
                ret = results[0];
            }
            else
            {
                ret = results;
            }
            return results[0];
        }

        public bool ExecuteScript(byte[] scriptCode,out object retObj)
        {
            object[] results = luaEnv.DoString(Encoding.UTF8.GetString(scriptCode), "XLua code", ScriptEnv);
            if(results != null && results.Length == 1)
            {
                retObj = results[0];
            }else
            {
                retObj = results;
            }
            return true;
        }

        public object ExecuteScript(string scriptCode)
        {
            object[] results = luaEnv.DoString(scriptCode, "XLua code", ScriptEnv);
            object ret;
            if(results != null && results.Length == 1)
            {
                ret = results[0];
            }else
            {
                ret = results;
            }
            return ret;
        }

        public bool ExecuteScript(string scriptCode,out object retObj)
        {
            object[] results = luaEnv.DoString(scriptCode, "XLua code");
            if(results != null && results.Length == 1)
            {
                retObj = results[0];
            }else
            {
                retObj = results;
            }
            return true;
        }

        /// <summary>
        /// 加载热补丁
        /// </summary>
        public IEnumerator LoadHotFix()
        {
            return LoadHotFixAysn();  
        }
        /// <summary>
        /// 异步加载Lua热修复脚本
        /// </summary>
        /// <returns>The hot fix aysn.</returns>
        protected IEnumerator LoadHotFixAysn()
        {

            //如果处于编辑器下的自动模式和开发者模式则不进行热补丁
            #if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
            {
                yield break;
            }
            #endif

            if(hotfixPath == null){ yield break; }

            Event.Trigger(LuaEvents.ON_HOT_FIXED_START);

            string[] filePaths = hotfixPath;

            foreach (string filePath in filePaths)
            {
                IFile[] infos = Disk.Directory(Env.AssetPath + "/" + filePath , PathTypes.Absolute).GetFiles(SearchOption.AllDirectories);
                foreach(var info in infos)
                {
                    if (!info.Name.EndsWith(".manifest"))
                    {
                        yield return Resources.LoadAllAsync(filePath + "/" + info.Name, (textAssets) =>
                            {
                                Event.Trigger(LuaEvents.ON_HOT_FIXED_ACTION);
                                foreach (TextAsset text in textAssets)
                                {
                                    luaEnv.DoString(text.text, "XLua hot fix code");
                                }
                            });
                    }
                }
            }
            Event.Trigger(LuaEvents.ON_HOT_FIXED_END);
            Event.Trigger(LuaEvents.ON_HOT_FIXED_COMPLETE);
        }

        public void Dispose()
        {
            if (scriptEnv != null)
            {
                scriptEnv.Dispose();
                scriptEnv = null;
            }
        }
    }
}


