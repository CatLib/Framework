using System;
using System.Collections.Generic;
using CatLib.Contracts;
using CatLib.Contracts.Event;

namespace CatLib
{

    /// <summary>
    /// 组件基类
    /// </summary>
    public class MonoComponent : MonoObject, IEvent
    {

        private IEventAchieve eventAchieve = null;
        /// <summary>
        /// 事件系统
        /// </summary>
        public virtual IEventAchieve Event
        {
            get
            {
                if (eventAchieve == null) { eventAchieve = App.Instance.Make<IEventAchieve>(); }
                return eventAchieve;
            }
        }

        /// <summary>
        /// 应用程序
        /// </summary>
        public IApplication Application
        {
            get { return App.Instance; }
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
            to.Event.On(name , handler);
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
            to.Event.One(name , handler);
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