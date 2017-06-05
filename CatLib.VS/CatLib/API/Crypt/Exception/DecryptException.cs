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

namespace CatLib.API.Crypt
{
    /// <summary>
    /// 解析异常
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DecryptException : RuntimeException
    {
        /// <summary>
        /// 解析异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public DecryptException(string message) : base(message) { }
    }
}