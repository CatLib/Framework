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
using CatLib.API.Event;

namespace CatLib.Event
{
    /// <summary>
    /// 事件句柄
    /// </summary>
    internal sealed class EventHandler : IEventHandler
    {
        /// <summary>
        /// 监听对象
        /// </summary>
        private EventImpl Target { get; set; }

        /// <summary>
        /// 事件句柄
        /// </summary>
        private System.EventHandler Handler { get; set; }

        /// <summary>
        /// 事件名
        /// </summary>
        private string EventName { get; set; }

        /// <summary>
        /// 是否取消事件
        /// </summary>
        private bool isCancel;

        /// <summary>
        /// 剩余的调用次数
        /// </summary>
        public int Life { get; private set; }

        /// <summary>
        /// 事件是否是有效的
        /// </summary>
        public bool IsLife { get; private set; }

        /// <summary>
        /// 创建一个事件句柄
        /// </summary>
        /// <param name="target">事件监听对象</param>
        /// <param name="eventName">事件名</param>
        /// <param name="eventHandler">事件句柄</param>
        /// <param name="life">生命次数</param>
        public EventHandler(EventImpl target, string eventName, System.EventHandler eventHandler, int life)
        {
            life = Math.Max(0, life);
            Handler = eventHandler;
            Target = target;
            EventName = eventName;
            Life = life;
            IsLife = true;
            isCancel = false;
        }

        /// <summary>
        /// 撤销事件监听
        /// </summary>
        /// <returns>是否撤销成功</returns>
        public bool Cancel()
        {
            if (isCancel)
            {
                return false;
            }
            if (Target != null)
            {
                Target.Off(EventName, this);
            }
            isCancel = true;
            return true;
        }

        /// <summary>
        /// 激活事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        internal void Call(object sender, EventArgs e)
        {
            if (Handler == null)
            {
                IsLife = false;
                return;
            }

            if (!IsLife)
            {
                return;
            }

            if (Life > 0)
            {
                if (--Life <= 0)
                {
                    IsLife = false;
                }
            }

            Handler.Invoke(sender, e);
        }
    }
}