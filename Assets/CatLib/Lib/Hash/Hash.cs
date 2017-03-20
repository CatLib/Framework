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
 
using CatLib.API.Hash;

namespace CatLib.Hash{

	/// <summary>
	/// Hash
	/// </summary>
	public class Hash : IHash{ 

		private string generateSalt;

        public void SetGenerateSalt(string salt){

            if(!string.IsNullOrEmpty(salt)){

                generateSalt = salt;

            }

        }

        public void SetFactor(int factor){

            generateSalt = BCrypt.Net.BCrypt.GenerateSalt(factor);

        }

		public string Make(string password){

			if (string.IsNullOrEmpty(generateSalt))
            {
                generateSalt = BCrypt.Net.BCrypt.GenerateSalt(6);
            }
			return BCrypt.Net.BCrypt.HashPassword(password, generateSalt);

		}

		public bool Check(string text, string hash){

			return BCrypt.Net.BCrypt.Verify(text , hash);

		}

        public string FileHash(string path)
        {
            return MD5.ParseFile(path);
        }
    }

}