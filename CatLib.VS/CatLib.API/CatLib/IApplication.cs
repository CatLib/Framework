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

using CatLib.API.Stl;
using System;
using System.Collections;
using CatLib.API.Events;

namespace CatLib.API
{
    /// <summary>
    /// 应用程序接口
    /// </summary>
    public interface IApplication : IContainer, IDispatcher
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
        IApplication Bootstrap(params IBootstrap[] bootstraps);

        /// <summary>
        /// 初始化程序
        /// </summary>
        /// <param name="callback">初始化完成后的回调</param>
        void Init(Action callback = null);

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="provider">服务提供者</param>
        void Register(IServiceProvider provider);

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
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程</param>
        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        void StopCoroutine(IEnumerator routine);

        /// <summary>
        /// 从驱动器中卸载对象
        /// 如果对象使用了增强接口，那么卸载对应增强接口
        /// 从驱动器中卸载对象会引发IDestroy增强接口
        /// </summary>
        /// <param name="obj">对象</param>
        void Detach(object obj);

        /// <summary>
        /// 如果对象实现了增强接口那么将对象装载进对应驱动器
        /// </summary>
        /// <param name="obj">对象</param>
        void Attach(object obj);

        /// <summary>
        /// 获取优先级，如果存在方法优先级定义那么优先返回方法的优先级
        /// 如果不存在优先级定义那么返回<c>int.MaxValue</c>
        /// </summary>
        /// <param name="type">获取优先级的类型</param>
        /// <param name="method">获取优先级的调用方法</param>
        /// <returns>优先级</returns>
        int GetPriorities(Type type, string method = null);
    }
}
