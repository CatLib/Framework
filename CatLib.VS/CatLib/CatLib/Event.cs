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
using System.Collections.Generic;
using CatLib.API;
using CatLib.Stl;

namespace CatLib
{
    /// <summary>
    /// 事件实现
    /// </summary>
    public sealed class Event : IEvent
    {
        /// <summary>
        /// 应用程序
        /// </summary>
        private IApplication App { get; set; }

        /// <summary>
        /// 事件地图
        /// </summary>
        private Dictionary<string, List<EventHandler>> handlers;

        /// <summary>
        /// 构造一个事件实现
        /// </summary>
        /// <param name="app">应用程序</param>
        public Event(IApplication app)
        {
            App = app;
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="e">事件参数</param>
        public void Trigger(string eventName, EventArgs e)
        {
            Trigger(eventName, null, e);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        public void Trigger(string eventName, object sender = null, EventArgs e = null)
        {
            Guard.NotEmptyOrNull(eventName, "eventName");

            e = e ?? EventArgs.Empty;

            if (!App.IsMainThread)
            {
                App.MainThread(() =>
                {
                    Trigger(eventName, sender, e);
                });
                return;
            }

            if (handlers == null)
            {
                return;
            }

            if (!handlers.ContainsKey(eventName) || handlers[eventName].Count <= 0)
            {
                return;
            }

            CallEvent(handlers[eventName], sender, e);
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, System.EventHandler handler, int life = 0)
        {
            Guard.NotEmptyOrNull(eventName, "eventName");
            Guard.NotNull(handler, "handler");
            Guard.Requires<ArgumentOutOfRangeException>(life >= 0);

            var callHandler = new EventHandler(this, eventName, handler, life);
            if (!App.IsMainThread)
            {
                App.MainThread(() =>
                {
                    On(eventName, callHandler);
                });
                return callHandler;
            }
            On(eventName, callHandler);
            return callHandler;
        }

        /// <summary>
        /// 注册一个事件，调用一次后就释放
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件句柄</param>
        /// <returns></returns>
        public IEventHandler One(string eventName, System.EventHandler handler)
        {
            return On(eventName, handler, 1);
        }

        /// <summary>
        /// 移除一个事件
        /// </summary>
        /// <param name="handler">操作句柄</param>
        internal void Off(EventHandler handler)
        {
            Guard.NotNull(handler, "handler");

            if (!App.IsMainThread)
            {
                App.MainThread(() =>
                {
                    Off(handler);
                });
                return;
            }

            if (!handlers.ContainsKey(handler.EventName))
            {
                return;
            }

            handlers[handler.EventName].Remove(handler);
            if (handlers[handler.EventName].Count <= 0)
            {
                handlers.Remove(handler.EventName);
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件句柄</param>
        private void On(string eventName, EventHandler handler)
        {
            if (handlers == null)
            {
                handlers = new Dictionary<string, List<EventHandler>>();
            }
            if (!handlers.ContainsKey(eventName))
            {
                handlers.Add(eventName, new List<EventHandler>());
            }
            handlers[eventName].Add(handler);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="handler">事件</param>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void CallEvent(IList<EventHandler> handler, object sender, EventArgs e)
        {
            List<EventHandler> removeList = null;
            for (var i = 0; i < handler.Count; i++)
            {
                handler[i].Call(sender, e);
                if (handler[i].IsLife)
                {
                    continue;
                }
                if (removeList == null)
                {
                    removeList = new List<EventHandler>();
                }
                removeList.Add(handler[i]);
            }

            if (removeList == null)
            {
                return;
            }

            for (var i = 0; i < removeList.Count; i++)
            {
                removeList[i].Cancel();
            }
        }
    }
}