using UnityEditor;
using CatLib.Support;
using UnityEngine;
using CatLib.Exception;
using CatLib.FileSystem;
using System.IO;
using CatLib.UpdateSystem;

namespace CatLib.Tool{

	public class CCreateAssetBundles{

		/// <summary>
		/// 编译完成后发布AssetBundle的路径
		/// </summary>
		protected const string RELEASE_PATH = "Release/";

		/// <summary>
		/// 需要编译成AssetBundle的资源包路径
		/// </summary>
		protected const string RESOURCES_BUILD_PATH =  "ResourcesBuild/";

		/// <summary>
		/// 编译Asset Bundle
		/// </summary>
		[MenuItem ("Release/Build AssetBundles")]
		public static void BuildAllAssetBundles ()
		{
			RuntimePlatform switchPlatform = CEnv.SwitchPlatform;
			string platform = CEnv.PlatformToName(switchPlatform);

			CCreateAssetBundles.ClearAssetBundle();
			CCreateAssetBundles.BuildAssetBundleName(CEnv.DataPath + CCreateAssetBundles.RESOURCES_BUILD_PATH);

			string releasePath = CEnv.DataPath + CCreateAssetBundles.RELEASE_PATH + platform;
			CDirectory.CreateDir(releasePath , CDirectory.Operations.EXISTS_TO_DELETE);
			BuildPipeline.BuildAssetBundles("Assets/" + CCreateAssetBundles.RELEASE_PATH + platform, 
												BuildAssetBundleOptions.None , 
												CCreateAssetBundles.PlatformToBuildTarget(switchPlatform));

			CCreateAssetBundles.BuildListFile(releasePath);

			AssetDatabase.Refresh();
		}

		/// <summary>
		/// 清空AssetBundle标记
		/// </summary>
		protected static void ClearAssetBundle(){

			string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
			foreach(string name in assetBundleNames){

				AssetDatabase.RemoveAssetBundleName(name , true);

			}

		}

		/// <summary>
		/// 编译AssetBundle标记的名字
		/// </summary>
		/// <param name="path">路径</param>
		protected static void BuildAssetBundleName(string path){

			path.Walk((file) => {

				if(!file.Name.EndsWith(".meta"))  
                {  
                     CCreateAssetBundles.BuildFileBundleName(file , path);
                } 

			});

		}

		/// <summary>
		/// 编译文件AssetBundle名字
		/// </summary>
		/// <param name="file">文件信息</param>
		/// <param name="basePath">基础路径</param>

		protected static void BuildFileBundleName(FileSystemInfo file , string basePath){

			string extension = file.Extension;
			string fullName = file.Standard();
			string fileName = file.Name;
			string baseFileName = fileName.Substring(0 , fileName.Length - extension.Length);
			string assetName = fullName.Substring(basePath.Length);
			assetName = assetName.Substring(0 , assetName.Length - fileName.Length).TrimEnd('/');

			int variantIndex = baseFileName.LastIndexOf(".");
			string variantName = string.Empty;

			if(variantIndex > 0){

				variantName = baseFileName.Substring(variantIndex + 1);

			}

			AssetImporter assetImporter = AssetImporter.GetAtPath("Assets/" + CCreateAssetBundles.RESOURCES_BUILD_PATH + assetName + "/" + baseFileName + extension);
			assetImporter.assetBundleName = assetName + ".hot";
			if(variantName != string.Empty){

				assetImporter.assetBundleVariant = variantName;

			}

		}

		/// <summary>
		/// 编译列表文件
		/// </summary>
		/// <param name="path">路径</param>
		protected static void BuildListFile(string path){

			CUpdateList lst = new CUpdateList(path);
			path.Walk((file)=>{

				string fullName = file.Standard();
				string assetName = fullName.Substring(path.Length);
				lst.Append(assetName , CMD5.ParseFile(file) , file.Length);

			});
			lst.Close();
			
		}

		/// <summary>
		/// 将平台转为编译目标
		/// </summary>
		protected static BuildTarget PlatformToBuildTarget(RuntimePlatform platform){

			switch(platform){
				
				case RuntimePlatform.LinuxPlayer: return BuildTarget.StandaloneLinux64;
				case RuntimePlatform.WindowsPlayer: 
				case RuntimePlatform.WindowsEditor: return BuildTarget.StandaloneWindows64;
				case RuntimePlatform.Android: return BuildTarget.Android;
				case RuntimePlatform.IPhonePlayer: return BuildTarget.iOS;
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer: return BuildTarget.StandaloneOSXIntel64;
				default: throw new CArgumentException("Undefined Platform");

			}

		}

	}

}