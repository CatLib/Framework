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

namespace CatLib.API.Thread
{
    /// <summary>
    /// 线程任务
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 延迟多少秒执行这个线程
        /// </summary>
        /// <param name="time">延迟秒数</param>
        /// <returns>线程任务实例</returns>
        ITask Delay(float time);

        /// <summary>
        /// 当线程任务完成时
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>线程任务实例</returns>
        ITask OnComplete(Action onComplete);

        /// <summary>
        /// 当线程任务完成时
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>线程任务实例</returns>
        ITask OnComplete(Action<object> onComplete);

        /// <summary>
        /// 当线程执行过程时抛出异常
        /// </summary>
        /// <param name="onError">当异常时</param>
        /// <returns>线程任务实例</returns>
        ITask OnError(Action<Exception> onError);

        /// <summary>
        /// 启动线程任务
        /// </summary>
        /// <returns></returns>
        ITaskHandler Start();
    }
}