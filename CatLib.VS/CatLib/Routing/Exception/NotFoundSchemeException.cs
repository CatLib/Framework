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

using CatLib.API;

namespace CatLib.Routing
{
    /// <summary>
    /// 未能找到 Scheme
    /// </summary>
    public sealed class NotFoundSchemeException : RuntimeException
    {
        /// <summary>
        /// 未能找到 Scheme
        /// </summary>
        /// <param name="message">异常消息</param>
        public NotFoundSchemeException(string message) : base(message) { }
    }
}