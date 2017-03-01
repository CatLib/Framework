using CatLib.API.IO;
using CatLib.API.Resources;
using System.Collections.Generic;
using System.IO;

namespace CatLib.Resources
{

    public class ScanningStrategy : IBuildStrategy
    {

        public BuildProcess Process { get { return BuildProcess.Scanning; } }

        public void Build(IBuildContext context)
        {

            var filter = new List<string>(){ ".meta", ".DS_Store" };
            
            IDirectory releaseDir = context.Disk.Directory(context.ReleasePath);

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