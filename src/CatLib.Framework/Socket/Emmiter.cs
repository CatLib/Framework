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

namespace CatLib.Socket
{
    /// <summary>
    /// 发射器
    /// </summary>
    internal sealed class Emmiter
    {
        /// <summary>
        /// 发射器
        /// </summary>
        private readonly IDictionary<string, List<Action<object>>> emmiters;

        /// <summary>
        /// 构建一个发射器
        /// </summary>
        public Emmiter()
        {
            emmiters = new Dictionary<string, List<Action<object>>>();
        }

        /// <summary>
        /// 注册一条事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="callback">回调</param>]
        public void On(string eventName, Action<object> callback)
        {
            Guard.Requires<ArgumentNullException>(eventName != null);
            Guard.Requires<ArgumentNullException>(callback != null);
            List<Action<object>> emmiter;
            if (!emmiters.TryGetValue(eventName, out emmiter))
            {
                emmiters[eventName] = emmiter = new List<Action<object>>();
            }
            emmiter.Add(callback);
        }

        /// <summary>
        /// 反注册一条事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="callback">回调</param>
        public void Off(string eventName, Action<object> callback)
        {
            Guard.Requires<ArgumentNullException>(eventName != null);
            Guard.Requires<ArgumentNullException>(callback != null);
            List<Action<object>> emmiter;
            if (!emmiters.TryGetValue(eventName, out emmiter))
            {
                return;
            }

            emmiter.RemoveAll((o) => o == callback);
            if (emmiter.Count <= 0)
            {
                emmiters.Remove(eventName);
            }
        }

        /// <summary>
        /// 触发一个Socket事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="payload">载荷</param>
        public void Trigger(string eventName, object payload = null)
        {
            Guard.Requires<ArgumentNullException>(eventName != null);
            List<Action<object>> emmiter;
            if (!emmiters.TryGetValue(eventName, out emmiter))
            {
                return;
            }
            foreach (var action in emmiter)
            {
                action.Invoke(payload);
            }
        }
    }
}
