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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 未定义默认的Scheme
    /// </summary>
    public sealed class UndefinedDefaultSchemeException : RuntimeException
    {
        /// <summary>
        /// 未定义默认的Scheme
        /// </summary>
        /// <param name="message">异常消息</param>
        public UndefinedDefaultSchemeException(string message) : base(message)
        {
        }
    }
}