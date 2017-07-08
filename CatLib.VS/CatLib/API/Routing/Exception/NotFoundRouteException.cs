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
    /// 未能找到路由条目
    /// </summary>
    public sealed class NotFoundRouteException : RuntimeException
    {
        /// <summary>
        /// 未能找到路由条目
        /// </summary>
        /// <param name="message">异常消息</param>
        public NotFoundRouteException(string message) : base(message)
        {
        }
    }
}