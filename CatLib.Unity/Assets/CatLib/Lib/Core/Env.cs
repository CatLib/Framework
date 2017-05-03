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
using System.IO;

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
        /// 资源发布路径
        /// </summary>
        private string releasePath;

        /// <summary>
        /// 编译完成后发布AssetBundle的路径
        /// </summary>
        public string ReleasePath
        {
            get { return releasePath; }
        }

        /// <summary>
        /// 需要编译成AssetBundle的资源包路径
        /// </summary>
        private string resourcesBuildPath;

        /// <summary>
        /// 需要编译成AssetBundle的资源包路径
        /// </summary>
        public string ResourcesBuildPath
        {
            get { return resourcesBuildPath; }
        }

        /// <summary>
        /// 资源无需编译成资源包的路径
        /// </summary>
        private string resourcesNoBuildPath;

        /// <summary>
        /// 资源无需编译成资源包的路径
        /// </summary>
        public string ResourcesNoBuildPath
        {
            get { return resourcesNoBuildPath; }
        }

        /// <summary>
        /// 只可读不可写的文件存放路径(不能做热更新)
        /// </summary>
        public string StreamingAssetsPath
        {
            get { return UnityEngine.Application.streamingAssetsPath; }
        }

        /// <summary>
        /// 只可读不可写的文件存放路径(不能做热更新)
        /// </summary>
        public string DataPath
        {
            get { return UnityEngine.Application.dataPath; }
        }

        /// <summary>
        /// 可以更删改的文件路径
        /// </summary>
        public string PersistentDataPath
        {
            get
            {
                return UnityEngine.Application.persistentDataPath;
            }
        }

        /// <summary>
        /// 热更新系统资源的下载路径
        /// </summary>
        private string assetPath;

        /// <summary>
        /// 热更新系统资源的下载路径
        /// </summary>
        public string AssetPath
        {
            get
            {
#if UNITY_EDITOR
                switch (DebugLevel)
                {
                    case DebugLevels.Staging:
                        return DataPath + ReleasePath + Path.AltDirectorySeparatorChar + PlatformToName(SwitchPlatform);
                    case DebugLevels.Auto:
                    case DebugLevels.Dev:
                        return DataPath;
                }
#endif
                return assetPath;
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
            SetDebugLevel(DebugLevels.Dev);
            SetReleasePath(null);
            SetResourcesBuildPath(null);
            SetResourcesNoBuildPath(null);
            SetAssetPath(null);
        }

        #region Config

        /// <summary>
        /// 设定调试等级
        /// </summary>
        /// <param name="level">调试等级</param>
        public void SetDebugLevel(DebugLevels level)
        {
            DebugLevel = level;
        }

        /// <summary>
        /// 设定发布路径
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public void SetReleasePath(string path)
        {
            releasePath = path;
            if (string.IsNullOrEmpty(releasePath))
            {
                releasePath = Path.AltDirectorySeparatorChar + "Release";
            }
            else
            {
                releasePath = Path.AltDirectorySeparatorChar + releasePath.Trim(Path.AltDirectorySeparatorChar);
            }
        }

        /// <summary>
        /// 设定资源编译路径
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public void SetResourcesBuildPath(string path)
        {
            resourcesBuildPath = path;
            if (string.IsNullOrEmpty(resourcesBuildPath))
            {
                resourcesBuildPath = Path.AltDirectorySeparatorChar + "Assets/AssetBundle";
            }
            else
            {
                resourcesBuildPath = Path.AltDirectorySeparatorChar + resourcesBuildPath.Trim(Path.AltDirectorySeparatorChar);
            }
        }

        /// <summary>
        /// 设定资源非编译路径
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public void SetResourcesNoBuildPath(string path)
        {
            resourcesNoBuildPath = path;
            if (string.IsNullOrEmpty(resourcesNoBuildPath))
            {
                resourcesNoBuildPath = Path.AltDirectorySeparatorChar + "Assets/NotAssetBundle";
            }
            else
            {
                resourcesNoBuildPath = Path.AltDirectorySeparatorChar + resourcesNoBuildPath.Trim(Path.AltDirectorySeparatorChar);
            }
        }

        /// <summary>
        /// 热更新系统资源的下载路径
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public void SetAssetPath(string path)
        {
            assetPath = path;
            if (string.IsNullOrEmpty(assetPath))
            {
                assetPath = PersistentDataPath + Path.AltDirectorySeparatorChar + "Assets";
            }
            else
            {
                assetPath = PersistentDataPath + Path.AltDirectorySeparatorChar + assetPath.Trim(Path.AltDirectorySeparatorChar);
            }
        }

        #endregion
    }
}