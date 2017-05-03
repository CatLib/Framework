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

using CatLib.API.Container;
using CatLib.API.Event;
using System;
using System.Collections;

namespace CatLib.API
{
    /// <summary>
    /// 应用程序接口
    /// </summary>
    public interface IApplication : IContainer, IEventImpl, IEvent
    {
        /// <summary>
        /// CatLib版本号
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 引导程序类型
        /// </summary>
        /// <param name="bootstraps">引导程序</param>
        /// <returns>当前应用程序</returns>
        IApplication Bootstrap(Type[] bootstraps);

        /// <summary>
        /// 初始化程序
        /// </summary>
        void Init();

        /// <summary>
        /// 注册服务提供商
        /// </summary>
        /// <param name="type">服务提供商类型</param>
        void Register(Type type);

        /// <summary>
        /// 获取应用程序内的唯一Id
        /// </summary>
        /// <returns>运行时的唯一Id</returns>
        long GetGuid();

        /// <summary>
        /// 是否是主线程
        /// </summary>
        bool IsMainThread { get; }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">协程，执行会处于主线程</param>
        void MainThread(IEnumerator action);

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">回调，回调的内容会处于主线程</param>
        void MainThread(Action action);

        /// <summary>
        /// 触发一个全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>全局事件</returns>
        IGlobalEvent TriggerGlobal(string eventName);

        /// <summary>
        /// 触发一个全局事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="source">触发事件的源</param>
        /// <returns>全局事件</returns>
        IGlobalEvent TriggerGlobal(string eventName, object source);

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程</param>
        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        void StopCoroutine(IEnumerator routine);
    }
}
