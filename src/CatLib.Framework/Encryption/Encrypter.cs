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
using System.Security.Cryptography;
using CatLib.API.Encryption;
using CatLib._3rd.Elliptic;

namespace CatLib.Encryption
{
    /// <summary>
    /// 加解密
    /// </summary>
    public sealed class Encrypter : SingleManager<IEncrypter> , IEncryptionManager
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">加密数据</param>
        /// <returns>加密后的数据</returns>
        public string Encrypt(byte[] content)
        {
            return Default.Encrypt(content);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="payload">被加密的内容</param>
        /// <returns>解密内容</returns>
        public byte[] Decrypt(string payload)
        {
            return Default.Decrypt(payload);
        }

        /// <summary>
        /// 交换密钥
        /// </summary>
        /// <param name="exchange">交换流程(输入值是我方公钥，返回值是对端公钥)</param>
        /// <returns>密钥</returns>
        public byte[] ExchangeSecret(Func<byte[], byte[]> exchange)
        {
            Guard.Requires<ArgumentNullException>(exchange != null);
            var randBytes = new byte[32];
            RandomNumberGenerator.Create().GetBytes(randBytes);
            var privateKey = Curve25519.ClampPrivateKey(randBytes);
            var publicKey = Curve25519.GetPublicKey(privateKey);
            return Curve25519.GetSharedSecret(privateKey, exchange.Invoke(publicKey));
        }
    }
}
