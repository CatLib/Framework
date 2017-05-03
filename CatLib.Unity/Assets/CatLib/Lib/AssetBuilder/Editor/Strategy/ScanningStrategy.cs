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

using System.Collections.Generic;
using System.IO;
using CatLib.API.AssetBuilder;
using CatLib.API.IO;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// 文件扫描策略
    /// </summary>
    public sealed class ScanningStrategy : IBuildStrategy
    {
        /// <summary>
        /// 配置的编译流程
        /// </summary>
        public BuildProcess Process
        {
            get { return BuildProcess.Scanning; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
        public void Build(IBuildContext context)
        {
            var filter = new List<string>() { ".meta", ".DS_Store" };
            var releaseDir = context.Disk.Directory(context.ReleasePath, PathTypes.Absolute);
            var releaseFile = new List<string>();
            releaseDir.Walk((file) =>
            {
                if (!filter.Contains(file.Extension))
                {
                    releaseFile.Add(Util.StandardPath(file.FullName.Substring(context.ReleasePath.Length).Trim(Path.AltDirectorySeparatorChar, '\\')));
                }
            }, SearchOption.AllDirectories);

            context.ReleaseFiles = releaseFile.ToArray();
        }
    }
}