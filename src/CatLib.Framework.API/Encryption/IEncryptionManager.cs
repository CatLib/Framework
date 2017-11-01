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
    /// 加解密管理器
    /// </summary>
    public interface IEncryptionManager : ISingleManager<IEncrypter> , IEncrypter
    {
        /// <summary>
        /// 生成交换密钥算法
        /// </summary>
        /// <param name="name">私钥存储名字</param>
        /// <param name="randValue">随机值</param>
        /// <returns>私钥</returns>
        byte[] GenDiffieHellman(string name, byte[] randValue = null);
        
        /// <summary>
        /// 生成密钥
        /// </summary>
        /// <param name="name"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        byte[] MakeSharedSecret(string name, byte[] publicKey);
    }
}
