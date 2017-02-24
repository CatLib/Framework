using UnityEditor;
using System.IO;
using CatLib.API.IO;
using CatLib.API.ResourcesSystem;

namespace CatLib.ResourcesSystem{

	public class BuildStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Build; } }

		public void Build(IBuildContext context){

			BuildAssetBundleName(context.BuildPath);

			IDirectory copyDir = IO.IO.MakeDirectory(context.NoBuildPath);
            if (copyDir.Exists())
            {
                copyDir.CopyTo(context.ReleasePath);
            }

			BuildPipeline.BuildAssetBundles("Assets" + context.ReleasePath.Substring(Env.DataPath.Length), 
												BuildAssetBundleOptions.None ,
                                                context.BuildTarget);

		}

		/// <summary>
		/// 编译AssetBundle标记的名字
		/// </summary>
		/// <param name="path">路径</param>
		protected void BuildAssetBundleName(string path){

			IDirectory directory = IO.IO.MakeDirectory(path);
            directory.Walk((file) => {

				if(!file.Name.EndsWith(".meta"))  
                {  
                     BuildFileBundleName(file.FileInfo , path);
                } 

			},SearchOption.AllDirectories);

		}

		/// <summary>
		/// 编译文件AssetBundle名字
		/// </summary>
		/// <param name="file">文件信息</param>
		/// <param name="basePath">基础路径</param>

		protected void BuildFileBundleName(FileSystemInfo file , string basePath){

			string extension = file.Extension;
			string fullName = file.FullName.Standard();
			string fileName = file.Name;
			string baseFileName = fileName.Substring(0 , fileName.Length - extension.Length);
			string assetName = fullName.Substring(basePath.Length);
			assetName = assetName.Substring(0 , assetName.Length - fileName.Length).TrimEnd(IO.IO.PATH_SPLITTER);
			
			if(baseFileName + extension == ".DS_Store"){ return; }

			int variantIndex = baseFileName.LastIndexOf(".");
			string variantName = string.Empty;

			if(variantIndex > 0){

				variantName = baseFileName.Substring(variantIndex + 1);

			}

			AssetImporter assetImporter = AssetImporter.GetAtPath("Assets" + Env.ResourcesBuildPath + assetName + IO.IO.PATH_SPLITTER + baseFileName + extension);
			assetImporter.assetBundleName = assetName.TrimStart(IO.IO.PATH_SPLITTER);
			if(variantName != string.Empty){

				assetImporter.assetBundleVariant = variantName;

			}

		}



	}

}