using UnityEngine;
using System.Collections;
using CatLib.Base;
using XLua;
using CatLib.Container;
using CatLib.Support;
using CatLib.FileSystem;
using CatLib.ResourcesSystem;
using System.IO;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua 虚拟机
    /// </summary>
    public class CLua : CComponent , IUpdate
    {

        [CDependency]
        public CApplication Application { get; set; }

        [CDependency]
        public CConfig Config { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public class Events
        {
            public readonly static string ON_HOT_FIXED_START = "lua.hotfix.start";

            public readonly static string ON_HOT_FIXED_ACTION = "lua.hotfix.action";

            public readonly static string ON_HOT_FIXED_END = "lua.hotfix.end";

            public readonly static string ON_HOT_FIXED_COMPLETE = "lua.hotfix.complete";
        }

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

        /// <summary>
        /// 加载热补丁
        /// </summary>
        public void LoadHotFix()
        {
            Application.StartCoroutine(LoadHotFixAysn());  
        }

        protected IEnumerator LoadHotFixAysn()
        {
            Event.Trigger(Events.ON_HOT_FIXED_START);

            string[] filePath = Config.Get<string[]>("lua.hotfix");

            CResources resources = Application.Make<CResources>();

            foreach (string file in filePath)
            {

                FileInfo[] infos = (CEnv.AssetPath + "/" + file).RWalk();

                foreach(var info in infos)
                {
                    if (!info.Name.EndsWith(".manifest"))
                    {
                        yield return resources.LoadAllAsyn<TextAsset>(file + "/" + info.Name, (textAssets) =>
                        {
                            Event.Trigger(Events.ON_HOT_FIXED_ACTION);
                            foreach (TextAsset text in textAssets)
                            {
                                LuaEnv.DoString(text.text);
                            }
                        });
                    }
                }
            }
            Event.Trigger(Events.ON_HOT_FIXED_END);
            Event.Trigger(Events.ON_HOT_FIXED_COMPLETE);
        }


    }
}
