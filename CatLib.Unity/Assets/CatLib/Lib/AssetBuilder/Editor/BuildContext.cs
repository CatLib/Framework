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

using UnityEditor;
using CatLib.API.AssetBuilder;
using CatLib.API.IO;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// AssetBuilder编译上下文
    /// </summary>
    public class BuildContext : IBuildContext
    {
        /// <summary>
        /// 当前编译目标平台
        /// </summary>
        public BuildTarget BuildTarget { get; set; }

        /// <summary>
        /// 目标平台的名字
        /// </summary>
        public string PlatformName { get; set; }

        /// <summary>
        /// 需要编译的文件路径
        /// </summary>
        public string BuildPath { get; set; }

        /// <summary>
        /// 不需要编译的文件路径
        /// </summary>
        public string NoBuildPath { get; set; }

        /// <summary>
        /// 最终发布的路径
        /// </summary>
        public string ReleasePath { get; set; }

        /// <summary>
        /// 最终发布的文件列表
        /// </summary>
        public string[] ReleaseFiles { get; set; }

        /// <summary>
        /// 被加密的文件列表
        /// </summary>
        public string[] EncryptionFiles { get; set; }

        /// <summary>
        /// 文件系统中对应的磁盘
        /// </summary>
        public IDisk Disk { get; set; }
    }
}