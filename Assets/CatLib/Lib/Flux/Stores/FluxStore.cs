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
using CatLib.API;
using CatLib.API.Flux;

namespace CatLib.Flux
{
    
    /// <summary>
    /// 存储
    /// 我们一般情况下建议每个store都是单例的
    /// </summary>
    public abstract class FluxStore : IStore
    {

        /// <summary>
        /// 存储名
        /// </summary>
        private string storeName;

        /// <summary>
        /// 存储的名字
        /// </summary>
		public string Name { get { return storeName; } }

        /// <summary>
        /// token
        /// </summary>
        private string dispatchToken;

        /// <summary>
        /// 调度器
        /// </summary>
        protected IFluxDispatcher dispatcher;

        /// <summary>
        /// 是否被修改的
        /// </summary>
        protected bool changed;

        /// <summary>
        /// 监听者
        /// </summary>
        protected event Action<INotification> listener;

        /// <summary>
        /// 通知名
        /// </summary>
        protected virtual string NotificationName { get { return Name; } }

        /// <summary>
        /// 是否是被释放的
        /// </summary>
        private bool isDestroy;

        /// <summary>
        /// 是否被释放的
        /// </summary>
        public bool IsDestroy { get { return IsDestroy; } }

        /// <summary>
        /// 构建一个存储块
        /// </summary>
        /// <param name="dispatcher"></param>
        public FluxStore(IFluxDispatcher dispatcher)
        {
            storeName = GetType().Name;
            changed = false;
            isDestroy = false;
            this.dispatcher = dispatcher;
            dispatchToken = this.dispatcher.On((payload) =>
            {
                InvokeOnDispatch(payload);
            });
        }

        /// <summary>
        /// 释放存储块
        /// </summary>
        public void Destroy()
        {
            if (dispatcher != null)
            {
                dispatcher.Off(dispatchToken);
            }
            listener = null;
            isDestroy = true;
        }

        /// <summary>
        /// 增加监听者
        /// </summary>
        /// <param name="action"></param>
        public void AddListener(Action<INotification> action)
        {
            if (isDestroy)
            {
                throw new CatLibException(GetType().Name + " is be destroy.");
            }
            listener += action;
        }

        /// <summary>
        /// 减少监听者
        /// </summary>
        /// <param name="action"></param>
        public void RemoveListener(Action<INotification> action)
        {
            if (isDestroy)
            {
                throw new CatLibException(GetType().Name + " is be destroy.");
            }
            listener -= action;
        }

        /// <summary>
        /// 内容是否是被修改的
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (!dispatcher.IsDispatching)
                {
                    throw new CatLibException(GetType().Name + ".IsChanged Must be invoked while dispatching.");
                }
                return changed;
            }
        }

        /// <summary>
        /// 调度器
        /// </summary>
        public IFluxDispatcher Dispatcher
        {
            get { return dispatcher; }
        }

        /// <summary>
        /// 调度token
        /// </summary>
        public string DispatchToken
        {
            get { return dispatchToken; }
        }

        /// <summary>
        /// 内容发生修改
        /// </summary>
        protected void Change()
        {
            if (!dispatcher.IsDispatching)
            {
                throw new CatLibException(GetType().Name + ".TriggerChange() Must be invoked while dispatching.");
            }
            changed = true;
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <returns></returns>
        protected abstract INotification Notification();

        /// <summary>
        /// 触发调度
        /// </summary>
        /// <param name="payload"></param>
        private void InvokeOnDispatch(INotification payload)
        {
            changed = false;
            OnDispatch(payload);
            if (changed && listener != null)
            {
                listener.Invoke(Notification());
            }
        }

        /// <summary>
        /// 被调度
        /// </summary>
        /// <param name="payload"></param>
        protected virtual void OnDispatch(INotification payload)
        {
            throw new CatLibException(GetType().Name + " has not overridden FluxStore.OnDispatch(), which is required");
        }
    }
}
