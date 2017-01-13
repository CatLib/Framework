using System.IO;

namespace CatLib.FileSystem{

	/// <summary>
	/// 文件
	/// </summary>
	public static class CFile{

		/// <summary>
		/// 标准化路径
		/// </summary>
		public static string Standard(this FileSystemInfo file){

			return file.FullName.Standard();

		}

		/// <summary>
		/// 标准化路径
		/// </summary>
		public static string Standard(this string path){

			return path.Replace("\\","/");

		}

		/// <summary>
		/// 文件大小
		/// </summary>
		public static long Length(this FileSystemInfo file){

			if(file is DirectoryInfo){

				return 0;
				
			}

			return (new FileInfo(file.FullName)).Length;

		}

	}

}