﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib.API.IO;
using UnityEditor;

namespace CatLib.API.AssetBuilder
{
    /// <summary>
    /// 编译上下文
    /// </summary>
    public interface IBuildContext
    {
        /// <summary>
        /// 当前编译目标平台
        /// </summary>
        BuildTarget BuildTarget { get; set; }

        /// <summary>
        /// 目标平台的名字
        /// </summary>
        string PlatformName { get; set; }

        /// <summary>
        /// 需要编译的文件路径
        /// </summary>
        string BuildPath { get; set; }

        /// <summary>
        /// 不需要编译的文件路径
        /// </summary>
        string NoBuildPath { get; set; }

        /// <summary>
        /// 最终发布的路径
        /// </summary>
        string ReleasePath { get; set; }

        /// <summary>
        /// 最终发布的文件列表
        /// </summary>
        string[] ReleaseFiles { get; set; }

        /// <summary>
        /// 被加密的文件列表
        /// </summary>
        string[] EncryptionFiles { get; set; }

        /// <summary>
        /// 文件系统磁盘
        /// </summary>
        IDisk Disk { get; set; }
    }
}