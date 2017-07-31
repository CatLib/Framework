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
using IEventHandler = CatLib.API.Events.IEventHandler;

namespace CatLib.Events
{
    /// <summary>
    /// 事件句柄
    /// </summary>
    internal sealed class EventHandler : IEventHandler
    {
        /// <summary>
        /// 剩余的调用次数
        /// </summary>
        public int Life { get; private set; }

        /// <summary>
        /// 事件是否是有效的
        /// </summary>
        public bool IsLife { get; private set; }

        /// <summary>
        /// 事件名
        /// </summary>
        internal string EventName { get; private set; }

        /// <summary>
        /// 是否使用了通配符
        /// </summary>
        internal bool IsWildcard { get; private set; }

        /// <summary>
        /// 调度器
        /// </summary>
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// 事件句柄
        /// </summary>
        private readonly Func<object, object> handler;

        /// <summary>
        /// 是否取消事件
        /// </summary>
        private bool isCancel;

        /// <summary>
        /// 调用计数
        /// </summary>
        private int count;

        /// <summary>
        /// 创建一个事件句柄
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">生命次数</param>
        /// <param name="wildcard">是否使用了通配符</param>
        internal EventHandler(Dispatcher dispatcher, string eventName, Func<object, object> handler, int life , bool wildcard)
        {
            this.dispatcher = dispatcher;
            this.handler = handler;

            EventName = eventName;
            Life = Math.Max(0, life);
            IsLife = true;
            IsWildcard = wildcard;

            isCancel = false;
            count = 0;
        }

        /// <summary>
        /// 撤销事件监听
        /// </summary>
        /// <returns>是否撤销成功</returns>
        public void Cancel()
        {
            if (count > 0 || isCancel)
            {
                return;
            }

            dispatcher.Off(this);
            isCancel = true;
        }

        /// <summary>
        /// 激活事件
        /// </summary>
        /// <param name="payload">载荷</param>
        internal object Trigger(object payload)
        {
            if (!IsLife)
            {
                return null;
            }

            if (Life > 0)
            {
                if (--Life <= 0)
                {
                    IsLife = false;
                }
            }

            count++;

            var result = handler.Invoke(payload);

            count--;

            Guard.Requires<AssertException>(count >= 0);

            return result;
        }
    }
}