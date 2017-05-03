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
using UnityEditor;
using System.IO;
using CatLib.API.AssetBuilder;
using CatLib.API;
using CatLib.API.IO;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// 在编译之前执行的策略
    /// </summary>
    public sealed class PrecompiledStrategy : IBuildStrategy
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
            get { return BuildProcess.Precompiled; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
        public void Build(IBuildContext context)
        {
            BuildAssetBundleName(context);
        }

        /// <summary>
        /// 编译AssetBundle标记的名字
        /// </summary>
        private void BuildAssetBundleName(IBuildContext context)
        {
            var directory = context.Disk.Directory(context.BuildPath, PathTypes.Absolute);
            directory.Walk((file) =>
            {
                if (!file.Name.EndsWith(".meta"))
                {
                    BuildFileBundleName(file, context.BuildPath);
                }
            }, SearchOption.AllDirectories);
        }

        /// <summary>
        /// 编译文件AssetBundle名字
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="basePath">基础路径</param>
        private void BuildFileBundleName(IFile file, string basePath)
        {
            var extension = file.Extension;
            var fullName = Util.StandardPath(file.FullName);
            var fileName = file.Name;
            var baseFileName = fileName.Substring(0, fileName.Length - extension.Length);
            var assetName = fullName.Substring(basePath.Length);
            assetName = assetName.Substring(0, assetName.Length - fileName.Length).TrimEnd(Path.AltDirectorySeparatorChar);

            if (baseFileName + extension == ".DS_Store")
            {
                return;
            }

            var variantIndex = baseFileName.LastIndexOf(".", StringComparison.Ordinal);
            var variantName = string.Empty;

            if (variantIndex > 0)
            {
                variantName = baseFileName.Substring(variantIndex + 1);
            }

            var assetImporter = AssetImporter.GetAtPath("Assets" + Env.ResourcesBuildPath + assetName + Path.AltDirectorySeparatorChar + baseFileName + extension);
            assetImporter.assetBundleName = assetName.TrimStart(Path.AltDirectorySeparatorChar);
            if (variantName != string.Empty)
            {
                assetImporter.assetBundleVariant = variantName;
            }
        }
    }
}