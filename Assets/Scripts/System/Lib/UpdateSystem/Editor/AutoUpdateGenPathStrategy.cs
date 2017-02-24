using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using CatLib;
using CatLib.Secret;
using CatLib.API.IO;
using CatLib.API.ResourcesSystem;


namespace CatLib.UpdateSystem{

	public class AutoUpdateGenPathStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.GenPath; } }

		public void Build(IBuildContext context){

			BuildListFile(context.ReleasePath);

		}

		/// <summary>
		/// 编译列表文件
		/// </summary>
		/// <param name="path">路径</param>
		protected void BuildListFile(string path){

			UpdateFile lst = new UpdateFile();
			IDirectory directory = IO.IO.MakeDirectory(path);
            directory.Walk((file)=>{

                if (!file.Name.EndsWith(".meta") && !file.Name.EndsWith(".DS_Store"))
                {
                    string fullName = file.FileInfo.FullName.Standard();
                    string assetName = fullName.Substring(path.Length);
                    lst.Append(assetName, MD5.ParseFile(file.FileInfo), file.FileInfo.Length);
                }

			}, SearchOption.AllDirectories);
			var store = new UpdateFileStore();
			store.Save(path , lst);
			
		}
	}

}