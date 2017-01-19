using UnityEngine;
using System.Collections;
using CatLib.Base;
using XLua;
using CatLib.Container;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua 虚拟机
    /// </summary>
    public class CLua : CComponent , IUpdate , ILua
    {

        /// <summary>
        /// 垃圾回收间隔
        /// </summary>
        protected const float GC_INTERVAL = 1; 

        /// <summary>
        /// Lua 虚拟机
        /// </summary>
        protected LuaEnv luaEnv = new LuaEnv();

        /// <summary>
        /// Lua 虚拟机
        /// </summary>
        public LuaEnv LuaEnv { get { return luaEnv; } } 


        public void Update()
        {
            if (Time.time - LuaBehaviour.lastGCTime > GC_INTERVAL)
            {
                LuaEnv.Tick();
                LuaBehaviour.lastGCTime = Time.time;
            }
        }


    }
}
