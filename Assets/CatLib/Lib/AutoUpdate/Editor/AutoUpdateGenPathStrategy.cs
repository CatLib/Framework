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
using CatLib.API.AssetBuilder;
using CatLib.API.IO;
using CatLib.Hash;

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 自动更新路径生成策略
    /// </summary>
    public sealed class AutoUpdateGenPathStrategy : IBuildStrategy
    {
        /// <summary>
        /// 配置的编译流程
        /// </summary>
        public BuildProcess Process
        {
            get { return BuildProcess.GenPath; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
        public void Build(IBuildContext context)
        {
            BuildListFile(context);
        }

        /// <summary>
        /// 编译列表文件
        /// </summary>
        /// <param name="context">编译上下文</param>
        private void BuildListFile(IBuildContext context)
        {
            var lst = new UpdateFile();

            IFile file;
            for (var i = 0; i < context.ReleaseFiles.Length; i++)
            {
                file = context.Disk.File(context.ReleasePath + Path.AltDirectorySeparatorChar + context.ReleaseFiles[i], PathTypes.Absolute);
                lst.Append(context.ReleaseFiles[i], Md5.ParseFile(file.FullName), file.Length);
            }

            var store = App.Instance.Make(typeof(UpdateFileStore).ToString()) as UpdateFileStore;
            store.Save(context.ReleasePath, lst);
        }
    }
}