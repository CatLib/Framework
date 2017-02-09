using System.IO;

namespace CatLib.FileSystem{

	/// <summary>
	/// 文件
	/// </summary>
	public static class CFile{
		
		/// <summary>
		/// 删除指定文件
		/// </summary>
        public static void Delete(string path)
        {
            File.Delete(path);
        }

		/// <summary>
		/// 文件是否存在
		/// </summary>
        public static bool Exists(string path)
        {

            return File.Exists(path);

        }

		/// <summary>
		/// 覆盖文件
		/// </summary>
        public static void Cover(string path , byte[] array, int offset, int count)
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Write(array, offset, count);
                fs.Close();
            }
        }

		/// <summary>
		/// 标准化路径
		/// </summary>
		public static string Standard(this FileSystemInfo file){

			return Standard(file.FullName);

		}

		/// <summary>
		/// 标准化路径
		/// </summary>
		public static string Standard(string path){

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