
using System;
using System.IO;

namespace CatLib.FileSystem{

	/// <summary>
	/// 文件夹
	/// </summary>
	public static class CDirectory{

		/// <summary>
		/// 操作行为
		/// </summary>
		public enum Operations{	

			/// <summary>
			/// 文件夹存在时就先执行删除文件夹的操作后再创建
			/// </summary>
			EXISTS_TO_DELETE = 1,

			/// <summary>
			/// 文件夹存在时就不执行任何操作
			/// </summary>
			EXISTS_TO_RETURN = 2,


		}

		/// <summary>
		/// 创建文件夹
		/// </summary>
		public static void CreateDir(this string path , Operations operation = Operations.EXISTS_TO_RETURN)
		{
			DirectoryInfo dir = new DirectoryInfo(path);
			if(dir.Exists){ 
				if(operation == Operations.EXISTS_TO_RETURN){ return; }
				dir.Delete(true);
			}
			dir.Create();
		}

		/// <summary>
		/// 文件夹是否存在
		/// </summary>
		public static bool DirExists(this string path){

			DirectoryInfo dir = new DirectoryInfo(path);
			return dir.Exists;

		}

		/// <summary>
		/// 遍历文件夹中的所有文件
		/// </summary>
		public static void Walk(this string path , Action<FileInfo> callBack){

            DirectoryInfo folder = new DirectoryInfo(path);
			FileSystemInfo[] files = folder.GetFileSystemInfos();
			int length = files.Length;
			for (int i = 0; i < length; i++) {

				if(files[i] is DirectoryInfo){

					files[i].FullName.Walk(callBack);

				}else{

					callBack(new FileInfo(files[i].FullName));

				}

			}

		}

	}

}
