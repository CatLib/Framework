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

        private IEventAchieve m_eventAchieve = null;
        /// <summary>
        /// 事件系统
        /// </summary>
        public virtual IEventAchieve Event
        {
            get
            {
                if (m_eventAchieve == null)
                {
                    m_eventAchieve = App.Instance.Make<IEventAchieve>();
                }
                return m_eventAchieve;
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
        private List<IEventHandler> m_handlers;

        /// <summary>
        /// 注册事件到指定的对象
        /// </summary>
        /// <param name="target">注册到的目标</param>
        /// <param name="eventName">事件名字</param>
        /// <param name="handler">句柄</param>
        /// <param name="lift"></param>
        protected IEventHandler On(IEvent target, string eventName, EventHandler handler, int lift = -1)
        {
            if (m_handlers == null)
            {
                m_handlers = new List<IEventHandler>();
            }
            IEventHandler eventHandler = target.Event.On(eventName, handler, lift);
            m_handlers.Add(eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 注册事件到指定对象（执行一次后释放关联）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="eventName"></param>
        /// <param name="handler"></param>
        protected IEventHandler One(IEvent target, string eventName, EventHandler handler)
        {
            return On(target, eventName, handler, 1);
        }

        /// <summary>
        /// 清除我注册的消息
        /// </summary>
        private void ClearHandlers()
        {
            if (m_handlers != null)
            {
                for (int i = 0; i < m_handlers.Count; i++)
                {
                    m_handlers[i].Cancel();
                }
                m_handlers.Clear();
                m_handlers = null;
            }
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