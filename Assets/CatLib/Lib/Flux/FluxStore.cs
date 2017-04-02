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
    public class FluxStore<TPayload>
    {

        /// <summary>
        /// token
        /// </summary>
        private string dispatchToken;

        /// <summary>
        /// 调度器
        /// </summary>
        protected IFluxDispatcher<TPayload> dispatcher;

        /// <summary>
        /// 是否被修改的
        /// </summary>
        protected bool changed;

        /// <summary>
        /// 监听者
        /// </summary>
        protected event Action listener;

        /// <summary>
        /// 构建一个存储块
        /// </summary>
        /// <param name="dispatcher"></param>
        public FluxStore(IFluxDispatcher<TPayload> dispatcher)
        {
            this.dispatcher = dispatcher;
            changed = false;
            dispatchToken = this.dispatcher.On((payload) =>
            {
                InvokeOnDispatch(payload);
            });
        }

        /// <summary>
        /// 增加监听者
        /// </summary>
        /// <param name="action"></param>
        public void AddListener(Action action)
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
        public IFluxDispatcher<TPayload> Dispatcher
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

        private void InvokeOnDispatch(TPayload payload)
        {
            changed = false;
            OnDispatch(payload);
            if (changed && listener != null)
            {
                listener.Invoke();
            }
        }

        protected virtual void OnDispatch(TPayload payload)
        {
            throw new CatLibException(GetType().Name + " has not overridden FluxStore.OnDispatch(), which is required");
        }
    }
}
