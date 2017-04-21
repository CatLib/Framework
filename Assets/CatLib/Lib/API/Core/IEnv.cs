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

namespace CatLib.API
{
    /// <summary>
    /// 环境
    /// </summary>
    public interface IEnv
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        DebugLevels DebugLevel { get; }

        /// <summary>
        /// 编译完成后发布AssetBundle的路径
        /// </summary>
        string ReleasePath { get; }

        /// <summary>
        /// 需要编译成AssetBundle的资源包路径
        /// </summary>
        string ResourcesBuildPath { get; }

        /// <summary>
        /// 需要编译成AssetBundle的资源包路径
        /// </summary>
        string ResourcesNoBuildPath { get; }

        /// <summary>
        /// 只可读不可写的文件存放路径(不能做热更新)
        /// </summary>
        string StreamingAssetsPath { get; }

        /// <summary>
        /// 只可读不可写的文件存放路径(不能做热更新)
        /// </summary>
        string DataPath { get; }

        /// <summary>
        /// 可以更删改的文件路径
        /// </summary>
        string PersistentDataPath { get; }

        /// <summary>
        /// 热更新系统资源的下载路径
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// 当前运行的平台(和编辑器所在平台有关)
        /// </summary>
        RuntimePlatform Platform { get; }

        /// <summary>
        /// 当前所选择的编译平台
        /// </summary>
        RuntimePlatform SwitchPlatform { get; }

        /// <summary>
        /// 将平台转为名字
        /// </summary>
        /// <param name="platform">平台名</param>
        /// <returns>转换后的名字</returns>
        string PlatformToName(RuntimePlatform? platform = null);
    }
}