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

namespace CatLib.API.Encryption
{
    /// <summary>
    /// 加解密
    /// </summary>
    public interface IEncrypter
    {
        /// <summary>
        /// 加密(以Base64返回)
        /// </summary>
        /// <param name="content">加密数据</param>
        /// <returns>加密后的数据</returns>
        string Encrypt(byte[] content);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="payload">被加密的内容</param>
        /// <returns>解密内容</returns>
        byte[] Decrypt(string payload);
    }
}
