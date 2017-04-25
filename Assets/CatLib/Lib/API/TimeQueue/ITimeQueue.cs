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

namespace CatLib.API.TimeQueue
{
    /// <summary>
    /// 时间队列
    /// </summary>
    public interface ITimeQueue
    {
        /// <summary>
        /// 创建一个任务
        /// </summary>
        /// <param name="task">任务实现</param>
        /// <returns>执行的任务</returns>
        ITimeTask Task(Action task);

        /// <summary>
        /// 创建一个任务
        /// </summary>
        /// <param name="task">任务实现</param>
        /// <returns>执行的任务</returns>
        ITimeTask Task(Action<object> task);

        /// <summary>
        /// 当完成时
        /// </summary>
        /// <param name="onComplete">完成时</param>
        /// <returns>当前队列实例</returns>
        ITimeQueue OnComplete(Action<object> onComplete);

        /// <summary>
        /// 当完成时
        /// </summary>
        /// <param name="onComplete">完成时</param>
        /// <returns>当前队列实例</returns>
        ITimeQueue OnComplete(Action onComplete);

        /// <summary>
        /// 设定上下文
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns>当前队列实例</returns>
        ITimeQueue SetContext(object context);

        /// <summary>
        /// 暂停队列执行
        /// </summary>
        /// <returns>是否成功</returns>
        bool Pause();

        /// <summary>
        /// 启动队列
        /// </summary>
        /// <returns>是否成功</returns>
        bool Play();

        /// <summary>
        /// 停止队列执行
        /// </summary>
        /// <returns>是否成功</returns>
        bool Stop();

        /// <summary>
        /// 重播队列
        /// </summary>
        /// <returns></returns>
        bool Replay();
    }
}