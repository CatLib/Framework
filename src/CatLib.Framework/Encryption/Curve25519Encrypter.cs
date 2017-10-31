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
using System.Collections.Generic;

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
        private Queue<byte[]> privateKey;

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">加密数据</param>
        /// <returns>加密后的数据</returns>
        public string Encrypt(byte[] content)
        {
            var key = new byte[] { };
            privateKey.Enqueue(key = Curve25519.ClampPrivateKey(content));
            return Convert.ToBase64String(Curve25519.GetPublicKey(key));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="payload">被加密的内容</param>
        /// <returns>解密内容</returns>
        public byte[] Decrypt(string payload)
        {
            var data = Convert.FromBase64String(payload);
            return Curve25519.GetSharedSecret(privateKey.Dequeue(), data);
        }
    }
}
