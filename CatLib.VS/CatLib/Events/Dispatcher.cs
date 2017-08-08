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

using CatLib.API.Events;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        /// 跳出标记
        /// </summary>
        private readonly object breakFlag = false;

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
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="payload">载荷</param>
        /// <returns>事件结果</returns>
        public object[] Trigger(string eventName, object payload = null)
        {
            return Dispatch(eventName, payload) as object[];
        }

        /// <summary>
        /// 触发一个事件,遇到第一个事件存在处理结果后终止,并获取事件的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="payload">载荷</param>
        /// <returns>事件结果</returns>
        public object TriggerHalt(string eventName, object payload = null)
        {
            return Dispatch(eventName, payload, true);
        }

        /// <summary>
        /// 调度事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="payload">载荷</param>
        /// <param name="halt">遇到第一个事件存在处理结果后终止</param>
        /// <returns>处理结果</returns>
        private object Dispatch(string eventName, object payload = null, bool halt = false)
        {
            Guard.Requires<ArgumentNullException>(eventName != null);
            eventName = Normalize(eventName);

            lock (syncRoot)
            {
                var listeners = GetListeners(eventName);
                var outputs = new List<object>(listeners.Count);
                var triggerListener = new List<EventHandler>(listeners.Count);

                foreach (var listener in listeners)
                {
                    var result = listener.Trigger(payload);
                    triggerListener.Add(listener);

                    if (halt && result != null)
                    {
                        outputs.Add(result);
                        break;
                    }

                    if (result != null && result.Equals(breakFlag))
                    {
                        break;
                    }

                    outputs.Add(result);
                }

                foreach (var listener in triggerListener)
                {
                    if (!listener.IsLife)
                    {
                        listener.Cancel();
                    }
                }

                return halt ? outputs.Count <= 0 ? null : outputs[Math.Max(0, outputs.Count - 1)] : outputs.ToArray();
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
            return Listen(eventName, (payload) =>
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
        public IEventHandler Listen(string eventName, Func<object, object> handler, int life = 0)
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
