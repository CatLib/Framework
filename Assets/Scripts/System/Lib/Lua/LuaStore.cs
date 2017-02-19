using UnityEngine;
using System.IO;
using System.Collections;
using XLua;
using CatLib.Contracts;
using CatLib.Contracts.Lua;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Contracts.IO;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua 虚拟机
    /// </summary>
    public class LuaStore : Component , IUpdate , ILua
    {

        [Dependency]
        public Configs Config { get; set; }

        [Dependency]
        public IDirectory Directory { get; set; }

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

        public LuaStore(){

            LuaEnv.AddLoader(this.AutoLoader);

        }

        public void Update()
        {
            if (App.Time.Time - LuaBehaviour.lastGCTime > GC_INTERVAL)
            {
                LuaEnv.Tick();
                LuaBehaviour.lastGCTime = App.Time.Time;
            }
        }

        protected byte[] AutoLoader(ref string filepath)
        {
            TextAsset text = App.Make<IResources>().Load<TextAsset>(filepath);
            return text.bytes;
        }

        /// <summary>
        /// 加载热补丁
        /// </summary>
        public IEnumerator LoadHotFix()
        {
            return LoadHotFixAysn();  
        }

        protected IEnumerator LoadHotFixAysn()
        {
            Event.Trigger(LuaEvents.ON_HOT_FIXED_START);

            string[] filePath = Config.Get<string[]>("lua.hotfix");

            IResources resources = App.Make<IResources>();

            foreach (string file in filePath)
            {

                FileInfo[] infos = Directory.Walk((Env.AssetPath + "/" + file));

                foreach(var info in infos)
                {
                    if (!info.Name.EndsWith(".manifest"))
                    {
                        yield return resources.LoadAllAsyn<TextAsset>(file + "/" + info.Name, (textAssets) =>
                        {
                            Event.Trigger(LuaEvents.ON_HOT_FIXED_ACTION);
                            foreach (TextAsset text in textAssets)
                            {
                                LuaEnv.DoString(text.text);
                            }
                        });
                    }
                }
            }
            Event.Trigger(LuaEvents.ON_HOT_FIXED_END);
            Event.Trigger(LuaEvents.ON_HOT_FIXED_COMPLETE);
        }


    }
}
