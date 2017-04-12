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
using CatLib.API.Event;

namespace CatLib
{
    /// <summary>
    /// 组件基类
    /// </summary>
    public class MonoComponent : MonoObject, IEvent
    {
        /// <summary>
        /// 事件实现
        /// </summary>
        private IEventAchieve eventAchieve;

        /// <summary>
        /// 注册到消息中心的句柄
        /// </summary>
        private List<IEventHandler> handlers;

        /// <summary>
        /// 事件系统
        /// </summary>
        public virtual IEventAchieve Event
        {
            get { return eventAchieve ?? (eventAchieve = App.Make<IEventAchieve>()); }
        }

        /// <summary>
        /// 应用程序
        /// </summary>
        public IApplication App
        {
            get { return CatLib.App.Instance; }
        }

        /// <summary>
        /// 注册事件到指定的对象
        /// </summary>
        /// <param name="to">注册到的目标</param>
        /// <param name="name">注册的事件名</param>
        /// <param name="handler">事件回调</param>
        /// <param name="life">激活几次后将会被自动释放</param>
        /// <returns></returns>
        protected IEventHandler On(IEvent to, string name, EventHandler handler, int life = -1)
        {
            if (handlers == null)
            {
                handlers = new List<IEventHandler>();
            }
            var eventHandler = to.Event.On(name, handler, life);
            handlers.Add(eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 注册事件到指定对象（执行一次后释放关联）
        /// </summary>
        /// <param name="to"></param>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        protected IEventHandler One(IEvent to, string name, EventHandler handler)
        {
            return On(to, name, handler, 1);
        }

        /// <summary>
        /// 清除我注册的消息
        /// </summary>
        private void ClearHandlers()
        {
            if (handlers == null)
            {
                return;
            }
            for (var i = 0; i < handlers.Count; i++)
            {
                handlers[i].Cancel();
            }
            handlers.Clear();
            handlers = null;
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public virtual void OnDestroy()
        {
            ClearHandlers();
        }
    }
}