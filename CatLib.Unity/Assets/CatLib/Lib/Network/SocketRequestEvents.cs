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
    /// Socket请求事件
    /// </summary>
    public class SocketRequestEvents
    {
        /// <summary>
        /// 当连接时
        /// </summary>
        public static readonly string ON_CONNECT = "network.socket.connector.connect.";

        /// <summary>
        /// 当连接关闭时
        /// </summary>
        public static readonly string ON_CLOSE = "network.socket.connector.close.";

        /// <summary>
        /// 当出现错误时
        /// </summary>
        public static readonly string ON_ERROR = "network.socket.connector.error.";

        /// <summary>
        /// 当收到服务器的消息时
        /// </summary>
        public static readonly string ON_MESSAGE = "network.socket.connector.message.";
    }
}