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
using System.Collections;
using System.Collections.Generic;
using CatLib.API;
using CatLib.API.Event;
using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// Application行为驱动器
    /// </summary>
    public class Driver : Container.Container, IEventImpl
    {
        /// <summary>
        /// 主线程调度队列锁
        /// </summary>
        private readonly object mainThreadDispatcherQueueLocker = new object();

        /// <summary>
        /// 更新
        /// </summary>
        private readonly LinkedList<IUpdate> update = new LinkedList<IUpdate>();

        /// <summary>
        /// 延后更新
        /// </summary>
        private readonly LinkedList<ILateUpdate> lateUpdate = new LinkedList<ILateUpdate>();

        /// <summary>
        /// 释放时需要调用的
        /// </summary>
        private readonly LinkedList<IDestroy> destroy = new LinkedList<IDestroy>();

        /// <summary>
        /// 优先标记
        /// </summary>
        private readonly Type priority = typeof(PriorityAttribute);

        /// <summary>
        /// 事件实体
        /// </summary>
        private IEventImpl eventImpl;

        /// <summary>
        /// 事件系统
        /// </summary>
        private IEventImpl EventSystem
        {
            get { return eventImpl ?? (eventImpl = this.Make<IEventImpl>()); }
        }

        /// <summary>
        /// 主线程ID
        /// </summary>
        private readonly int mainThreadId;

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public bool IsMainThread
        {
            get
            {
                return mainThreadId == System.Threading.Thread.CurrentThread.ManagedThreadId;
            }
        }

        /// <summary>
        /// 驱动脚本
        /// </summary>
        private DriverBehaviour driverBehaviour;

        /// <summary>
        /// 主线程调度队列
        /// </summary>
        private readonly Queue<Action> mainThreadDispatcherQueue = new Queue<Action>();

        /// <summary>
        /// Application行为驱动器
        /// </summary>
        public Driver() : this(null) { }

        /// <summary>
        /// Application行为驱动器
        /// </summary>
        public Driver(Component mainBehavior)
        {
            Initialization(mainBehavior);

            mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            OnResolving((bindData, obj) =>
            {
                if (obj == null)
                {
                    return null;
                }
                if (bindData.IsStatic)
                {
                    Load(obj);
                }
                return obj;
            });

            OnRelease((bindData, obj) =>
            {
                if (obj == null)
                {
                    return;
                }
                if (bindData.IsStatic)
                {
                    UnLoad(obj);
                }
            });
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mainBehavior">mono脚本</param>
        private void Initialization(Component mainBehavior)
        {
            if (mainBehavior == null)
            {
                return;
            }
            driverBehaviour = mainBehavior.gameObject.AddComponent<DriverBehaviour>();
            driverBehaviour.SetDriver(this);
        }

        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="obj">卸载的对象</param>
        private void UnLoad(object obj)
        {
            if (obj is IUpdate)
            {
                update.Remove((IUpdate)obj);
            }

            if (obj is ILateUpdate)
            {
                lateUpdate.Remove((ILateUpdate)obj);
            }

            if (obj is IDestroy)
            {
                destroy.Remove((IDestroy)obj);
                ((IDestroy)obj).OnDestroy();
            }
        }

        /// <summary>
        /// 装载
        /// </summary>
        /// <param name="obj">加载的对象</param>
        private void Load(object obj)
        {
            if (obj is IStart)
            {
                ((IStart)obj).Start();
            }

            if (obj is IUpdate)
            {
                AddWidthPriorities(update, (IUpdate)obj, "Update");
            }

            if (obj is ILateUpdate)
            {
                AddWidthPriorities(lateUpdate, (ILateUpdate)obj, "LateUpdate");
            }

            if (obj is IDestroy)
            {
                AddWidthPriorities(destroy, (IDestroy)obj, "OnDestroy");
            }
        }

        /// <summary>
        /// 根据优先级增加到列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="val">增加的内容</param>
        /// <param name="method">识别的方法</param>
        private void AddWidthPriorities<T>(LinkedList<T> list, T val, string method)
        {
            var current = list.First;
            while (current != null)
            {
                var currentPriorities = GetPriorities(current.Value.GetType(), method);
                var valPriorities = GetPriorities(val.GetType(), method);

                if (currentPriorities > valPriorities)
                {
                    break;
                }

                current = current.Next;
            }
            if (current != null)
            {
                list.AddBefore(current, val);
            }
            else
            {
                list.AddLast(val);
            }
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">识别的类型</param>
        /// <param name="method">识别的方法</param>
        /// <returns>优先级</returns>
        protected int GetPriorities(Type type, string method)
        {
            var currentPriority = int.MaxValue;
            var methodInfo = type.GetMethod(method);

            if (methodInfo.IsDefined(priority, false))
            {
                currentPriority = (methodInfo.GetCustomAttributes(priority, false)[0] as PriorityAttribute).Priorities; ;
            }
            else if (type.IsDefined(priority, false))
            {
                currentPriority = (type.GetCustomAttributes(priority, false)[0] as PriorityAttribute).Priorities;
            }

            return currentPriority;
        }

        #region Action

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            var current = update.First;
            while (current != null)
            {
                current.Value.Update();
                current = current.Next;
            }
            lock (mainThreadDispatcherQueueLocker)
            {
                while (mainThreadDispatcherQueue.Count > 0)
                {
                    mainThreadDispatcherQueue.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// 每帧更新后
        /// </summary>
        public void LateUpdate()
        {
            var current = lateUpdate.First;
            while (current != null)
            {
                current.Value.LateUpdate();
                current = current.Next;
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            var current = destroy.First;
            while (current != null)
            {
                current.Value.OnDestroy();
                current = current.Next;
            }
        }

        #endregion

        #region Main Thread Dispatcher

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">协程，执行会处于主线程</param>
        public void MainThread(IEnumerator action)
        {
            if (IsMainThread)
            {
                StartCoroutine(action);
                return;
            }
            lock (mainThreadDispatcherQueueLocker)
            {
                mainThreadDispatcherQueue.Enqueue(() =>
                {
                    StartCoroutine(action);
                });
            }
        }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">回调，回调的内容会处于主线程</param>
        public void MainThread(Action action)
        {
            if (IsMainThread)
            {
                action.Invoke();
                return;
            }
            MainThread(ActionWrapper(action));
        }

        /// <summary>
        /// 包装器
        /// </summary>
        /// <param name="action">回调函数</param>
        /// <returns></returns>
        private IEnumerator ActionWrapper(Action action)
        {
            action.Invoke();
            yield return null;
        }

        #endregion

        #region Coroutine

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程</param>
        public UnityEngine.Coroutine StartCoroutine(IEnumerator routine)
        {
            return driverBehaviour.StartCoroutine(routine);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        public void StopCoroutine(IEnumerator routine)
        {
            driverBehaviour.StopCoroutine(routine);
        }

        #endregion

        #region Dispatcher

        /// <summary>
        /// 事件系统
        /// </summary>
        public IEventImpl Event
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// 触发一个全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>全局事件</returns>
        public IGlobalEvent TriggerGlobal(string eventName)
        {
            return TriggerGlobal(eventName, null);
        }

        /// <summary>
        /// 触发一个全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="source">触发事件的源</param>
        /// <returns>全局事件</returns>
        public IGlobalEvent TriggerGlobal(string eventName, object source)
        {
            return new GlobalEvent(eventName, source);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        public void Trigger(string eventName)
        {
            EventSystem.Trigger(eventName);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="e">事件参数</param>
        public void Trigger(string eventName, EventArgs e)
        {
            EventSystem.Trigger(eventName, e);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">事件发送者</param>
        public void Trigger(string eventName, object sender)
        {
            EventSystem.Trigger(eventName, sender);
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        public void Trigger(string eventName, object sender, EventArgs e)
        {
            EventSystem.Trigger(eventName, sender, e);
        }

        /// <summary>
        /// 注册一个全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件回调</param>
        /// <param name="life">事件生命，当生命为0则自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, EventHandler handler, int life = -1)
        {
            return EventSystem.On(eventName, handler, life);
        }

        /// <summary>
        /// 注册一个一次性的全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件回调</param>
        /// <returns>事件句柄</returns>
        public IEventHandler One(string eventName, EventHandler handler)
        {
            return EventSystem.One(eventName, handler);
        }

        /// <summary>
        /// 释放一个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件句柄</param>
        public void Off(string eventName, IEventHandler handler)
        {
            EventSystem.Off(eventName, handler);
        }

        #endregion
    }
}