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

namespace CatLib.Encryption.Aes
{
    /// <summary>
    /// Aes类
    /// </summary>
    public sealed class Aes
    {
        /// <summary>
        /// RijndaelManaged
        /// </summary>
        private readonly RijndaelManaged rijndaelManaged;

        /// <summary>
        /// Aes基础类
        /// </summary>
        /// <param name="size">密钥长度</param>
        /// <param name="mode">密码模式</param>
        /// <param name="padding">填充模式</param>
        public Aes(int size = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            rijndaelManaged = new RijndaelManaged
            {
                KeySize = size,
                BlockSize = size,
                Padding = padding,
                Mode = mode,
            };
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] content, byte[] key)
        {
            rijndaelManaged.Key = key;
            rijndaelManaged.GenerateIV();

            var aesEncrypt = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);

            var aesBufferString = Convert.ToBase64String(aesEncrypt.TransformFinalBlock(content, 0, content.Length));
            var ivString = Convert.ToBase64String(rijndaelManaged.IV);

            return null;
        }

        /// <summary>
        /// 计算HMac
        /// </summary>
        /// <param name="content">需要计算HMac的数据</param>
        /// <param name="key">密钥</param>
        /// <returns>HMac值</returns>
        private byte[] HMac(byte[] content, byte[] key)
        {
            var hmacSha256 = new HMACSHA256(key);
            return hmacSha256.ComputeHash(content);
        }
    }
}
