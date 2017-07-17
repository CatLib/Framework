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

using System.IO;
using CatLib.API;

namespace CatLib.Core
{
    /// <summary>
    /// 环境
    /// </summary>
    internal sealed class Env : IEnv
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
        [ExcludeFromCodeCoverage]
        public string AssetPath
        {
            get
            {
                if (!string.IsNullOrEmpty(assetPath))
                {
                    return assetPath;
                }

                return GetAssetPathWithUnity();
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
        /// 从Unity获取资源路径
        /// </summary>
        /// <returns>资源路径</returns>
        [ExcludeFromCodeCoverage]
        private string GetAssetPathWithUnity()
        {
            if (UnityEngine.Application.isEditor)
            {
                switch (DebugLevel)
                {
                    case DebugLevels.Staging:
                        return Path.Combine(UnityEngine.Application.dataPath, "StreamingAssets");
                    case DebugLevels.Auto:
                    case DebugLevels.Dev:
                        return UnityEngine.Application.dataPath;
                    case DebugLevels.Prod:
                        return UnityEngine.Application.persistentDataPath;
                }
            }

            return UnityEngine.Application.persistentDataPath;
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