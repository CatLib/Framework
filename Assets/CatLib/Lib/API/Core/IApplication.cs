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
using CatLib.API.Time;

namespace CatLib.API
{
    /// <summary>
    /// 应用程序接口
    /// </summary>
    public interface IApplication : IContainer, IEventAchieve, IDriver, IEvent
    {
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
        /// 程序时间
        /// </summary>
        ITime Time { get; }
    }
}
