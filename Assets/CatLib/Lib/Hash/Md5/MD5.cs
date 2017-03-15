using System;
using System.IO;
using System.Security.Cryptography;

namespace CatLib.Hash{

	public static class MD5 {

		public static string ParseFile(FileInfo file){

			return ParseFile(file.FullName);

		}

		public static string ParseFile(string path){

            int bufferSize = 1024 * 16;
            byte[] buffer = new byte[bufferSize];
            string fileMD5 = null;
			using (var fileStream  = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
                HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
                int readLength = 0;
                var output = new byte[bufferSize];
                while ((readLength = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
                }
                hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
                string md5 = BitConverter.ToString(hashAlgorithm.Hash);
                hashAlgorithm.Clear();
                fileMD5 = md5.Replace("-", "");
            }

			return fileMD5.ToUpper();

		}

	}

}