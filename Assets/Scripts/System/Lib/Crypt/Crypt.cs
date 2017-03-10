using CatLib.API.Crypt;

namespace CatLib.Crypt{


	public class Crypt : ICrypt{

        private HMacAes256 hMacAes256 = new HMacAes256();

        private string key;

        #region config

        public void SetKey(string key){

            this.key = key;
            if (this.key.Length != 32)
            {
                this.key = null;
            }

        }
        
        #endregion
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