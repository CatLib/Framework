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

using CatLib.API.Thread;
using System;

namespace CatLib.Thread
{
    /// <summary>
    /// 线程任务
    /// </summary>
    public sealed class ThreadTask : ITask, ITaskHandler
    {
        /// <summary>
        /// 线程运行器
        /// </summary>
        private readonly ThreadRuner runner;

        /// <summary>
        /// 任务，允许返回一个结果
        /// </summary>
        public Func<object> TaskWithResult { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        public Action Task { get; set; }

        /// <summary>
        /// 完成的结果
        /// </summary>
        public Action<object> CompleteWithResult { get; private set; }

        /// <summary>
        /// 完成时的回调
        /// </summary>
        public Action Complete { get; private set; }

        /// <summary>
        /// 错误时的回调
        /// </summary>
        public Action<Exception> Error { get; private set; }

        /// <summary>
        /// 是否返回结果
        /// </summary>
        public bool ReturnResult { get; set; }

        /// <summary>
        /// 延迟执行时间
        /// </summary>
        public float DelayTime { get; private set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        /// 构造一个线程任务
        /// </summary>
        /// <param name="runner">线程运行器</param>
        public ThreadTask(ThreadRuner runner)
        {
            this.runner = runner;
        }

        /// <summary>
        /// 当线程任务完成时
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>线程任务实例</returns>
        public ITask OnComplete(Action onComplete)
        {
            Complete = onComplete;
            return this;
        }

        /// <summary>
        /// 当线程任务完成时
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>线程任务实例</returns>
        public ITask OnComplete(Action<object> onComplete)
        {
            CompleteWithResult = onComplete;
            return this;
        }

        /// <summary>
        /// 延迟多少秒执行这个线程
        /// </summary>
        /// <param name="time">延迟秒数</param>
        /// <returns>线程任务实例</returns>
        public ITask Delay(float time)
        {
            DelayTime = time;
            return this;
        }

        /// <summary>
        /// 当线程执行过程时抛出异常
        /// </summary>
        /// <param name="onError">当异常时</param>
        /// <returns>线程任务实例</returns>
        public ITask OnError(Action<Exception> onError)
        {
            Error = onError;
            return this;
        }

        /// <summary>
        /// 撤销线程执行，只有在delay状态才能撤销
        /// </summary>
        public void Cancel()
        {
            runner.Cancel(this);
        }

        /// <summary>
        /// 启动线程任务
        /// </summary>
        /// <returns></returns>
        public ITaskHandler Start()
        {
            return runner.AddTask(this);
        }
    }
}