using UnityEngine;
using System.IO;
using System.Collections;
using XLua;
using CatLib.API;
using CatLib.API.Lua;
using CatLib.API.Resources;
using CatLib.API.IO;

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
        public IIO IO{ get; set; }

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

        private float lastGC = 0;

        public LuaStore(){

            LuaEnv.AddLoader(this.AutoLoader);

        }

        public void Update()
        {
            if (App.Time.Time - lastGC > GC_INTERVAL)
            {
                LuaEnv.Tick();
                lastGC = App.Time.Time;
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

            #if UNITY_EDITOR
            if (Env.DebugLevel == Env.DebugLevels.Auto)
            {
                yield break;
            }
            #endif

            Event.Trigger(LuaEvents.ON_HOT_FIXED_START);

            string[] filePaths = Config.Get<string[]>("lua.hotfix");

            IResources resources = App.Make<IResources>();

            foreach (string filePath in filePaths)
            {
                IFile[] infos = IO.Directory(Env.AssetPath + "/" + filePath).GetFiles(SearchOption.AllDirectories);
                foreach(var info in infos)
                {
                    if (!info.Name.EndsWith(".manifest"))
                    {
                        yield return resources.LoadAllAsync<TextAsset>(filePath + "/" + info.Name, (textAssets) =>
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
