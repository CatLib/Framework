using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using CatLib;
using CatLib.Contracts.IO;
using CatLib.Secret;

namespace CatLib.UpdateSystem{

	public class CreateAssetBundles{

        /// <summary>
        /// 编译Asset Bundle
        /// </summary>
        [MenuItem ("CatLib/Build AssetBundles")]
		public static void BuildAllAssetBundles ()
		{
			RuntimePlatform switchPlatform = Env.SwitchPlatform;
			string platform = Env.PlatformToName(switchPlatform);

            ClearAssetBundle();
            BuildAssetBundleName(Env.DataPath + Env.ResourcesBuildPath);

			string releasePath = Env.DataPath + Env.ReleasePath + "/" + platform;			
			
			IDirectory releaseDir = CatLib.IO.IO.MakeDirectory(releasePath);
			UnityEngine.Debug.Log(releaseDir);
            releaseDir.Delete();
			releaseDir.Create();

			IDirectory copyDire = CatLib.IO.IO.MakeDirectory(Env.DataPath + Env.ResourcesNoBuildPath);
			copyDire.CopyTo(Env.DataPath + Env.ReleasePath + "/" + platform);

			BuildPipeline.BuildAssetBundles("Assets" + Env.ReleasePath + "/" + platform, 
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

			IDirectory directory = CatLib.IO.IO.MakeDirectory(path);
            directory.Walk((file) => {

				if(!file.Name.EndsWith(".meta"))  
                {  
                     CreateAssetBundles.BuildFileBundleName(file.FileInfo , path);
                } 

			},SearchOption.AllDirectories);

		}

		/// <summary>
		/// 编译文件AssetBundle名字
		/// </summary>
		/// <param name="file">文件信息</param>
		/// <param name="basePath">基础路径</param>

		protected static void BuildFileBundleName(FileSystemInfo file , string basePath){

			string extension = file.Extension;
			string fullName = file.FullName.Standard();
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

			AssetImporter assetImporter = AssetImporter.GetAtPath("Assets" + Env.ResourcesBuildPath + assetName + "/" + baseFileName + extension);
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

			
			UpdateList lst = new UpdateList(path);
			IDirectory directory = CatLib.IO.IO.MakeDirectory(path);
            directory.Walk((file)=>{

                if (!file.Name.EndsWith(".meta"))
                {
                    string fullName = file.FileInfo.FullName.Standard();
                    string assetName = fullName.Substring(path.Length);
                    lst.Append(assetName, MD5.ParseFile(file.FileInfo), file.FileInfo.Length);
                }

			}, SearchOption.AllDirectories);
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
				default: throw new ArgumentException("Undefined Platform");

			}

		}

	}

}