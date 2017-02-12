using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Lua;
using CatLib.Container;
using XLua;
using System;
using CatLib.ResourcesSystem;
using CatLib.Contracts.Lua;
using CatLib.Contracts.ResourcesSystem;

namespace CatLib.Base {

    /// <summary>
    /// Lua 组件
    /// </summary>
    public class CLuaMonoComponent : CMonoComponent
    {

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

        protected LuaEnv LuaEnv
        {
            get { return Application.Make<ILua>().LuaEnv; }
        }

        void Awake()
        {

            scriptEnv = LuaEnv.NewTable();

            LuaTable meta = LuaEnv.NewTable();
            meta.Set("__index", LuaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
            foreach (var injection in injections)
            {
                scriptEnv.Set(injection.name, injection.value);
            }

            TextAsset text = Application.Make<IResources>().Load<TextAsset>(luaPath);

            LuaEnv.DoString(text.text, "LuaBehaviour", scriptEnv);

            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);

            if (luaAwake != null)
            {
                luaAwake();
            }

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
            if (scriptEnv != null)
            {
                scriptEnv.Dispose();
                scriptEnv = null;
            }
            injections = null;
            base.OnDestroy();
        }

    }

}
