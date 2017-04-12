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
    public class Driver : Container.Container, IEventAchieve, IDriver
    {
        /// <summary>
        /// 主线程调度队列锁
        /// </summary>
        private readonly object mainThreadDispatcherQueueLocker = new object();

        /// <summary>
        /// 更新
        /// </summary>
        private readonly List<IUpdate> update = new List<IUpdate>();

        /// <summary>
        /// 延后更新
        /// </summary>
        private readonly List<ILateUpdate> lateUpdate = new List<ILateUpdate>();

        /// <summary>
        /// 释放时需要调用的
        /// </summary>
        private readonly List<IDestroy> destroy = new List<IDestroy>();

        /// <summary>
        /// 事件实体
        /// </summary>
        private IEventAchieve eventAchieve;

        /// <summary>
        /// 事件系统
        /// </summary>
        private IEventAchieve EventSystem
        {
            get { return eventAchieve ?? (eventAchieve = this.Make<IEventAchieve>()); }
        }

        /// <summary>
        /// 主线程ID
        /// </summary>
        private readonly int mainThreadID;

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public bool IsMainThread
        {
            get
            {
                return mainThreadID == System.Threading.Thread.CurrentThread.ManagedThreadId;
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

            mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

            OnResolving((bindData, obj) =>
            {
                if (bindData.IsStatic)
                {
                    Load(obj);
                }
                return obj;
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
        /// 装载
        /// </summary>
        /// <param name="obj">加载的对象</param>
        private void Load(object obj)
        {
            if (obj is IUpdate)
            {
                update.Add(obj as IUpdate);
            }

            if (obj is ILateUpdate)
            {
                lateUpdate.Add(obj as ILateUpdate);
            }

            if (obj is IDestroy)
            {
                destroy.Add(obj as IDestroy);
            }
        }

        #region Action

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            for (var i = 0; i < update.Count; i++)
            {
                update[i].Update();
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
            for (var i = 0; i < lateUpdate.Count; i++)
            {
                lateUpdate[i].LateUpdate();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            for (var i = 0; i < destroy.Count; i++)
            {
                destroy[i].OnDestroy();
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
        public IEventAchieve Event
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
        public IGlobalEvent TriggerGlobal(string eventName,object source)
        {
            return new GlobalEvent(eventName , source);
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