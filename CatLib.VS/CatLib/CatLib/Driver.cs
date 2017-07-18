/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this sender code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CatLib.API;
using CatLib.API.Stl;
using CatLib.Stl;
using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// Application行为驱动器
    /// </summary>
    public class Driver : Container, IEvent
    {
        /// <summary>
        /// 主线程调度队列锁
        /// </summary>
        private readonly object mainThreadDispatcherQueueLocker = new object();

        /// <summary>
        /// 更新
        /// </summary>
        private readonly SortSet<IUpdate, int> update = new SortSet<IUpdate, int>();

        /// <summary>
        /// 延后更新
        /// </summary>
        private readonly SortSet<ILateUpdate, int> lateUpdate = new SortSet<ILateUpdate, int>();

        /// <summary>
        /// 释放时需要调用的
        /// </summary>
        private readonly SortSet<IDestroy, int> destroy = new SortSet<IDestroy, int>();

        /// <summary>
        /// 载入结果集
        /// </summary>
        private readonly HashSet<object> loadSet = new HashSet<object>();

        /// <summary>
        /// 优先标记
        /// </summary>
        private readonly Type priority = typeof(PriorityAttribute);

        /// <summary>
        /// 事件系统
        /// </summary>
        private IEvent eventSystem;

        /// <summary>
        /// 事件系统
        /// </summary>
        private IEvent EventSystem
        {
            get
            {
                return eventSystem ?? (eventSystem = new Event());
            }
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
        public Driver() : this(null)
        {
        }

        /// <summary>
        /// Application行为驱动器
        /// </summary>
        public Driver(Component mainBehavior)
        {
            if (mainBehavior != null)
            {
                Initialization(mainBehavior);
            }

            mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            OnResolving(DefaultOnResolving);
            OnRelease(DefaultOnRelease);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mainBehavior">mono脚本</param>
        [ExcludeFromCodeCoverage]
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
        /// 从驱动器中卸载对象
        /// 如果对象使用了增强接口，那么卸载对应增强接口
        /// 从驱动器中卸载对象会引发IDestroy增强接口
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当卸载对象为<c>null</c>时引发</exception>
        public void Detach(object obj)
        {
            Guard.Requires<ArgumentNullException>(obj != null);

            if (!loadSet.Contains(obj))
            {
                return;
            }

            ConvertAndRemove(update, obj);
            ConvertAndRemove(lateUpdate, obj);

            if (ConvertAndRemove(destroy, obj))
            {
                ((IDestroy)obj).OnDestroy();
            }

            loadSet.Remove(obj);
        }

        /// <summary>
        /// 如果对象实现了增强接口那么将对象装载进对应驱动器
        /// 在装载的时候会引发IStart接口
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当装载对象为<c>null</c>时引发</exception>
        public void Attach(object obj)
        {
            Guard.Requires<ArgumentNullException>(obj != null);

            if (loadSet.Contains(obj))
            {
                throw new RuntimeException("Object [" + obj + "] is already load.");
            }

            var isLoad = ConvertAndAdd(update, obj, "Update");
            isLoad = ConvertAndAdd(lateUpdate, obj, "LateUpdate") || isLoad;
            isLoad = ConvertAndAdd(destroy, obj, "OnDestroy") || isLoad;

            if (isLoad)
            {
                loadSet.Add(obj);
            }
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">识别的类型</param>
        /// <param name="method">识别的方法</param>
        /// <returns>优先级</returns>
        public int GetPriorities(Type type, string method = null)
        {
            Guard.Requires<ArgumentNullException>(type != null);
            var currentPriority = int.MaxValue;

            MethodInfo methodInfo;
            if (method != null &&
                (methodInfo = type.GetMethod(method)) != null &&
                methodInfo.IsDefined(priority, false))
            {
                currentPriority = (methodInfo.GetCustomAttributes(priority, false)[0] as PriorityAttribute).Priorities;
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
            foreach (var current in update)
            {
                current.Update();
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
            foreach (var current in lateUpdate)
            {
                current.LateUpdate();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            Trigger(ApplicationEvents.OnBeforeDestroy, this);
            ReleaseAll();
            foreach (var current in destroy)
            {
                current.OnDestroy();
            }
            update.Clear();
            lateUpdate.Clear();
            destroy.Clear();
            loadSet.Clear();
            App.Instance = null;
            Trigger(ApplicationEvents.OnDestroyed, this);
        }

        #endregion

        #region Main Thread Dispatcher

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">代码块执行会处于主线程</param>
        public void MainThread(IEnumerator action)
        {
            Guard.Requires<ArgumentNullException>(action != null);
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
        /// <param name="action">代码块执行会处于主线程</param>
        public void MainThread(Action action)
        {
            Guard.Requires<ArgumentNullException>(action != null);
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
        /// <returns>迭代器</returns>
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
        /// <param name="routine">协程内容</param>
        /// <returns>协程</returns>
        /// <exception cref="ArgumentNullException">当<paramref name="routine"/>为<c>null</c>时引发</exception>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            Guard.Requires<ArgumentNullException>(routine != null);
            if (driverBehaviour == null)
            {
                while (routine.MoveNext())
                {
                    var current = routine.Current as IEnumerator;
                    if (current != null)
                    {
                        StartCoroutine(current);
                    }
                }
                return null;
            }
            return driverBehaviour.StartCoroutine(routine);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        /// <exception cref="ArgumentNullException">当<paramref name="routine"/>为<c>null</c>时引发</exception>
        public void StopCoroutine(IEnumerator routine)
        {
            if (driverBehaviour == null)
            {
                return;
            }
            Guard.Requires<ArgumentNullException>(routine != null);
            driverBehaviour.StopCoroutine(routine);
        }

        #endregion

        #region Dispatcher

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="e">事件参数</param>
        public void Trigger(string eventName, EventArgs e)
        {
            if (EventSystem != null)
            {
                EventSystem.Trigger(eventName, e);
            }
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        public void Trigger(string eventName, object sender = null, EventArgs e = null)
        {
            if (EventSystem != null)
            {
                EventSystem.Trigger(eventName, sender, e);
            }
        }

        /// <summary>
        /// 注册一个全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件回调</param>
        /// <param name="life">事件生命，当生命为0则自动释放</param>
        /// <returns>事件句柄</returns>
        public IEventHandler On(string eventName, System.EventHandler handler, int life = 0)
        {
            if (EventSystem != null)
            {
                return EventSystem.On(eventName, handler, life);
            }
            return null;
        }

        /// <summary>
        /// 注册一个一次性的全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件回调</param>
        /// <returns>事件句柄</returns>
        public IEventHandler One(string eventName, System.EventHandler handler)
        {
            if (EventSystem != null)
            {
                return EventSystem.One(eventName, handler);
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 默认的解决事件
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">对象</param>
        /// <returns>处理后的对象</returns>
        private object DefaultOnResolving(IBindData binder , object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (binder.IsStatic)
            {
                Attach(obj);
            }

            return obj;
        }

        /// <summary>
        /// 默认的释放事件
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">对象</param>
        private void DefaultOnRelease(IBindData binder, object obj)
        {
            if (obj == null)
            {
                return;
            }

            if (binder.IsStatic)
            {
                Detach(obj);
            }
        }

        /// <summary>
        /// 转换到指定目标并且删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sortset">有序集</param>
        /// <param name="obj">对象</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndRemove<T>(ISortSet<T, int> sortset, object obj) where T : class
        {
            var ele = obj as T;
            return ele != null && sortset.Remove(ele);
        }

        /// <summary>
        /// 转换到指定目标并且添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sortset">有序集</param>
        /// <param name="obj">对象</param>
        /// <param name="function">获取优先级的函数名</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndAdd<T>(ISortSet<T, int> sortset, object obj , string function) where T : class
        {
            var ele = obj as T;
            if (ele == null)
            {
                return false;
            }
            sortset.Add(ele , GetPriorities(obj.GetType(), function));
            return true;
        }
    }
}