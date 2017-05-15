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
using CatLib.API.Flux;

namespace CatLib.Flux
{
    /// <summary>
    /// 调度器
    /// </summary>
    public sealed class FluxDispatcher : IFluxDispatcher
    {
        /// <summary>
        /// last id生成时给定的前缀
        /// </summary>
        private static readonly string prefix = "id_";

        /// <summary>
        /// 调度列表，store接受来自调度器的调度
        /// </summary>
        private readonly Dictionary<string, Action<IAction>> callbacks;

        /// <summary>
        /// 是否是待处理的
        /// </summary>
        private readonly Dictionary<string, bool> isPending;

        /// <summary>
        /// 是否是已经处理的
        /// </summary>
        private readonly Dictionary<string, bool> isHandled;

        /// <summary>
        /// 是否处于调度中
        /// </summary>
        /// <returns>是否处于调度中</returns>
        public bool IsDispatching { get; private set; }

        /// <summary>
        /// Id 计数器
        /// </summary>
        private int lastId;

        /// <summary>
        /// 构建一个调度器
        /// </summary>
        public FluxDispatcher()
        {
            callbacks = new Dictionary<string, Action<IAction>>();
            IsDispatching = false;
            isPending = new Dictionary<string, bool>();
            isHandled = new Dictionary<string, bool>();
            lastId = 0;
        }

        /// <summary>
        /// 注册一个匿名调度事件
        /// </summary>
        /// <param name="action">响应调度事件的回调</param>
        public string On(Action<IAction> action)
        {
            var id = prefix + lastId++;
            On(id, action);
            return id;
        }

        /// <summary>
        /// 注册一个调度事件
        /// </summary>
        /// <param name="token">标识符</param>
        /// <param name="action">响应调度事件的回调</param>
        public void On(string token, Action<IAction> action)
        {
            if (callbacks.ContainsKey(token))
            {
                throw new API.RuntimeException("catlib flux Dispatcher.On(...): " + token + " , token is alreay exists.");
            }
            callbacks.Add(token, action);
        }

        /// <summary>
        /// 解除调度事件
        /// </summary>
        /// <param name="token">标识符</param>
        public void Off(string token)
        {
            if (!callbacks.ContainsKey(token))
            {
                throw new API.RuntimeException("catlib flux Dispatcher.Off(...): " + token + " , does not map to a registered callback.");
            }
            callbacks.Remove(token);
        }

        /// <summary>
        /// 等待调度器完成另外的调度
        /// </summary>
        /// <param name="token">标识符</param>
        /// <param name="action">行为</param>
        public void WaitFor(string token, IAction action)
        {
            if (!IsDispatching)
            {
                throw new API.RuntimeException("Dispatcher.WaitFor(...): Must be invoked while dispatching.");
            }

            GuardCallback(token);

            if (isPending[token])
            {
                if (isHandled[token])
                {
                    throw new API.RuntimeException("Dispatcher.WaitFor(...): circular dependency detected while waiting for : " + token + ".");
                }
            }

            InvokeCallback(token, action);
        }

        /// <summary>
        /// 将行为调度到指定标识符的Store中
        /// </summary>
        /// <param name="token">标识符</param>
        /// <param name="action">行为</param>
        public void Dispatch(string token, IAction action)
        {
            GuardDispatching();
            StartDispatch(token);
            InvokeCallback(token, action);
            StopDispatch();
        }

        /// <summary>
        /// 调度行为
        /// </summary>
        /// <param name="action">行为</param>
        public void Dispatch(IAction action)
        {
            GuardDispatching();
            StartDispatch();
            InvokeCallback(action);
            StopDispatch();
        }

        /// <summary>
        /// 通过标识符来调度
        /// </summary>
        /// <param name="token">标识符</param>
        private void StartDispatch(string token)
        {
            GuardCallback(token);
            isPending[token] = false;
            isHandled[token] = false;
            IsDispatching = true;
        }

        /// <summary>
        /// 开始调度
        /// </summary>
        private void StartDispatch()
        {
            foreach (var kv in callbacks)
            {
                isPending[kv.Key] = false;
                isHandled[kv.Key] = false;
            }
            IsDispatching = true;
        }

        /// <summary>
        /// 调度到指定的Store
        /// </summary>
        /// <param name="token">标识符</param>
        /// <param name="action">行为</param>
        private void InvokeCallback(string token, IAction action)
        {
            isPending[token] = true;
            callbacks[token].Invoke(action);
            isHandled[token] = true;
        }

        /// <summary>
        /// 调度到全体Store
        /// </summary>
        /// <param name="action">行为</param>
        private void InvokeCallback(IAction action)
        {
            foreach (var kv in callbacks)
            {
                isPending[kv.Key] = true;
                kv.Value.Invoke(action);
                isHandled[kv.Key] = true;
            }
        }

        /// <summary>
        /// 结束调度
        /// </summary>
        private void StopDispatch()
        {
            IsDispatching = false;
        }

        /// <summary>
        /// 验证回调状态
        /// </summary>
        /// <param name="token">标识符</param>
        private void GuardCallback(string token)
        {
            if (!callbacks.ContainsKey(token))
            {
                throw new API.RuntimeException("catlib flux Dispatcher.Dispatch(...): " + token + " , does not map to a registered callback.");
            }
        }

        /// <summary>
        /// 验证调度状态
        /// </summary>
        private void GuardDispatching()
        {
            if (IsDispatching)
            {
                throw new API.RuntimeException("Dispatch.Dispatch(...): Cannot dispatch in the middle of a dispatch.");
            }
        }
    }
}