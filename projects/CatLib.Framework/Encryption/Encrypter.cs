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
using CatLib.API.Encryption;

namespace CatLib.Encryption
{
    /// <summary>
    /// 加解密
    /// </summary>
    public sealed class Encrypter : IEncrypter
    {
        /// <summary>
        /// 加解密实现
        /// </summary>
        private readonly AesEncrypter encrypter;

        /// <summary>
        /// 密钥
        /// </summary>
        private readonly byte[] key;

        /// <summary>
        /// 加解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="cipher">加密类型</param>
        public Encrypter(byte[] key, string cipher)
        {
            Guard.Requires<ArgumentNullException>(key != null);
            this.key = key;
            if (!Supported(key, cipher))
            {
                throw new RuntimeException("The only supported ciphers are AES-128-CBC and AES-256-CBC with the correct key lengths.");
            }
            encrypter = MakeEncrypter(cipher);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">加密数据</param>
        /// <returns>加密后的数据</returns>
        public string Encrypt(byte[] content)
        {
            Guard.Requires<ArgumentNullException>(content != null);
            return encrypter.Encrypt(content, key);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="payload">被加密的内容</param>
        /// <returns>解密内容</returns>
        public byte[] Decrypt(string payload)
        {
            Guard.NotEmptyOrNull(payload, "payload");
            return encrypter.Decrypt(payload, key);
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public byte[] GetKey()
        {
            return key;
        }

        /// <summary>
        /// 是否支持
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="cipher">加密类型</param>
        /// <returns>是否支持</returns>
        public static bool Supported(byte[] key, string cipher)
        {
            return (cipher == "AES-128-CBC" && key.Length == 16) ||
                   (cipher == "AES-256-CBC" && key.Length == 32);
        }

        /// <summary>
        /// 根据加密方式生成加密器
        /// </summary>
        /// <param name="cipher">加密方式</param>
        private AesEncrypter MakeEncrypter(string cipher)
        {
            return new AesEncrypter(cipher == "AES-128-CBC" ? 128 : 256);
        }
    }
}
