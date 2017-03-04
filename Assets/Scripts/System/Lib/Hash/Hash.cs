using CatLib.API;
using CatLib.API.Hash;

namespace CatLib.Hash{

	/// <summary>
	/// Hash
	/// </summary>
	public class Hash : IHash{ 

        [Dependency]
        public Configs Config { get; set; }

		private string generateSalt;

		public string Make(string password){

			if(generateSalt == null){

                if (Config != null)
                {
                    if (Config.IsExists("salt"))
                    {
                        generateSalt = Config.Get<string>("salt");
                    }
                    else if (Config.IsExists("factor"))
                    {
                        generateSalt = BCrypt.Net.BCrypt.GenerateSalt(Config.Get<int>("factor"));
                    }
                }

                if (string.IsNullOrEmpty(generateSalt))
                {
                    generateSalt = BCrypt.Net.BCrypt.GenerateSalt(6);
                }

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