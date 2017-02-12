using CatLib.Base;
using UnityEngine;

namespace CatLib.Support
{

    /// <summary>
    /// 环境
    /// </summary>
    public class CEnv {

        public enum DebugLevels
        {

            /// <summary>
            /// 线上 
            /// </summary>
            ONLINE,

            /// <summary>
            /// 仿真模拟
            /// </summary>
            STAGING,

            /// <summary>
            /// 开发者模式
            /// </summary>
            DEV,

        }

        /// <summary>
        /// 调试等级
        /// </summary>
        public static DebugLevels DebugLevel { get { return DebugLevels.DEV; } }

        /// <summary>
		/// 编译完成后发布AssetBundle的路径
		/// </summary>
		public static string ReleasePath { get { return "/Release"; } }

        /// <summary>
		/// 需要编译成AssetBundle的资源包路径
		/// </summary>
		public static string ResourcesBuildPath { get{ return "/Assets/AssetBundle"; } }

		/// <summary>
		/// 需要编译成AssetBundle的资源包路径
		/// </summary>
		public static string ResourcesNoBuildPath { get{ return "/Assets/NotAssetBundle"; } }

        /// <summary>
        /// 只可读不可写的文件存放路径(不能做热更新)
        /// </summary>
        public static string StreamingAssetsPath{

			get{ return Application.streamingAssetsPath; }

		}

		/// <summary>
		/// 只可读不可写的文件存放路径(不能做热更新)
		/// </summary>
		public static string DataPath{

			get{ return Application.dataPath; }

		}

		/// <summary>
		/// 配置,热更新存放路径
		/// </summary>
		public static string PersistentDataPath{

			get{

				return Application.persistentDataPath;

			}
			
		}

        /// <summary>
        /// 热更新系统资源的下载路径
        /// </summary>
        public static string AssetPath
        {

            get
            {
                return CEnv.PersistentDataPath + "/" + "Asset";

            }

        }

		/// <summary>
		/// 当前运行的平台(和编辑器所在平台有关)
		/// </summary>
		public static RuntimePlatform Platform{

			get{

				return Application.platform;

			}
			
		}

		/// <summary>
		/// 当前所选择的编译平台
		/// </summary>
		public static RuntimePlatform SwitchPlatform{

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

				 throw new CException("Undefined Switch Platform");

			}

		}

		/// <summary>
		/// 将平台转为名字
		/// </summary>
		public static string PlatformToName(RuntimePlatform? platform = null){

			if(platform == null){ platform = CEnv.Platform; }
			switch(platform){
				
				case RuntimePlatform.LinuxPlayer: return "Linux";
				case RuntimePlatform.WindowsPlayer: 
				case RuntimePlatform.WindowsEditor: return "Win";
				case RuntimePlatform.Android: return "Android";
				case RuntimePlatform.IPhonePlayer: return "IOS";
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer: return "OSX";
				default: throw new CArgumentException("Undefined Platform");

			}

		}
		
		
	}

}