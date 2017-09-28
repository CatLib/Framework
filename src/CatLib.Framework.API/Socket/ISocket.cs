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

namespace CatLib.API.Socket
{
    /// <summary>
    /// 套接字服务
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// 是否已经链接远端主机
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// 建立链接
        /// </summary>
        /// <returns>异步等待接口</returns>
        IAwait Connect();

        /// <summary>
        /// 建立链接
        /// </summary>
        /// <param name="hostname">服务器地址</param>
        /// <param name="port">服务器端口</param>
        /// <returns>异步等待接口</returns>
        IAwait Connect(string hostname, int port);

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="data">发送数据</param>
        /// <returns>异步等待接口</returns>
        IAwait Send(byte[] data);

        /// <summary>
        /// 断开链接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 注册一条事件
        /// </summary>
        /// <param name="socketEvent">Socket事件</param>
        /// <param name="callback">回调函数</param>
        void On(SocketEvents socketEvent, Action<object> callback);

        /// <summary>
        /// 反注册一条事件
        /// </summary>
        /// <param name="socketEvent">Socket事件</param>
        /// <param name="callback">回调函数</param>
        void Off(SocketEvents socketEvent, Action<object> callback);

        /// <summary>
        /// 触发一个Socket事件
        /// </summary>
        /// <param name="socketEvent">Socket事件</param>
        /// <param name="payload">载荷</param>
        void Trigger(SocketEvents socketEvent, object payload);
    }
}
