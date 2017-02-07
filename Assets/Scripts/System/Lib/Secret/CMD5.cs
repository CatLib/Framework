using System.IO;
using System.Security.Cryptography;

namespace CatLib.Secret{

	public static class CMD5 {

		public static string ParseFile(FileInfo file){

			return CMD5.ParseFile(file.FullName);

		}

		public static string ParseFile(string path){
			
			string fileMD5 = null;
			using (var fileStream = File.OpenRead(path))
			{
				var md5 = MD5.Create();
				var fileMD5Bytes = md5.ComputeHash(fileStream);                                  
				fileMD5 = System.BitConverter.ToString(fileMD5Bytes).Replace("-", "");
			}

			return fileMD5.ToUpper();

		}

	}

}