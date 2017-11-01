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

using CatLib.API.Encryption;
using Elliptic;
using System;
using System.Security.Cryptography;

namespace CatLib.Encryption
{
    /// <summary>
    /// Curve25519 Diffie-Hellman
    /// </summary>
    public sealed class Curve25519Encrypter : IEncrypter
    {
        /// <summary>
        /// 私有Key
        /// </summary>
        private byte[] privateKey;

        /// <summary>
        /// 公钥
        /// </summary>
        private byte[] publicKey;

        /// <summary>
        /// Curve25519 Diffie-Hellman
        /// </summary>
        public Curve25519Encrypter()
        {
            var randBytes = new byte[32];
            RandomNumberGenerator.Create().GetBytes(randBytes);
            GenKey(randBytes);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">加密数据</param>
        /// <returns>加密后的数据</returns>
        public byte[] Encode(byte[] content)
        {
            if (content != null && content.Length == 32)
            {
                GenKey(content);
            }
            return publicKey;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="payload">被加密的内容</param>
        /// <returns>解密内容</returns>
        public byte[] Decode(byte[] payload)
        {
            Guard.Requires<ArgumentNullException>(payload != null);
            return Curve25519.GetSharedSecret(privateKey, payload);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">加密数据</param>
        /// <returns>加密后的数据</returns>
        public string Encrypt(byte[] content)
        {
            return Convert.ToBase64String(Encode(content));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="payload">被加密的内容</param>
        /// <returns>解密内容</returns>
        public byte[] Decrypt(string payload)
        {
            Guard.NotEmptyOrNull(payload, "payload");
            return Decode(Convert.FromBase64String(payload));
        }

        /// <summary>
        /// 生成密钥
        /// </summary>
        /// <param name="data">参数</param>
        private void GenKey(byte[] data)
        {
            privateKey = Curve25519.ClampPrivateKey(data);
            publicKey = Curve25519.GetPublicKey(privateKey);
        }
    }
}
