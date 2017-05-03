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
using CatLib.API;
using CatLib.API.IO;
using CatLib.API.AssetBuilder;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// 编译策略
    /// </summary>
    public sealed class BuildStrategy : IBuildStrategy
    {
        /// <summary>
        /// 环境配置
        /// </summary>
        [Inject]
        public IEnv Env { get; set; }

        /// <summary>
        /// 配置的编译流程
        /// </summary>
        public BuildProcess Process
        {
            get { return BuildProcess.Build; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
        public void Build(IBuildContext context)
        {
            var copyDir = context.Disk.Directory(context.NoBuildPath, PathTypes.Absolute);
            if (copyDir.Exists())
            {
                copyDir.CopyTo(context.ReleasePath);
            }

            BuildPipeline.BuildAssetBundles("Assets" + context.ReleasePath.Substring(Env.DataPath.Length),
                                                BuildAssetBundleOptions.None,
                                                context.BuildTarget);
        }
    }
}