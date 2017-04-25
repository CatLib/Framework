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
using System.Collections;
using System.IO;
using System.Text;
using System;
using CatLib.API;
using CatLib.API.Resources;
using CatLib.API.IO;
using CatLib.API.Event;
using CatLib.API.Time;
using CatLib.API.Lua;


namespace CatLib.Lua
{
    // ===============================================================================
    // File Name           :    LuaEngine.cs
    // Class Description   :    Lua引擎，适配具体Lua引擎皆通过继承此类
    // Author              :    Mingming
    // Create Time         :    2017-04-22 15:40:33
    // ===============================================================================
    // Copyright © Mingming . All rights reserved.
    // ===============================================================================
    public sealed class LuaEngine : ILua
    {
        /// <summary>
        /// lua引擎
        /// </summary>
        public ILuaEngineAdapter LuaEngineAdapter { get; set; }

        public bool IsInited { get; set; }

        [Inject]
        public IIOFactory IO{ get; set; }

        [Inject]
        public IEnv Env { get; set; }

        [Inject]
        public IResources Resources { get; set; }

        private IDisk disk;

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk Disk { get{ return disk ?? (disk = IO.Disk()); } }

        /// <summary>
        /// 构造方法注入
        /// </summary>
        /// <param name="luaAdapter">Lua adapter.</param>
        public LuaEngine(ILuaEngineAdapter luaAdapter)
        {
            LuaEngineAdapter = luaAdapter;
            if(LuaEngineAdapter == null)
            {
                throw new System.Exception("ILuaEngineAdapter is NULL, please check it!");
            }
        }

        public void SetLuaEngineAdapter(ILuaEngineAdapter luaAdapter)
        {
            LuaEngineAdapter = luaAdapter;
            if(LuaEngineAdapter == null)
            {
                throw new System.Exception("ILuaEngineAdapter is NULL, please check it!");
            }
        }

        /// <summary>
        /// Application启动时加载，可以当做Init方法使用
        /// </summary>
        /// <returns>The provider process.</returns>
        public IEnumerator LoadProviderProcess()
        {
            if(LuaEngineAdapter == null)
            {
                throw new System.Exception("LoadProviderProcess is Failed,ILuaEngineAdapter is NULL, please check it!");
                yield break;
            }

            yield return LuaEngineAdapter.Init();

            IsInited = true;
        }

        public object ExecuteScript(byte[] scriptCode)
        {
            return LuaEngineAdapter.ExecuteScript(scriptCode);
        }

        public bool ExecuteScript(byte[] scriptCode,out object retObj)
        {
            return LuaEngineAdapter.ExecuteScript(scriptCode, out retObj);
        }
            
        public object ExecuteScript(string scriptCode)
        {
            return LuaEngineAdapter.ExecuteScript(scriptCode);
        }
            
        public object ExecuteScriptByFile(string filePath,string fileName)
        {
            byte[] scriptCode;
            if(HasScript(filePath, fileName, out scriptCode) == false) 
            {
                throw new System.Exception(string.Format("{0}{1} is not found", filePath, fileName));
                return null;
            }
            return LuaEngineAdapter.ExecuteScript(scriptCode);
        }
            
        public bool ExecuteScriptByFile(string filePath,string fileName,out object retObj)
        {
            byte[] scriptCode;
            if(HasScript(filePath, fileName, out scriptCode) == false) 
            {
                retObj = null;
                throw new System.Exception(string.Format("{0}{1} is not found", filePath, fileName));
                return false;
            }
            return LuaEngineAdapter.ExecuteScript(scriptCode, out retObj);
        }

        public bool HasScript(string filePath, string fileName, out byte[] scriptCode)
        {
            scriptCode = null;
            //是否存在此脚本
            bool result = false;

            string fullPath = filePath + Path.AltDirectorySeparatorChar + fileName;

            //判断Resources、AssetBundle、PersistentDataPath下是否存在该文件

            TextAsset text = Resources.Load<TextAsset>(fullPath,LoadTypes.Resources).Original as TextAsset;
            if (text != null)
            {
                scriptCode = text.bytes;
                result = true;
            }
            else
            {
                fullPath = Env.AssetPath + Path.AltDirectorySeparatorChar + fullPath;

                IFile file = Disk.File(fullPath, PathTypes.Absolute);
                if (file.Exists)
                {
                    scriptCode = file.Read();
                    result = true;
                }
                else
                {
                    text = Resources.Load<TextAsset>(fullPath).Original as TextAsset;
                    if (text != null)
                    {
                        scriptCode = text.bytes;
                        result = true;
                    }
                }
            }

            return result;
        }

        public void SetHotfixPath(string[] path)
        {
            LuaEngineAdapter.SetHotfixPath(path);
        }

        public void AddCustomLoader(Func<string,byte[]> callback)
        {
            if (callback == null)
            {
                throw new System.Exception(string.Format("Add LuaCustomLoader Callback is NULL!"));
                return;
            }

            LuaEngineAdapter.AddCustomLoader(callback);
        }

        public bool RemoveCustomLoader(Func<string,byte[]> callback)
        {
            if (callback == null)
            {
                throw new System.Exception(string.Format("Remove LuaCustomLoader Callback is NULL!"));
                return false;
            }

            return LuaEngineAdapter.RemoveCustomLoader(callback);
        }

        public void Dispose()
        {
            
        }
    } 
}


