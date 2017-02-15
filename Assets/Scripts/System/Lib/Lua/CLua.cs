using UnityEngine;
using System.Collections;
using CatLib.Base;
using XLua;
using CatLib.Container;
using CatLib.Support;
using CatLib.FileSystem;
using System.IO;
using CatLib.Contracts.Lua;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Contracts.Base;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua 虚拟机
    /// </summary>
    public class CLua : CComponent , IUpdate , ILua
    {

        [CDependency]
        public CConfig Config { get; set; }

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

        public CLua(){

            LuaEnv.AddLoader(this.AutoLoader);

        }

        public void Update()
        {
            if (Time.time - LuaBehaviour.lastGCTime > GC_INTERVAL)
            {
                LuaEnv.Tick();
                LuaBehaviour.lastGCTime = Time.time;
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
            Event.Trigger(CLuaEvents.ON_HOT_FIXED_START);

            string[] filePath = Config.Get<string[]>("lua.hotfix");

            IResources resources = App.Make<IResources>();

            foreach (string file in filePath)
            {

                FileInfo[] infos = CDirectory.Walk((CEnv.AssetPath + "/" + file));

                foreach(var info in infos)
                {
                    if (!info.Name.EndsWith(".manifest"))
                    {
                        yield return resources.LoadAllAsyn<TextAsset>(file + "/" + info.Name, (textAssets) =>
                        {
                            Event.Trigger(CLuaEvents.ON_HOT_FIXED_ACTION);
                            foreach (TextAsset text in textAssets)
                            {
                                LuaEnv.DoString(text.text);
                            }
                        });
                    }
                }
            }
            Event.Trigger(CLuaEvents.ON_HOT_FIXED_END);
            Event.Trigger(CLuaEvents.ON_HOT_FIXED_COMPLETE);
        }


    }
}
