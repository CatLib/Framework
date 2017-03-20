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

    public class ScanningStrategy : IBuildStrategy
    {

        public BuildProcess Process { get { return BuildProcess.Scanning; } }

        public void Build(IBuildContext context)
        {

            var filter = new List<string>(){ ".meta", ".DS_Store" };
            
            IDirectory releaseDir = context.Disk.Directory(context.ReleasePath, PathTypes.Absolute);

            var releaseFile = new List<string>();

            releaseDir.Walk((file) =>
            {

                if (!filter.Contains(file.Extension))
                {

                    releaseFile.Add(file.FullName.Substring(context.ReleasePath.Length).Trim(Path.AltDirectorySeparatorChar, '\\').Standard());

                }


            }, System.IO.SearchOption.AllDirectories);

            context.ReleaseFiles = releaseFile.ToArray();

        }

    }

}