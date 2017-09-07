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
    /// 加解密异常
    /// </summary>
    public sealed class EncryptionException : RuntimeException
    {
        /// <summary>
        /// 加解密异常
        /// </summary>
        public EncryptionException() : base()
        {

        }

        /// <summary>
        /// 加解密异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public EncryptionException(string message) : base(message)
        {
        }

        /// <summary>
        /// 加解密异常
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public EncryptionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
