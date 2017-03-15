using UnityEditor;
using System.IO;
using CatLib.API;
using CatLib.API.IO;
using CatLib.API.Resources;
using CatLib.API.AssetBuilder;

namespace CatLib.AssetBuilder
{

	public class BuildStrategy : IBuildStrategy {


        [Dependency]
        public IEnv Env { get; set; }

        public BuildProcess Process{ get { return BuildProcess.Build; } }

		public void Build(IBuildContext context){

			IDirectory copyDir = context.Disk.Directory(context.NoBuildPath, PathTypes.Absolute);
            if (copyDir.Exists())
            {
                copyDir.CopyTo(context.ReleasePath);
            }

			BuildPipeline.BuildAssetBundles("Assets" + context.ReleasePath.Substring(Env.DataPath.Length), 
												BuildAssetBundleOptions.None ,
                                                context.BuildTarget);

		}

	}

}