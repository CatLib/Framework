
using CatLib.API;
using CatLib.API.Crypt;

namespace CatLib.Crypt{


	public class Crypt : ICrypt{

        private Configs config;

        [Dependency]
        public Configs Config
        {
            get { return config; }
            set
            {
                config = value;
                if (config == null){ return; }
                key = config.Get<string>("key", string.Empty);
                if (key.Length != 32)
                {
                    key = null;
                }
            }
        }
        private HMacAes256 hMacAes256 = new HMacAes256();

        private string key;

        public string Encrypt(string toEncrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new System.Exception("crypt key is invalid");
            }
            return hMacAes256.Encrypt(toEncrypt , key);

        }

        public string Decrypt(string toDecrypt)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new System.Exception("crypt key is invalid");
            }
            return hMacAes256.Decrypt(toDecrypt, key);

        }


    }

}