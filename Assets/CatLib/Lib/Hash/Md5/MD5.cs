/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CatLib.Hash
{

    public static class MD5
    {

        public static string ParseFile(FileInfo file)
        {

            return ParseFile(file.FullName);

        }

        public static string ParseFile(string path)
        {

            int bufferSize = 1024 * 16;
            byte[] buffer = new byte[bufferSize];
            string fileMD5 = null;
            using (var fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
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


        public static string ParseString(string input)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

    }

}