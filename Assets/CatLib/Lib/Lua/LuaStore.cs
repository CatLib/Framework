/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using UnityEngine;
using System.IO;
using System.Collections;
using XLua;
using CatLib.API;
using CatLib.API.Resources;
using CatLib.API.IO;
using CatLib.API.Event;
using CatLib.API.Time;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua 虚拟机
    /// </summary>
    public class LuaStore :  IUpdate , ILua
    {

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
        private IDisk Disk{

            get{
                return disk ?? (disk = IO.Disk());
            }
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

        private float lastGC = 0;

        private string[] hotfixPath;

        public void SetHotfixPath(string[] path){

            hotfixPath = path;

        }

        public LuaStore(){

            LuaEnv.AddLoader(this.AutoLoader);

        }

        public void Update()
        {
            if (Time.Time - lastGC > GC_INTERVAL)
            {
                LuaEnv.Tick();
                lastGC = Time.Time;
            }
        }

        protected byte[] AutoLoader(ref string filepath)
        {
            TextAsset text = Resources.Load<TextAsset>(filepath).Original as TextAsset;
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

            IResources resources = App.Make<IResources>();

            foreach (string filePath in filePaths)
            {
                IFile[] infos = Disk.Directory(Env.AssetPath + "/" + filePath , PathTypes.Absolute).GetFiles(SearchOption.AllDirectories);
                foreach(var info in infos)
                {
                    if (!info.Name.EndsWith(".manifest"))
                    {
                        yield return resources.LoadAllAsync(filePath + "/" + info.Name, (textAssets) =>
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
