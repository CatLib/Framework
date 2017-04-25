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

namespace CatLib.Network
{
    /// <summary>
    /// Http请求全局事件
    /// </summary>
    public sealed class HttpRequestEvents
    {
        /// <summary>
        /// Http请求回应事件
        /// </summary>
        public static readonly string ON_MESSAGE = "network.http.connector.message.";
    }
}