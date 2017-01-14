using System.IO;

namespace CatLib.FileSystem{

	/// <summary>
	/// 文件
	/// </summary>
	public static class CFile{

        public static void Delete(this string path)
        {
            File.Delete(path);
        }

        public static bool Exists(this string path)
        {

            return File.Exists(path);

        }

        public static void Cover(this string path , byte[] array, int offset, int count)
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