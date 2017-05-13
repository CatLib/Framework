/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this sender code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Collections.Generic;
using CatLib.API;

namespace CatLib
{
    /// <summary>
    /// 全局事件
    /// </summary>
    public sealed class GlobalEvent : IGlobalEvent
    {
        /// <summary>
        /// 事件源
        /// </summary>
        private readonly object sender;

        /// <summary>
        /// 事件名称
        /// </summary>
        private readonly string eventName;

        /// <summary>
        /// 触发接口级事件的接口
        /// </summary>
        private List<string> classInterface;

        /// <summary>
        /// 事件广播级别
        /// </summary>
        private EventLevel eventLevel = EventLevel.All;

        /// <summary>
        /// 构造一个全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">发送者</param>
        public GlobalEvent(string eventName , object sender)
        {
            this.eventName = eventName;
            this.sender = sender;
        }

        /// <summary>
        /// 增加事件响应接口
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns>全局事件实例</returns>
        public IGlobalEvent AppendInterface<T>()
        {
            AppendInterface(typeof(T));
            return this;
        }

        /// <summary>
        /// 增加事件响应接口
        /// </summary>
        /// <param name="t">接口类型</param>
        /// <returns>全局事件实例</returns>
        public IGlobalEvent AppendInterface(Type t)
        {
            if (classInterface == null)
            {
                classInterface = new List<string>();
            }
            classInterface.Add(t.ToString());
            return this;
        }

        /// <summary>
        /// 设定事件等级
        /// </summary>
        /// <param name="level">事件等级</param>
        /// <returns>全局事件实例</returns>
        public IGlobalEvent SetEventLevel(EventLevel level)
        {
            eventLevel = level;
            return this;
        }

        /// <summary>
        /// 触发一个全局事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public void Trigger(EventArgs args = null)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new RuntimeException("global event , event name can not be null");
            }

            if ((eventLevel & EventLevel.Self) > 0)
            {
                var guid = sender as IGuid;
                if (guid != null)
                {
                    App.Instance.Trigger(eventName + sender.GetType() + guid.Guid, sender, args);
                }
            }

            if (sender != null && (eventLevel & EventLevel.Type) > 0)
            {
                App.Instance.Trigger(eventName + sender.GetType(), sender, args);
            }

            if (classInterface != null && (eventLevel & EventLevel.Interface) > 0)
            {
                for (var i = 0; i < classInterface.Count; i++)
                {
                    App.Instance.Trigger(eventName + classInterface[i], sender, args);
                }
            }

            if ((eventLevel & EventLevel.Global) > 0)
            {
                App.Instance.Trigger(eventName, sender, args);
            }
        }
    }
}