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

namespace CatLib.Crypt
{   
    /// <summary>
    /// 加密
    /// </summary>
    public sealed class Crypt : ICrypt
    {
        /// <summary>
        /// 加密算法
        /// </summary>
        private ICryptAdapter adapter;

        /// <summary>
        /// 密钥
        /// </summary>
        private string key;

        /// <summary>
        /// 设定加密适配器
        /// </summary>
        /// <param name="adapter">适配器</param>
        public void SetAdapter(ICryptAdapter adapter)
        {
            this.adapter = adapter;
        }

        /// <summary>
        /// 设定密钥
        /// </summary>
        /// <param name="key">32位密钥</param>
        public void SetKey(string key)
        {
            this.key = key;
            if (this.key.Length != 32)
            {
                this.key = null;
            }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str">需要被加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new System.Exception("crypt key is invalid");
            }
            if (adapter == null)
            {
                throw new System.Exception("undefined crypt adapter");
            }
            return adapter.Encrypt(str, key);
        }

        /// <summary>
        /// 解密被加密的字符串
        /// </summary>
        /// <param name="str">需要被解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new System.Exception("crypt key is invalid");
            }
            if (adapter == null)
            {
                throw new System.Exception("undefined crypt adapter");
            }
            return adapter.Decrypt(str, key);
        }
    }
}