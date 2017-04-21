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

namespace CatLib.Crypt
{
    /// <summary>
    /// 加密适配器
    /// </summary>
    public interface ICryptAdapter
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的值</returns>
        string Encrypt(string str, string key);

        /// <summary>
        /// 解密被加密的内容
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的值</returns>
        string Decrypt(string str, string key);
    }
}
