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

namespace CatLib.API.Socket
{
    /// <summary>
    /// 套接字事件
    /// </summary>
    public class SocketEvents : Enum
    {
        /// <summary>
        /// 消息
        /// </summary>
        private const string EventConnect = "SocketEvents.Connect";

        /// <summary>
        /// 断开链接
        /// </summary>
        private const string EventDiconnect = "SocketEvents.Disconnect";

        /// <summary>
        /// 消息
        /// </summary>
        private const string EventMessage = "SocketEvents.Message";

        /// <summary>
        /// 当发送完成
        /// </summary>
        private const string EventSent = "SocketEvents.Sent";

        /// <summary>
        /// 关闭后
        /// </summary>
        private const string EventClosed = "SocketEvents.Closed";

        /// <summary>
        /// 关闭后
        /// </summary>
        private const string EventError = "SocketEvents.Error";

        /// <summary>
        /// 当链接成功时
        /// </summary>
        public static readonly SocketEvents Connect = new SocketEvents(EventConnect);

        /// <summary>
        /// 断开链接时
        /// </summary>
        public static readonly SocketEvents Disconnect = new SocketEvents(EventDiconnect);

        /// <summary>
        /// 当收到消息时
        /// </summary>
        public static readonly SocketEvents Message = new SocketEvents(EventMessage);

        /// <summary>
        /// 当发送完成时
        /// </summary>
        public static readonly SocketEvents Sent = new SocketEvents(EventSent);

        /// <summary>
        /// 当关闭后
        /// </summary>
        public static readonly SocketEvents Closed = new SocketEvents(EventClosed);

        /// <summary>
        /// 当出现异常时
        /// </summary>
        public static readonly SocketEvents Error = new SocketEvents(EventError);

        /// <summary>
        /// 构造一个枚举
        /// </summary>
        protected SocketEvents(string name) : base(name)
        {
        }

        /// <summary>
        /// 字符串转RandomTypes
        /// </summary>
        /// <param name="type">类型</param>
        public static implicit operator SocketEvents(string type)
        {
            switch (type)
            {
                case EventMessage:
                    return Message;
                case EventClosed:
                    return Closed;
                case EventConnect:
                    return Connect;
                case EventDiconnect:
                    return Disconnect;
                case EventError:
                    return Error;
                case EventSent:
                    return Sent;
                default:
                    return new SocketEvents(type);
            }
        }
    }
}
