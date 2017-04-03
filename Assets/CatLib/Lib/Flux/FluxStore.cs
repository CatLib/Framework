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

namespace CatLib.Flux
{
    
    /// <summary>
    /// 存储
    /// </summary>
    public abstract class FluxStore : IStore
    {

        /// <summary>
        /// 默认的存储名
        /// </summary>
        public static string DefaultName = "Store";

        /// <summary>
        /// 存储名
        /// </summary>
        private string storeName;

        /// <summary>
        /// 存储的名字
        /// </summary>
		public string Name { get { return storeName; } }

        /// <summary>
		/// 被管理的数据对象
		/// </summary>
		protected object data;

        /// <summary>
        /// 被管理的数据对象
        /// </summary>
        public object Data { get { return data; } set { data = value; } }

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
        protected virtual string NotificationName { get { return GetType().Name; } }

        /// <summary>
        /// 构建一个存储块
        /// </summary>
        /// <param name="dispatcher"></param>
        public FluxStore()
            : this(DefaultName, null)
        {
        }

        /// <summary>
        /// 构建一个存储块
        /// </summary>
        /// <param name="dispatcher"></param>
        public FluxStore(string storeName) 
            : this(storeName, null)
        {
        }

        /// <summary>
        /// 构建一个存储块
        /// </summary>
        /// <param name="dispatcher"></param>
        public FluxStore(string storeName, object data)
        {
            this.storeName = storeName;
            this.data = data;
            changed = false;
        }

        /// <summary>
        /// 当被移除时
        /// </summary>
        public virtual void OnRemove() { }

        /// <summary>
        /// 设定调度器
        /// </summary>
        /// <param name="dispatcher"></param>
        public void SetDispatcher(IFluxDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            dispatchToken = this.dispatcher.On((payload) =>
            {
                InvokeOnDispatch(payload);
            });
        }

        /// <summary>
        /// 增加监听者
        /// </summary>
        /// <param name="action"></param>
        public void AddListener(Action<INotification> action)
        {
            listener += action;
        }

        /// <summary>
        /// 内容是否是被修改的
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (dispatcher.IsDispatching)
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
            if (dispatcher.IsDispatching)
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
