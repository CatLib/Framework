using UnityEditor;
using System;
using CatLib.API.Resources;
using System.Collections.Generic;
using CatLib.API.IO;

namespace CatLib.Resources{

	public class AssetBundlesMaker{

        /// <summary>
        /// 编译Asset Bundle
        /// </summary>
        [MenuItem ("CatLib/Build AssetBundles")]
		public static void BuildAllAssetBundles ()
		{

			List<IBuildStrategy> strategys = new List<IBuildStrategy>();

			foreach(Type t in typeof(IBuildStrategy).GetChildTypesWithInterface()){
				
				strategys.Add(App.Instance.Make(t.ToString()) as IBuildStrategy);

			}
		
			strategys.Sort(	(left , right) => ((int)left.Process).CompareTo((int)right.Process));
			
			var context = new BuildContext();
			foreach(IBuildStrategy buildStrategy in strategys.ToArray()){

				buildStrategy.Build(context);

			}


	
			/* 
			RuntimePlatform switchPlatform = Env.SwitchPlatform;
			string platform = Env.PlatformToName(switchPlatform);

            ClearAssetBundle();
            BuildAssetBundleName(Env.DataPath + Env.ResourcesBuildPath);
			string releasePath = Env.DataPath + Env.ReleasePath + IO.IO.PATH_SPLITTER + platform;			
			IDirectory releaseDir = CatLib.IO.IO.MakeDirectory(releasePath);

            releaseDir.Delete();
			releaseDir.Create();

			IDirectory copyDire = CatLib.IO.IO.MakeDirectory(Env.DataPath + Env.ResourcesNoBuildPath);
			copyDire.CopyTo(Env.DataPath + Env.ReleasePath + IO.IO.PATH_SPLITTER + platform);

			BuildPipeline.BuildAssetBundles("Assets" + Env.ReleasePath + IO.IO.PATH_SPLITTER + platform, 
												BuildAssetBundleOptions.None ,
                                                PlatformToBuildTarget(switchPlatform));

            BuildListFile(releasePath);

			AssetDatabase.Refresh();*/
		}


		

	}

}