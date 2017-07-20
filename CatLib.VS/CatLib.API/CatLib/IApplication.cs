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

using CatLib.API.Events;
using CatLib.API.Support;
using System;

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
        void Init();

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
        /// 获取优先级，如果存在方法优先级定义那么优先返回方法的优先级
        /// 如果不存在优先级定义那么返回<c>int.MaxValue</c>
        /// </summary>
        /// <param name="type">获取优先级的类型</param>
        /// <param name="method">获取优先级的调用方法</param>
        /// <returns>优先级</returns>
        int GetPriorities(Type type, string method = null);
    }
}
