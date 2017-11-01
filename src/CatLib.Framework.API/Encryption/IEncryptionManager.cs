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

namespace CatLib.API.Encryption
{
    /// <summary>
    /// 加解密管理器
    /// </summary>
    public interface IEncryptionManager : ISingleManager<IEncrypter> , IEncrypter
    {
        /// <summary>
        /// 交换密钥
        /// </summary>
        /// <param name="exchange">交换流程(输入值是我方公钥，返回值是对端公钥)</param>
        /// <returns>密钥</returns>
        byte[] ExchangeSecret(Func<byte[], byte[]> exchange);
    }
}
