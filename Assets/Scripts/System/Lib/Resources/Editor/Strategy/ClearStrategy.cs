
using CatLib.API.Resources;
using UnityEditor;
using UnityEngine;
using CatLib;
using System;
using CatLib.API.IO;

namespace CatLib.Resources{

	public class ClearStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Clear; } }

		public void Build(IBuildContext context){

			ClearAssetBundleFlag();
			ClearReleaseDir(context);

		}

		/// <summary>
		/// 清空发布文件
		/// </summary>

		protected void ClearReleaseDir(IBuildContext context){

			IDirectory releaseDir = context.Disk.Directory(context.ReleasePath);
            releaseDir.Delete();
			releaseDir.Create();

		}
		
		/// <summary>
		/// 清空AssetBundle标记
		/// </summary>
		protected void ClearAssetBundleFlag(){

			string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
			foreach(string name in assetBundleNames){

				AssetDatabase.RemoveAssetBundleName(name , true);

			}

		}

	
	}

}