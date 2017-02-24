using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using CatLib;
using CatLib.API.ResourcesSystem;
using CatLib.Secret;
using CatLib.API.IO;
using System.Collections.Generic;

namespace CatLib.ResourcesSystem{

	public class AssetBundlesMaker{

        /// <summary>
        /// 编译Asset Bundle
        /// </summary>
        [MenuItem ("CatLib/Build AssetBundles")]
		public static void BuildAllAssetBundles ()
		{

			List<IBuildStrategy> strategys = new List<IBuildStrategy>();
			var container = new CatLib.Container.Container();
	
			foreach(Type t in typeof(IBuildStrategy).GetChildTypesWithInterface()){
				
				strategys.Add(container.Make(t.ToString()) as IBuildStrategy);

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