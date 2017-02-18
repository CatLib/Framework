
using System;
using System.Collections.Generic;
using System.IO;

namespace CatLib.FileSystem
{

	/// <summary>
	/// 目录
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

		/// <summary>遍历目录及其子目录</summary>
		/// <param name="path">起始路径</param>
		/// <param name="copyPath">复制路径</param>
		public static void CopyTo(string path, string copyPath){

			if(DirExists(path)){
				
				CopyTo(path , path , copyPath);
			
			}

		}

		/// <summary>
		/// 创建文件夹
		/// </summary>
		public static void CreateDir(string path , Operations operation = Operations.EXISTS_TO_RETURN)
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
		public static bool DirExists(string path){

			DirectoryInfo dir = new DirectoryInfo(path);
			return dir.Exists;

		}

		/// <summary>
		/// 遍历文件夹中的所有文件
		/// </summary>
        public static FileInfo[] Walk(string path)
        {
			List<FileInfo> returnList = new List<FileInfo>();
			if(!DirExists(path)){ return returnList.ToArray(); }
            DirectoryInfo folder = new DirectoryInfo(path);
            FileSystemInfo[] files = folder.GetFileSystemInfos();
            int length = files.Length;
            for (int i = 0; i < length; i++)
            {

                if (files[i] is DirectoryInfo)
                {

                    returnList.AddRange(Walk(files[i].FullName));

                }
                else
                {

                    returnList.Add(new FileInfo(files[i].FullName));

                }

            }
            return returnList.ToArray();
        }

		/// <summary>
		/// 遍历文件夹中的所有文件
		/// </summary>
		public static void Walk(string path , Action<FileInfo> callBack){

            DirectoryInfo folder = new DirectoryInfo(path);
			FileSystemInfo[] files = folder.GetFileSystemInfos();
			int length = files.Length;
			for (int i = 0; i < length; i++) {

				if(files[i] is DirectoryInfo){

					Walk(files[i].FullName , callBack);

				}else{

					callBack(new FileInfo(files[i].FullName));

				}

			}

		}

		/// <summary>遍历目录及其子目录</summary>
		/// <param name="root">起始结点</param>
		/// <param name="path">起始路径</param>
		/// <param name="copyPath">复制路径</param>
		private static void CopyTo(string root, string path, string copyPath)
		{
			root = root.Replace('\\', '/');
			path = path.Replace('\\', '/');
			copyPath = copyPath.Replace('\\', '/');

			string[] names = Directory.GetFiles(path);
			string[] dirs = Directory.GetDirectories(path);
			
			foreach (string filename in names)
			{
				string name = filename.Replace('\\', '/');
				string targetPath = copyPath + name.Replace(root, string.Empty);
				if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
				}
				File.Copy(filename, targetPath);
			}
			foreach (string dir in dirs)
			{
				CopyTo(root, dir, copyPath);
			}
		}

	}

}
