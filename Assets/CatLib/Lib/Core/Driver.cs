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

namespace CatLib {

    /// <summary>
    /// Application行为驱动器
    /// </summary>
    public class Driver :  Container.Container , IEventAchieve , IDriver
    {

        /// <summary>
        /// 主线程调度队列锁
        /// </summary>
        protected object mainThreadDispatcherQueueLocker = new object();

        /// <summary>
        /// 更新
        /// </summary>
        protected List<IUpdate> update = new List<IUpdate>();

        /// <summary>
        /// 延后更新
        /// </summary>
        protected List<ILateUpdate> lateUpdate = new List<ILateUpdate>();

        /// <summary>
        /// 释放时需要调用的
        /// </summary>
        protected List<IDestroy> destroy = new List<IDestroy>();

        /// <summary>
        /// 事件实体
        /// </summary>
        private IEventAchieve eventAchieve;

        /// <summary>
        /// 事件系统
        /// </summary>
        private IEventAchieve EventSystem
        {
            get
            {
                if (eventAchieve == null)
                {
                    eventAchieve = this.Make<IEventAchieve>();
                }
                return eventAchieve;
            }
        }

        /// <summary>
        /// 主线程ID
        /// </summary>
        private int mainThreadID;

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
        private Queue<Action> mainThreadDispatcherQueue = new Queue<Action>();

        /// <summary>
        /// Application行为驱动器
        /// </summary>
        public Driver() : this(null) { }

        /// <summary>
        /// Application行为驱动器
        /// </summary>
        public Driver(MonoBehaviour mainBehavior) : base()
        {

            Initialization(mainBehavior);

            mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

            OnResolving((container, bindData, obj) =>
            {
                if (bindData.IsStatic){ Load(obj); }
                return obj;
            });

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mainObject"></param>
        private void Initialization(MonoBehaviour mainBehavior)
        {
            if (mainBehavior != null)
            {
                driverBehaviour = mainBehavior.gameObject.AddComponent<DriverBehaviour>();
                driverBehaviour.SetDriver(this);
            }
        }

        /// <summary>
        /// 装载
        /// </summary>
        /// <param name="obj"></param>
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

        public void Update()
        {
            for (int i = 0; i < update.Count; i++)
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

        public void LateUpdate()
        {
            for (int i = 0; i < lateUpdate.Count; i++)
            {
                lateUpdate[i].LateUpdate();
            }
        }

        public void OnDestroy()
        {
            for (int i = 0; i < destroy.Count; i++)
            {
                destroy[i].OnDestroy();
            }
        }

        #endregion

        #region Main Thread Dispatcher

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action"></param>
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
        /// <param name="action"></param>
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
        /// <param name="action"></param>
        /// <returns></returns>
        private IEnumerator ActionWrapper(Action action)
        {
            action.Invoke();
            yield return null;
        }

        #endregion

        #region Coroutine

        /// <summary>
        /// 启动协同
        /// </summary>
        /// <param name="routine"></param>
        public UnityEngine.Coroutine StartCoroutine(IEnumerator routine)
        {
            return driverBehaviour.StartCoroutine(routine);
        }

        /// <summary>
        /// 停止协同
        /// </summary>
        /// <param name="routine"></param>
        public void StopCoroutine(IEnumerator routine)
        {
            driverBehaviour.StopCoroutine(routine);
        }

        #endregion

        #region Dispatcher

        public IEventAchieve Event
        {
            get
            {
                return this;
            }
        }

        public IGlobalEvent Trigger(object score)
        {
            return new GlobalEvent(score);
        }

        public void Trigger(string eventName)
        {
            EventSystem.Trigger(eventName);
        }

        public void Trigger(string eventName, EventArgs e)
        {
            EventSystem.Trigger(eventName, e);
        }

        public void Trigger(string eventName, object sender)
        {
            EventSystem.Trigger(eventName, sender);
        }

        public void Trigger(string eventName, object sender, EventArgs e)
        {
            EventSystem.Trigger(eventName, sender, e);
        }

        public IEventHandler On(string eventName, EventHandler handler, int life = -1)
        {
            return EventSystem.On(eventName, handler, life);
        }

        public IEventHandler One(string eventName, EventHandler handler)
        {
            return EventSystem.One(eventName, handler);
        }

        public void Off(string eventName, IEventHandler handler)
        {
            EventSystem.Off(eventName, handler);
        }

        #endregion

    }

}