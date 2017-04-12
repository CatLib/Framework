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
using CatLib.API.Event;
using CatLib.API;

namespace CatLib.Event
{

    public class EventStore : IEventAchieve
    {

        [Dependency]
        public IApplication App { get; set; }

        /// <summary>
        /// 事件地图
        /// </summary>
        protected Dictionary<string, List<EventHandler>> handlers;

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
            if (!App.IsMainThread)
            {
                App.MainThread(() =>
                {
                    Trigger(eventName, sender, e);
                });
                return;
            }

            if (handlers == null) { return; }

            if (handlers.ContainsKey(eventName) && handlers[eventName].Count > 0)
            {
                List<EventHandler> removeList = null;
                var handler = handlers[eventName];
                for(int i = 0; i < handler.Count; i++){
                    
                    handler[i].Call(sender, e);
                    if(!handler[i].IsLife){

                        if(removeList == null){ removeList = new List<EventHandler>(); }
                        removeList.Add(handler[i]);

                    }

                }

                if(removeList != null){
                    for(int i = 0; i < removeList.Count ; i++){

                        removeList[i].Cancel();
                    
                    }
                }
            }

        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, System.EventHandler handler , int life = -1)
        {
            var callHandler = new EventHandler(this , eventName , handler , life);
            if (!App.IsMainThread)
            {
                App.MainThread(() =>
                {
                    On(eventName , callHandler);
                });
                return callHandler;
            }
            On(eventName , callHandler);
            return callHandler;
        }

        /// <summary>
        /// 注册一个事件，调用一次后就释放
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件句柄</param>
        /// <returns></returns>
        public IEventHandler One(string eventName , System.EventHandler handler)
        {
            return On(eventName , handler , 1);
        }

        private void On(string eventName, EventHandler handler){

            if (handlers == null) { handlers = new Dictionary<string, List<EventHandler>>(); }
            if (!handlers.ContainsKey(eventName))
            {
                handlers.Add(eventName, new List<EventHandler>());
            }
            handlers[eventName].Add(handler);
            handler.Active();

        }

        /// <summary>
        /// 移除一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">操作句柄</param>
        public void Off(string eventName, IEventHandler handler)
        {

            if (handlers == null) { return; }

            if (!App.IsMainThread)
            {
                App.MainThread(() =>
                {
                    Off(eventName, handler);
                });
                return;
            }
            
            if (handlers.ContainsKey(eventName))
            {
                handlers[eventName].Remove(handler as EventHandler);
                if(handlers[eventName].Count <= 0)
                {
                    handlers.Remove(eventName);
                }
            }
        }

    }

}