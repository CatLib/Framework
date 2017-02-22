

using CatLib.Contracts.Secret;

namespace CattLib.Secret{

	/// <summary>
	/// Hash
	/// </summary>
	public class Hash : IHash{ 

		private string generateSalt;

		public void Bcrypt(string password){

			if(generateSalt == null){

				generateSalt = BCrypt.Net.BCrypt.GenerateSalt(10);

			}
			BCrypt.Net.BCrypt.HashPassword(password);

		}

		public bool BcryptVerify(string text, string hash){

			return BCrypt.Net.BCrypt.Verify(text , hash);

		}



	}

}