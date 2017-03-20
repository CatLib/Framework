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
 
using CatLib.API.Resources;
using UnityEditor;
using CatLib.API.IO;
using CatLib.API.AssetBuilder;

namespace CatLib.AssetBuilder
{

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

			IDirectory releaseDir = context.Disk.Directory(context.ReleasePath, PathTypes.Absolute);
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