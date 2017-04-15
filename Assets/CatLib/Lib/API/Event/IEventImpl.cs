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

namespace CatLib.API.Event
{
    /// <summary>
    /// 事件实现
    /// </summary>
    public interface IEventImpl
    {
        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        void Trigger(string eventName);

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="e">事件参数</param>
        void Trigger(string eventName, EventArgs e);

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="sender">发送者</param>
        void Trigger(string eventName, object sender);

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        void Trigger(string eventName, object sender, EventArgs e);

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        IEventHandler On(string eventName, EventHandler handler, int life = -1);

        /// <summary>
        /// 注册一个事件，调用一次后就释放
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件句柄</param>
        /// <returns></returns>
        IEventHandler One(string eventName, EventHandler handler);

        /// <summary>
        /// 移除一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">操作句柄</param>
        void Off(string eventName, IEventHandler handler);
    }
}