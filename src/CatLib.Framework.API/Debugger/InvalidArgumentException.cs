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

namespace CatLib.API.Debugger
{
    /// <summary>
    /// 无效的参数异常
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InvalidArgumentException : ArgumentException
    {
        /// <summary>
        /// 无效的参数
        /// </summary>
        public InvalidArgumentException()
            : base() {
        }

        /// <summary>
        /// 无效的参数
        /// </summary>
        /// <param name="message">异常消息</param>
        public InvalidArgumentException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// 无效的参数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">异常结构</param>
        public InvalidArgumentException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// 无效的参数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="paramName">诱发异常的参数</param>
        /// <param name="innerException">异常结构</param>
        public InvalidArgumentException(string message, string paramName, Exception innerException)
            : base(message , paramName, innerException)
        {
        }

        /// <summary>
        /// 无效的参数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="paramName">诱发异常的参数</param>
        public InvalidArgumentException(string message, string paramName)
            : base (message , paramName)
        {
        }
    }
}
