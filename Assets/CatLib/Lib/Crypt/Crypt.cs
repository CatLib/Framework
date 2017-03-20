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