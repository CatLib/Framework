using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CatLib.Event
{

    public class CEvent
    {

        /// <summary>
        /// 事件地图
        /// </summary>
        protected Dictionary<string, EventHandler> handlers;

        /// <summary>
        /// 事件地图
        /// </summary>
        protected Dictionary<string, EventHandler> handlersOne;

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        public void Trigger(string eventName)
        {
            Trigger(eventName, null, EventArgs.Empty);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="e">参数</param>
        public void Trigger(string eventName, EventArgs e)
        {
            Trigger(eventName, null, e);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="sender">发送者</param>
        public void Trigger(string eventName, object sender)
        {
            Trigger(eventName, sender, EventArgs.Empty);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="sender">发送者</param>
        /// <param name="e">参数</param>
        public void Trigger(string eventName, object sender, EventArgs e)
        {
            if (handlers == null) { handlers = new Dictionary<string, EventHandler>(); }
            if (handlersOne == null) { handlersOne = new Dictionary<string, EventHandler>(); }
            if (handlers.ContainsKey(eventName))
            {
                handlers[eventName](sender, e);
            }
            if (handlersOne.ContainsKey(eventName))
            {
                handlersOne[eventName](sender, e);
                handlersOne.Remove(eventName);
            }
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">操作句柄</param>
        /// <returns></returns>
        public void On(string eventName, EventHandler handler)
        {
            if (handlers == null) { handlers = new Dictionary<string, EventHandler>(); }
            if (!handlers.ContainsKey(eventName))
            {
                handlers.Add(eventName, handler);
                return;
            }
            handlers[eventName] += handler;
        }

        /// <summary>
        /// 注册一个事件，调用一次后就释放
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handler"></param>
        public void One(string eventName , EventHandler handler)
        {
            if (handlersOne == null) { handlersOne = new Dictionary<string, EventHandler>(); }
            if (!handlersOne.ContainsKey(eventName))
            {
                handlersOne.Add(eventName, handler);
                return;
            }
            handlersOne[eventName] += handler;
        }

        /// <summary>
        /// 移除一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">操作句柄</param>
        public void Off(string eventName, EventHandler handler)
        {
            if (handlers == null) { return; }
            if (handlers.ContainsKey(eventName))
            {
                handlers[eventName] -= handler;
            }
        }

        /// <summary>
        /// 移除一个一次性事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handler"></param>
        public void OffOne(string eventName , EventHandler handler)
        {
            if (handlersOne == null) { return; }
            if (handlersOne.ContainsKey(eventName))
            {
                handlersOne[eventName] -= handler;
            }
        }
    }

}