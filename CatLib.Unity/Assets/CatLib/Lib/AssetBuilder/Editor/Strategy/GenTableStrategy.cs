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

using CatLib.API.AssetBuilder;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// 文件列表生成策略
    /// </summary>
    public sealed class GenTableStrategy : IBuildStrategy
    {
        /// <summary>
        /// 配置的编译流程
        /// </summary>
        public BuildProcess Process
        {
            get { return BuildProcess.GenTable; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
        public void Build(IBuildContext context)
        {
            context.EncryptionFiles = context.ReleaseFiles;
            /*
            string[] assetBundles = AssetDatabase.GetAllAssetBundleNames();
            string[] assetFiles;
            string head = "Assets" + IO.IO.PATH_SPLITTER + Env.ResourcesBuildPath;
            string relativeFile;
            string extension;
            string variant;
            List<string> assetFileList = new List<string>();
            for (int i = 0; i < assetBundles.Length; i++)
            {
                assetFiles = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundles[i]);
                for(int n = 0;n < assetFiles.Length; n++)
                {
                    relativeFile = assetFiles[n].Substring(head.Length);
                    extension = System.IO.Path.GetExtension(relativeFile);

                    relativeFile = relativeFile.Substring(0, relativeFile.Length - extension.Length);
                    variant = System.IO.Path.GetExtension(relativeFile);

                    if(variant != string.Empty)
                    {
                        relativeFile = relativeFile.Substring(0, relativeFile.Length - variant.Length);
                    }

                    if (!assetFileList.Contains(relativeFile + extension))
                    {
                        assetFileList.Add(relativeFile + extension);
                    }

                }
            }*/
        }
    }
}