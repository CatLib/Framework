using System;
using System.Collections.Generic;
using CatLib.API;

namespace CatLib.Flux
{

    /// <summary>
    /// 调度器
    /// </summary>
    public class FluxDispatcher<TPayload> : IFluxDispatcher<TPayload>
    {

        /// <summary>
        /// 前缀
        /// </summary>
        private static string prefix = "id_";

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

        /// <summary>
        /// 末尾ID
        /// </summary>
        private int lastID;

        public FluxDispatcher()
        {
            callbacks = new Dictionary<string, Action<TPayload>>();
            isDispatching = false;
            isPending = new Dictionary<string, bool>();
            isHandled = new Dictionary<string, bool>();
            lastID = 0;
        }

        /// <summary>
        /// 是否处于调度中
        /// </summary>
        /// <returns></returns>
        public bool IsDispatching
        {
            get
            {
                return isDispatching;
            }
        }

        /// <summary>
        /// 注册一个匿名调度事件
        /// </summary>
        /// <param name="action"></param>
        public string On(Action<TPayload> action)
        {
            var id = prefix + lastID++;
            On(id, action);
            return id;
        }

        /// <summary>
        /// 注册一个调度事件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="action"></param>
        public void On(string token , Action<TPayload> action)
        {
            if (callbacks.ContainsKey(token))
            {
                throw new CatLibException("catlib flux Dispatcher.On(...): " + token + " , key is alreay exists.");
            }
            callbacks.Add(token, action);
        }

        /// <summary>
        /// 解除调度事件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="action"></param>
        public void Off(string token)
        {
            if (callbacks.ContainsKey(token))
            {
                throw new CatLibException("catlib flux Dispatcher.Off(...): " + token + " , does not map to a registered callback.");
            }
            callbacks.Remove(token);
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
        /// <param name="token">token</param>
        /// <param name="payload">附带物</param>
        public void Dispatch(string token , TPayload payload)
        {
            GuardDispatching();
            StartDispatch(token, payload);
            InvokeCallback(token, payload);
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
        /// <param name="token"></param>
        /// <param name="payload"></param>
        protected void StartDispatch(string token , TPayload payload)
        {
            GuardCallback(token);
            isPending[token] = false;
            isHandled[token] = false;
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
        /// <param name="token"></param>
        /// <param name="payload"></param>
        protected void InvokeCallback(string token , TPayload payload)
        {
            isPending[token] = true;
            callbacks[token].Invoke(payload);
            isHandled[token] = true;
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
        protected void StopDispatch()
        {
            isDispatching = false;
        }

        /// <summary>
        /// 验证回掉状态
        /// </summary>
        /// <param name="token"></param>
        protected void GuardCallback(string token)
        {
            if (!callbacks.ContainsKey(token))
            {
                throw new CatLibException("catlib flux Dispatcher.Dispatch(...): " + token + " , does not map to a registered callback.");
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