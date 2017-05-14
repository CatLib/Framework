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
        private readonly string storeName;

        /// <summary>
        /// 存储的名字
        /// </summary>
		public string Name { get { return storeName; } }

        /// <summary>
        /// 调度器中注册的token
        /// </summary>
        private readonly string dispatchToken;

        /// <summary>
        /// 调度器
        /// </summary>
        private readonly IFluxDispatcher dispatcher;

        /// <summary>
        /// 是否被修改的
        /// </summary>
        private bool changed;

        /// <summary>
        /// 监听者
        /// </summary>
        private event Action<IAction> listener;

        /// <summary>
        /// 行为名
        /// </summary>
        protected virtual string ActionName { get { return Name; } }

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
        protected FluxStore(IFluxDispatcher dispatcher)
        {
            storeName = GetType().Name;
            changed = false;
            isDestroy = false;
            this.dispatcher = dispatcher;
            dispatchToken = this.dispatcher.On(InvokeOnDispatch);
        }

        /// <summary>
        /// 释放存储
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
        public void AddListener(Action<IAction> action)
        {
            if (isDestroy)
            {
                throw new RuntimeException(GetType().Name + " is be destroy.");
            }
            listener += action;
        }

        /// <summary>
        /// 减少监听者
        /// </summary>
        /// <param name="action"></param>
        public void RemoveListener(Action<IAction> action)
        {
            if (isDestroy)
            {
                throw new RuntimeException(GetType().Name + " is be destroy.");
            }
            listener -= action;
        }

        /// <summary>
        /// 存储是否是被修改的
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (!dispatcher.IsDispatching)
                {
                    throw new RuntimeException(GetType().Name + ".IsChanged Must be invoked while dispatching.");
                }
                return changed;
            }
        }

        /// <summary>
        /// 存储归属的调度器
        /// </summary>
        public IFluxDispatcher Dispatcher
        {
            get { return dispatcher; }
        }

        /// <summary>
        /// 调度器中该存储注册的token
        /// </summary>
        public string DispatchToken
        {
            get { return dispatchToken; }
        }

        /// <summary>
        /// 触发存储发生修改
        /// </summary>
        protected void Change()
        {
            if (!dispatcher.IsDispatching)
            {
                throw new RuntimeException(GetType().Name + ".TriggerChange() Must be invoked while dispatching.");
            }
            changed = true;
        }

        /// <summary>
        /// 构建一个存储行为
        /// </summary>
        /// <returns></returns>
        protected abstract IAction StoreAction();

        /// <summary>
        /// 收到来自调度器的调度
        /// </summary>
        /// <param name="action">行为</param>
        private void InvokeOnDispatch(IAction action)
        {
            changed = false;
            OnDispatch(action);
            if (changed && listener != null)
            {
                listener.Invoke(StoreAction());
            }
        }

        /// <summary>
        /// 进行调度
        /// </summary>
        /// <param name="action">行为</param>
        protected virtual void OnDispatch(IAction action)
        {
            throw new RuntimeException(GetType().Name + " has not overridden FluxStore.OnDispatch(), which is required");
        }
    }
}
