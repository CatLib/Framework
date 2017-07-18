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
using System.Text.RegularExpressions;
using CatLib.API.Events;
using CatLib.Stl;

namespace CatLib.Events
{
    /// <summary>
    /// 事件调度器
    /// </summary>
    public sealed class Dispatcher : IDispatcher
    {
        /// <summary>
        /// 事件句柄
        /// </summary>
        private readonly Dictionary<string, List<EventHandler>> handlers;

        /// <summary>
        /// 通配符事件句柄
        /// </summary>
        private readonly Dictionary<Regex, List<EventHandler>> wildcardHandlers;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot;

        /// <summary>
        /// 调度器
        /// </summary>
        public Dispatcher()
        {
            handlers = new Dictionary<string, List<EventHandler>>();
            wildcardHandlers = new Dictionary<Regex, List<EventHandler>>();
            syncRoot = new object();
        }

        /// <summary>
        /// 触发一个事件,并获取事件的返回结果
        /// <para>如果<paramref name="halt"/>为<c>true</c>那么返回的结果是事件的返回结果,没有一个事件进行处理的话返回<c>null</c>
        /// 反之返回一个事件处理结果数组(<c>object[]</c>)</para>
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="payload">载荷</param>
        /// <param name="halt">是否只触发一次就终止</param>
        /// <returns>事件结果</returns>
        public object Trigger(string eventName, object payload = null , bool halt = false)
        {
            Guard.Requires<ArgumentNullException>(eventName != null);
            eventName = Normalize(eventName);

            lock (syncRoot)
            {
                var listeners = GetListeners(eventName);
                var outputs = new object[listeners.Count];
                var triggerListener = new List<EventHandler>(listeners.Count);

                var i = 0;
                foreach (var listener in listeners)
                {
                    outputs[i++] = listener.Trigger(payload);
                    triggerListener.Add(listener);
                    if (halt)
                    {
                        break;
                    }
                }

                foreach (var listener in triggerListener)
                {
                    if (!listener.IsLife)
                    {
                        listener.Cancel();
                    }
                }

                return halt ? outputs.Length <= 0 ? null : outputs[0] : outputs;
            }
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, Action<object> handler, int life = 0)
        {
            Guard.Requires<ArgumentNullException>(handler != null);
            return On(eventName, (payload) =>
            {
                handler.Invoke(payload);
                return null;
            }, life);
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, Func<object, object> handler, int life = 0)
        {
            Guard.Requires<ArgumentNullException>(eventName != null);
            Guard.Requires<ArgumentNullException>(handler != null);

            eventName = Normalize(eventName);

            var wildcard = eventName.IndexOf("*") != -1;
            var eventHandler = new EventHandler(this, wildcard ? Str.AsteriskWildcard(eventName) : eventName, handler, life, wildcard);

            lock (syncRoot)
            {
                if (wildcard)
                {
                    SetWildcardListener(eventHandler);
                }
                else
                {
                    List<EventHandler> handlers;
                    if (!this.handlers.TryGetValue(eventName, out handlers))
                    {
                        this.handlers[eventName] = handlers = new List<EventHandler>();
                    }
                    handlers.Add(eventHandler);
                }

                return eventHandler;
            }
        }

        /// <summary>
        /// 移除一个事件
        /// </summary>
        /// <param name="handler">事件句柄</param>
        internal void Off(EventHandler handler)
        {
            lock (syncRoot)
            {
                List<EventHandler> result;
                if (handlers.TryGetValue(handler.EventName, out result))
                {
                    result.Remove(handler);
                    if (result.Count <= 0)
                    {
                        handlers.Remove(handler.EventName);
                    }
                }

                Regex wildcardkey = null;
                foreach (var element in wildcardHandlers)
                {
                    if (element.Key.ToString() == handler.EventName)
                    {
                        element.Value.Remove(handler);
                        wildcardkey = element.Key;
                        result = element.Value;
                        break;
                    }
                }
                if (wildcardkey != null && result.Count <= 0)
                {
                    wildcardHandlers.Remove(wildcardkey);
                }
            }
        }

        /// <summary>
        /// 设定通配符监听
        /// </summary>
        /// <param name="handler">监听句柄</param>
        private void SetWildcardListener(EventHandler handler)
        {
            List<EventHandler> handlers = null;
            foreach (var element in wildcardHandlers)
            {
                if (element.Key.ToString() == handler.EventName)
                {
                    handlers = element.Value;
                    break;
                }
            }

            if (handlers == null)
            {
                wildcardHandlers[new Regex(handler.EventName)] = handlers = new List<EventHandler>();
            }

            handlers.Add(handler);
        }

        /// <summary>
        /// 获取指定事件的事件句柄列表
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>句柄列表</returns>
        private IList<EventHandler> GetListeners(string eventName)
        {
            var outputs = new List<EventHandler>();

            List<EventHandler> result;
            if (handlers.TryGetValue(eventName, out result))
            {
                outputs.AddRange(result);
            }

            foreach (var element in wildcardHandlers)
            {
                if (element.Key.IsMatch(eventName))
                {
                    outputs.AddRange(element.Value);
                }
            }

            return outputs;
        }

        /// <summary>
        /// 标准化字符串
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>输出</returns>
        private string Normalize(string input)
        {
            return input.ToLower();
        }
    }
}
