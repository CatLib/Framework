using UnityEditor;
using CatLib.Support;
using UnityEngine;
using CatLib.Base;
using CatLib.FileSystem;
using System.IO;
using CatLib.UpdateSystem;
using CatLib.Secret;

namespace CatLib.UpdateSystem{

	public class CCreateAssetBundles{


		/// <summary>
		/// 编译Asset Bundle
		/// </summary>
		[MenuItem ("CatLib/Build AssetBundles")]
		public static void BuildAllAssetBundles ()
		{
			RuntimePlatform switchPlatform = CEnv.SwitchPlatform;
			string platform = CEnv.PlatformToName(switchPlatform);

            ClearAssetBundle();
            BuildAssetBundleName(CEnv.DataPath + CEnv.ResourcesBuildPath);

			string releasePath = CEnv.DataPath + CEnv.ReleasePath + "/" + platform;
			CDirectory.CreateDir(releasePath , CDirectory.Operations.EXISTS_TO_DELETE);
			CDirectory.CopyTo(CEnv.DataPath + CEnv.ResourcesNoBuildPath , CEnv.DataPath + CEnv.ReleasePath + "/" + platform);
			BuildPipeline.BuildAssetBundles("Assets" + CEnv.ReleasePath + "/" + platform, 
												BuildAssetBundleOptions.None ,
                                                PlatformToBuildTarget(switchPlatform));

            BuildListFile(releasePath);

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

			CDirectory.Walk(path , (file) => {

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
			
			if(baseFileName + extension == ".DS_Store"){ return; }

			int variantIndex = baseFileName.LastIndexOf(".");
			string variantName = string.Empty;

			if(variantIndex > 0){

				variantName = baseFileName.Substring(variantIndex + 1);

			}

			AssetImporter assetImporter = AssetImporter.GetAtPath("Assets" + CEnv.ResourcesBuildPath + assetName + "/" + baseFileName + extension);
			assetImporter.assetBundleName = assetName.TrimStart('/');
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
			CDirectory.Walk(path , (file)=>{

                if (!file.Standard().EndsWith(".meta"))
                {
                    string fullName = file.Standard();
                    string assetName = fullName.Substring(path.Length);
                    lst.Append(assetName, CMD5.ParseFile(file), file.Length);
                }

			});
			lst.Save();
			
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