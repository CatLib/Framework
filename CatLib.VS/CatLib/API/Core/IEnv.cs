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
        /// 系统资源路径
        /// <para>不同的调试等级下对应不同的资源路径</para>
        /// <para><c>DebugLevels.Prod</c> : 生产环境下将会为<c>Application.persistentDataPath</c>读写目录</para>
        /// <para><c>DebugLevels.Staging</c> : 仿真环境下将会为<c>StreamingAssets</c>文件夹</para>
        /// <para><c>DebugLevels.Dev</c> : 开发者环境下将会为<c>Application.dataPath</c>数据路径</para>
        /// <para>调试等级无论如何设置，脱离编辑器将自动使用<c>Application.persistentDataPath</c>读写目录</para>
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// 设定资源路径，开发者设定的资源路径会覆盖默认的资源路径策略
        /// </summary>
        /// <param name="path">路径</param>
        void SetAssetPath(string path);
    }
}