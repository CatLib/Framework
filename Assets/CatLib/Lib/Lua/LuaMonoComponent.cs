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
using XLua;
using System;
using CatLib.API.Lua;
using CatLib.API.Resources;
using CatLib.Lua;

namespace CatLib
{

    [LuaCallCSharp]
    public class LuaMonoComponent : MonoComponent
    {
        /// <summary>
        /// Lua文件路径
        /// </summary>
        public string luaPath;

        /// <summary>
        /// 注入内容
        /// </summary>
        public Injection[] injections;

        /// <summary>
        /// 脚本环境
        /// </summary>
        private LuaTable scriptEnv;

        private Action luaStart;

        private Action luaUpdate;

        private Action luaOnDestroy;

        protected ILua Lua
        {
            get { return App.Make<ILua>(); }
        }

        protected XLuaEngine LuaEngine
        {
            get { /*return (App.Make<ILua>().LuaEngineAdapter as XLuaEngine);*/
                return null;
            }
        }

        void Awake()
        {
            BeforeInit();
            Init();
            AfterInit();
        }

        public virtual void BeforeInit()
        {
            scriptEnv = LuaEngine.ScriptEnv;
            LuaTable meta = LuaEngine.LuaEnv.NewTable();
            meta.Set("__index", LuaEngine.LuaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
        }

        public virtual void AfterInit()
        {
            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);

            if (luaAwake != null)
            {
                luaAwake();
            }
        }

        public virtual void Init()
        {
            TextAsset text = App.Make<IResources>().Load<TextAsset>(luaPath).Get<TextAsset>(scriptEnv);

            Lua.DoString(text.text);
        }

        void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
        }

        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
        }

        public override void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            injections = null;
            base.OnDestroy();
        }

    }

}
