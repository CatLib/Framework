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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CatLib.API.Crypt;

namespace CatLib.Crypt
{
    /// <summary>
    /// HMacAes256加解密
    /// </summary>
    internal sealed class HMacAes256 : ICryptAdapter
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的值</returns>
        public string Encrypt(string str, string key)
        {
            var aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 256,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = Encoding.UTF8.GetBytes(key)
            };
            aes.GenerateIV();

            var aesEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            var buffer = Encoding.UTF8.GetBytes(str);
            buffer = aesEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);

            var aesBufferString = Convert.ToBase64String(buffer);
            var ivString = Convert.ToBase64String(aes.IV);
            var aesIvString = Convert.ToBase64String(Encoding.UTF8.GetBytes(aesBufferString + "$" + ivString));

            return aesIvString + "$" + HMac(aesIvString, key);
        }

        /// <summary>
        /// 解密被加密的内容
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的值</returns>
        public string Decrypt(string str, string key)
        {
            var hmac = str.Split('$');
            if (hmac.Length < 2)
            {
                throw new DecryptException("mac is invalid");
            }
            if (HMac(hmac[0], key) != hmac[1])
            {
                throw new DecryptException("mac is invalid");
            }

            var aesIvString = Encoding.UTF8.GetString(Convert.FromBase64String(hmac[0]));
            var aesIvStringArr = aesIvString.Split('$');
            if (aesIvStringArr.Length < 2)
            {
                throw new DecryptException("mac is invalid");
            }

            var aesBuffer = Convert.FromBase64String(aesIvStringArr[0]);
            var ivBuffer = Convert.FromBase64String(aesIvStringArr[1]);

            var aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 256,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = Encoding.UTF8.GetBytes(key),
                IV = ivBuffer
            };

            var aesDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            aesBuffer = aesDecrypt.TransformFinalBlock(aesBuffer, 0, aesBuffer.Length);

            return Encoding.UTF8.GetString(aesBuffer);
        }

        /// <summary>
        /// 计算HMac
        /// </summary>
        /// <param name="str">需要计算HMac的值</param>
        /// <param name="key">密钥</param>
        /// <returns>HMac值</returns>
        private string HMac(string str, string key)
        {
            var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return HashAlgorithmBase(hmacSha256, str, Encoding.UTF8);
        }

        /// <summary>
        /// 字节流转二进制字符串
        /// </summary>
        /// <param name="source">字节流</param>
        /// <param name="formatStr">格式化格式</param>
        /// <returns>转换后的字符串</returns>
        private string Bytes2Str(IEnumerable<byte> source, string formatStr = "{0:X2}")
        {
            var pwd = new StringBuilder();
            foreach (var btStr in source)
            {
                pwd.AppendFormat(formatStr, btStr);
            }
            return pwd.ToString();
        }

        /// <summary>
        /// 计算Hash
        /// </summary>
        /// <param name="hashAlgorithmObj">hash算法对象</param>
        /// <param name="source">需要哈希的源</param>
        /// <param name="encoding">编码</param>
        /// <returns>计算的hash值</returns>
        private string HashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
        {
            var btStr = encoding.GetBytes(source);
            var hashStr = hashAlgorithmObj.ComputeHash(btStr);
            return Bytes2Str(hashStr);
        }
    }
}

