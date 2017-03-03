using System;
using UnityEngine;
using CatLib.API;
using System.IO;

namespace CatLib
{

    /// <summary>
    /// 环境
    /// </summary>
    public class Env : IEnv
    {

        private Configs config;
        [Dependency]
        public Configs Config
        {
            get { return config; }
            set
            {
                config = value;
                Init(config);
            }
        }

        /// <summary>
        /// 调试等级
        /// </summary>
        public DebugLevels DebugLevel { get; protected set; }

        private string releasePath;

        /// <summary>
		/// 编译完成后发布AssetBundle的路径
		/// </summary>
		public string ReleasePath { get { return releasePath; } }


        private string resourcesBuildPath;

        /// <summary>
		/// 需要编译成AssetBundle的资源包路径
		/// </summary>
		public string ResourcesBuildPath { get{ return resourcesBuildPath; } }



        private string resourcesNoBuildPath;

        /// <summary>
        /// 需要编译成AssetBundle的资源包路径
        /// </summary>
        public string ResourcesNoBuildPath { get{ return resourcesNoBuildPath; } }

        /// <summary>
        /// 只可读不可写的文件存放路径(不能做热更新)
        /// </summary>
        public string StreamingAssetsPath{

			get{ return UnityEngine.Application.streamingAssetsPath; }

		}

		/// <summary>
		/// 只可读不可写的文件存放路径(不能做热更新)
		/// </summary>
		public string DataPath{

			get{ return UnityEngine.Application.dataPath; }

		}

		/// <summary>
		/// 可以更删改的文件路径
		/// </summary>
		public string PersistentDataPath{

			get{

				return UnityEngine.Application.persistentDataPath;

			}
			
		}

        private string assetPath;

        /// <summary>
        /// 热更新系统资源的下载路径
        /// </summary>
        public string AssetPath
        {

            get
            {
                return assetPath;
            }

        }

		/// <summary>
		/// 当前运行的平台(和编辑器所在平台有关)
		/// </summary>
		public RuntimePlatform Platform{

			get{

				return UnityEngine.Application.platform;

			}
			
		}

		/// <summary>
		/// 当前所选择的编译平台
		/// </summary>
		public RuntimePlatform SwitchPlatform{

			get{

				 #if UNITY_ANDROID
				 return RuntimePlatform.Android;
				 #endif

				 #if UNITY_IOS
				 return RuntimePlatform.IPhonePlayer;
				 #endif

				 #if UNITY_STANDALONE_WIN
				 return RuntimePlatform.WindowsPlayer;
				 #endif

				 #if UNITY_STANDALONE_OSX
				 return RuntimePlatform.OSXPlayer;
				 #endif

				 throw new Exception("Undefined Switch Platform");

			}

		}

		/// <summary>
		/// 将平台转为名字
		/// </summary>
		public string PlatformToName(RuntimePlatform? platform = null){

			if(platform == null){ platform = Platform; }
			switch(platform){
				
				case RuntimePlatform.LinuxPlayer: return "Linux";
				case RuntimePlatform.WindowsPlayer: 
				case RuntimePlatform.WindowsEditor: return "Win";
				case RuntimePlatform.Android: return "Android";
				case RuntimePlatform.IPhonePlayer: return "IOS";
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer: return "OSX";
				default: throw new ArgumentException("Undefined Platform");

			}

		}

        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="config"></param>
        protected void Init(Configs config)
        {

            if (config != null && config.IsExists("debug"))
            {
                DebugLevel = config.Get<DebugLevels>("debug");
            }
            else { DebugLevel = DebugLevels.Auto; }

            if (config != null && config.IsExists("release.path"))
            {
                releasePath = config.Get<string>("release.path");
            }
            else { releasePath = "Release"; }

            if (config != null && config.IsExists("build.asset.path"))
            {
                resourcesBuildPath = config.Get<string>("build.asset.path");
            }
            else { resourcesBuildPath = "Assets/AssetBundle"; }

            if (config != null && config.IsExists("nobuild.asset.path"))
            {
                resourcesNoBuildPath = config.Get<string>("nobuild.asset.path");
            }
            else { resourcesNoBuildPath = "Assets/NotAssetBundle"; }

            if (config != null && config.IsExists("asset.path"))
            {
                assetPath = config.Get<string>("asset.path");
            }
            else { assetPath = "Asset"; }

            releasePath = Path.AltDirectorySeparatorChar + releasePath;
            resourcesBuildPath = Path.AltDirectorySeparatorChar + resourcesBuildPath;
            resourcesNoBuildPath = Path.AltDirectorySeparatorChar + resourcesNoBuildPath;

            assetPath = PersistentDataPath + Path.AltDirectorySeparatorChar + "Asset";

            #if UNITY_EDITOR
            if (DebugLevel == DebugLevels.Staging)
            {
                assetPath = DataPath + ReleasePath + Path.AltDirectorySeparatorChar + PlatformToName(SwitchPlatform);
            }
            #endif
           
        }


    }

}