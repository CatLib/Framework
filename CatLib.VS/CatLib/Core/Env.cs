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

using System;
using UnityEngine;
using CatLib.API;

namespace CatLib
{
    /// <summary>
    /// 环境
    /// </summary>
    public sealed class Env : IEnv
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        public DebugLevels DebugLevel { get; private set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        private string assetPath;

        /// <summary>
        /// 系统资源路径
        /// <para>不同的调试等级下对应不同的资源路径</para>
        /// <para><c>DebugLevels.Prod</c> : 生产环境下将会为<c>Application.persistentDataPath</c>读写目录</para>
        /// <para><c>DebugLevels.Staging</c> : 仿真环境下将会为<c>StreamingAssets</c>文件夹</para>
        /// <para><c>DebugLevels.Dev</c> : 开发者环境下将会为<c>Application.dataPath</c>数据路径</para>
        /// <para>调试等级无论如何设置，脱离编辑器将自动使用<c>Application.persistentDataPath</c>读写目录</para>
        /// <para>如果开发者有手动设置资源路径，将使用开发者设置的路径</para>
        /// </summary>
        public string AssetPath
        {
            get
            {
                if (!string.IsNullOrEmpty(assetPath))
                {
                    return assetPath;
                }
#if UNITY_EDITOR
                switch (DebugLevel)
                {
                    case DebugLevels.Staging:
                        return UnityEngine.Application.dataPath + Path.AltDirectorySeparatorChar + "StreamingAssets";
                    case DebugLevels.Auto:
                    case DebugLevels.Dev:
                        return UnityEngine.Application.dataPath;
                    case DebugLevels.Prod:
                        return UnityEngine.Application.persistentDataPath;
                }
#endif
#if UNITY_5_OR_NEW
                return UnityEngine.Application.persistentDataPath;
#else
                throw new RuntimeException("Not set asset path");
#endif
            }
        }

        /// <summary>
        /// 当前运行的平台(和编辑器所在平台有关)
        /// </summary>
        public RuntimePlatform Platform
        {
            get
            {
                return UnityEngine.Application.platform;
            }
        }

        /// <summary>
        /// 当前所选择的编译平台
        /// </summary>
        public RuntimePlatform SwitchPlatform
        {
            get
            {
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
        /// <param name="platform">平台名</param>
        /// <returns>转换后的名字</returns>
        public string PlatformToName(RuntimePlatform? platform = null)
        {
            if (platform == null)
            {
                platform = Platform;
            }
            switch (platform)
            {
                case RuntimePlatform.LinuxPlayer:
                    return "Linux";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Win";
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS";
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                default:
                    throw new ArgumentException("Undefined Platform");
            }
        }

        /// <summary>
        /// 构造一个环境
        /// </summary>
        public Env()
        {
            SetDebugLevel(DebugLevels.Auto);
        }

        /// <summary>
        /// 设定调试等级
        /// </summary>
        /// <param name="level">调试等级</param>
        public void SetDebugLevel(DebugLevels level)
        {
            DebugLevel = level;
        }

        /// <summary>
        /// 设定资源路径，开发者设定的资源路径会覆盖默认的资源路径策略
        /// </summary>
        /// <param name="path">路径</param>
        public void SetAssetPath(string path)
        {
            assetPath = path;
        }
    }
}