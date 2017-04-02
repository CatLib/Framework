using System;
using System.Collections.Generic;
using CatLib.API;

namespace CatLib.Flux
{

    /// <summary>
    /// 调度器
    /// </summary>
    public class Dispatcher<TPayload>
    {

        /// <summary>
        /// 调度列表
        /// </summary>
        private Dictionary<string , Action<TPayload>> callbacks;

        /// <summary>
        /// 是否处于调度中
        /// </summary>
        private bool isDispatching;

        /// <summary>
        /// 是否是待处理的
        /// </summary>
        private Dictionary<string, bool> isPending;

        /// <summary>
        /// 是否是已经处理的
        /// </summary>
        private Dictionary<string, bool> isHandled;

        public Dispatcher()
        {
            callbacks = new Dictionary<string, Action<TPayload>>();
            isDispatching = false;
            isPending = new Dictionary<string, bool>();
            isHandled = new Dictionary<string, bool>();
        }

        /// <summary>
        /// 注册一个调度事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void On(string key , Action<TPayload> action)
        {
            if (callbacks.ContainsKey(key))
            {
                throw new CatLibException("catlib flux Dispatcher.On(...): " + key + " , key is alreay exists.");
            }
            callbacks.Add(key, action);
        }

        /// <summary>
        /// 解除调度事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void Off(string key)
        {
            if (callbacks.ContainsKey(key))
            {
                throw new CatLibException("catlib flux Dispatcher.Off(...): " + key + " , does not map to a registered callback.");
            }
            callbacks.Remove(key);
        }

        /// <summary>
        /// 在调度中执行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        public void WaitFor(string key, TPayload payload)
        {
            if (!isDispatching)
            {
                throw new CatLibException("Dispatcher.WaitFor(...): Must be invoked while dispatching.");
            }

            GuardCallback(key);

            if (isPending[key])
            {
                throw new CatLibException("Dispatcher.WaitFor(...): Circular dependency detected while waiting for : " + key + ".");
            }
     
            InvokeCallback(key, payload);
        }

        /// <summary>
        /// 进行调度
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="payload">附带物</param>
        public void Dispatch(string key , TPayload payload)
        {
            GuardDispatching();
            StartDispatch(key, payload);
            InvokeCallback(key, payload);
            StopDispatch();
        }

        /// <summary>
        /// 进行调度
        /// </summary>
        /// <param name="payload">附带物</param>
        public void Dispatch(TPayload payload)
        {
            GuardDispatching();
            StartDispatch(payload);
            InvokeCallback(payload);
            StopDispatch();
        }

        /// <summary>
        /// 开始调度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        protected void StartDispatch(string key , TPayload payload)
        {
            GuardCallback(key);
            isPending[key] = false;
            isHandled[key] = false;
            isDispatching  = true;
        }

        /// <summary>
        /// 开始调度
        /// </summary>
        /// <param name="payload"></param>
        protected void StartDispatch(TPayload payload)
        {
            foreach(var kv in callbacks)
            {
                isPending[kv.Key] = false;
                isHandled[kv.Key] = false;
            }
            isDispatching = true;
        }

        /// <summary>
        /// 触发回调
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        protected void InvokeCallback(string key , TPayload payload)
        {
            isPending[key] = true;
            callbacks[key].Invoke(payload);
            isHandled[key] = true;
        }

        /// <summary>
        /// 触发回调
        /// </summary>
        /// <param name="payload"></param>
        protected void InvokeCallback(TPayload payload)
        {
            foreach (var kv in callbacks)
            {
                isPending[kv.Key] = true;
                kv.Value.Invoke(payload);
                isHandled[kv.Key] = true;
            }
        }

        /// <summary>
        /// 结束调度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        protected void StopDispatch()
        {
            isDispatching = false;
        }

        /// <summary>
        /// 验证回掉状态
        /// </summary>
        /// <param name="key"></param>
        protected void GuardCallback(string key)
        {
            if (!callbacks.ContainsKey(key))
            {
                throw new CatLibException("catlib flux Dispatcher.Dispatch(...): " + key + " , does not map to a registered callback.");
            }
        }

        /// <summary>
        /// 验证调度状态
        /// </summary>
        protected void GuardDispatching()
        {
            if (isDispatching)
            {
                throw new CatLibException("Dispatch.Dispatch(...): Cannot dispatch in the middle of a dispatch.");
            }
        }
    }

}