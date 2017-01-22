using UnityEngine;
using System.Collections;
using CatLib.Event;
using System.Collections.Generic;
using System;
using CatLib.Contracts.Event;

namespace CatLib.Base
{

    /// <summary>
    /// 组件基类
    /// </summary>
    public class CMonoComponent : CMonoObject, IEvent
    {

        private CEvent cevent = null;
        /// <summary>
        /// 事件系统
        /// </summary>
        public CEvent Event
        {
            get
            {
                if (this.cevent == null) { this.cevent = new CEvent(); }
                return this.cevent;             
            }
        }

        /// <summary>
        /// 应用程序
        /// </summary>
        public CApplication Application
        {
            get { return CApplication.Instance; }
        }

        /// <summary>
        /// 注册到消息中心的句柄
        /// </summary>
        private Dictionary<IEvent, Dictionary<string, EventHandler>> handlers;

        /// <summary>
        /// 注册到消息中心的句柄（执行一次）
        /// </summary>
        private Dictionary<IEvent, Dictionary<string, EventHandler>> handlersOne;

        /// <summary>
        /// 注册事件到指定的对象
        /// </summary>
        /// <param name="to">注册到的目标</param>
        /// <param name="name">事件名字</param>
        /// <param name="handler">句柄</param>
        protected void On(IEvent to , string name , EventHandler handler)
        {
            if (handlers == null) { handlers = new Dictionary<IEvent, Dictionary<string, EventHandler>>(); }
            if (!handlers.ContainsKey(to)) { handlers.Add(to, new Dictionary<string, EventHandler>()); }
            if (!handlers[to].ContainsKey(name))
            {
                handlers[to].Add(name, handler);
                return;
            }
            handlers[to][name] += handler;
        }
    
        /// <summary>
        /// 注册事件到指定对象（执行一次后释放关联）
        /// </summary>
        /// <param name="to"></param>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        protected void One(IEvent to , string name , EventHandler handler)
        {
            if (handlersOne == null) { handlersOne = new Dictionary<IEvent, Dictionary<string, EventHandler>>(); }
            if (!handlersOne.ContainsKey(to)) { handlersOne.Add(to, new Dictionary<string, EventHandler>()); }
            if (!handlersOne[to].ContainsKey(name))
            {
                handlersOne[to].Add(name, handler);
                return;
            }
            handlersOne[to][name] += handler;
        }

        /// <summary>
        /// 清除我注册的消息
        /// </summary>
        private void ClearHandlers()
        {
            if (handlers != null)
            {
                foreach (KeyValuePair<IEvent, Dictionary<string, EventHandler>> data1 in handlers)
                {
                    foreach (KeyValuePair<string, EventHandler> data2 in data1.Value)
                    {
                        if (data1.Key != null)
                        {
                            data1.Key.Event.Off(data2.Key, data2.Value);
                        }
                    }
                }
            }

            if (handlersOne != null)
            {
                foreach (KeyValuePair<IEvent, Dictionary<string, EventHandler>> data1 in handlersOne)
                {
                    foreach (KeyValuePair<string, EventHandler> data2 in data1.Value)
                    {
                        if (data1.Key != null)
                        {
                            data1.Key.Event.OffOne(data2.Key, data2.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public virtual void OnDestroy()
        {
            this.ClearHandlers();
        }

    }

}