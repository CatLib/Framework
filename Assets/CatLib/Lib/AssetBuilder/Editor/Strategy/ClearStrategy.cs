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
using CatLib.API.IO;
using CatLib.API.AssetBuilder;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// 清理策略
    /// </summary>
	public sealed class ClearStrategy : IBuildStrategy
    {
        /// <summary>
        /// 配置的编译流程
        /// </summary>
        public BuildProcess Process
        {
            get { return BuildProcess.Clear; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
		public void Build(IBuildContext context)
        {
            ClearAssetBundleFlag();
            ClearReleaseDir(context);
        }

        /// <summary>
        /// 清空发布文件
        /// </summary>
        /// <param name="context">编译上下文</param>
        private void ClearReleaseDir(IBuildContext context)
        {
            var releaseDir = context.Disk.Directory(context.ReleasePath, PathTypes.Absolute);
            releaseDir.Delete();
            releaseDir.Create();
        }

        /// <summary>
        /// 清空AssetBundle标记
        /// </summary>
        private void ClearAssetBundleFlag()
        {
            var assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in assetBundleNames)
            {
                AssetDatabase.RemoveAssetBundleName(name, true);
            }
        }
    }
}