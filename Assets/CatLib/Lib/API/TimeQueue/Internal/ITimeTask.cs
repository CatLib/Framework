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
    /// 时间任务
    /// </summary>
    public interface ITimeTask
    {
        /// <summary>
        /// 延迟时间执行
        /// </summary>
        /// <param name="time">延迟时间(秒)</param>
        /// <returns>当前任务实例</returns>
        ITimeTask Delay(float time);

        /// <summary>
        /// 延迟帧执行
        /// </summary>
        /// <param name="frame">帧数</param>
        /// <returns>当前任务实例</returns>
        ITimeTask DelayFrame(int frame);

        /// <summary>
        /// 循环执行指定时间
        /// </summary>
        /// <param name="time">循环时间(秒)</param>
        /// <returns>当前任务实例</returns>
        ITimeTask Loop(float time);

        /// <summary>
        /// 循环执行，直到函数返回false
        /// </summary>
        /// <param name="loopFunc">循环状态函数</param>
        /// <returns>当前任务实例</returns>
        ITimeTask Loop(Func<bool> loopFunc);

        /// <summary>
        /// 循环执行指定帧数
        /// </summary>
        /// <param name="frame">循环的帧数</param>
        /// <returns>当前任务实例</returns>
        ITimeTask LoopFrame(int frame);

        /// <summary>
        /// 当任务完成时
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>当前任务实例</returns>
        ITimeTask OnComplete(Action onComplete);

        /// <summary>
        /// 当任务完成时并附加一个上下文
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>当前任务实例</returns>
        ITimeTask OnComplete(Action<object> onComplete);

        /// <summary>
        /// 增加一个任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>当前任务实例</returns>
        ITimeTask Task(Action task);

        /// <summary>
        /// 增加一个任务并接受一个上下文
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>当前任务实例</returns>
        ITimeTask Task(Action<object> task);

        /// <summary>
        /// 将任务推入队列
        /// </summary>
        /// <returns>当前任务句柄</returns>
        ITimeTaskHandler Push();

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <returns>时间队列</returns>
        ITimeQueue Play();
    }
}