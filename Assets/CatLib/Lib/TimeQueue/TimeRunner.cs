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

using CatLib.API;
using CatLib.API.TimeQueue;
using System.Collections.Generic;
using System.Threading;
using CatLib.API.Time;

namespace CatLib.TimeQueue
{
    /// <summary>
    /// 时间队列运行器
    /// </summary>
    public sealed class TimeRunner : IUpdate
    {
        /// <summary>
        /// 时间
        /// </summary>
        [Inject]
        public ITime Time { get; set; }

        /// <summary>
        /// 运行列表
        /// </summary>
        private readonly List<TimeQueue> timeRuner = new List<TimeQueue>();

        /// <summary>
        /// 锁
        /// </summary>
        private readonly ReaderWriterLockSlim timeRunnerLocker = new ReaderWriterLockSlim();

        /// <summary>
        /// 创建一个时间队列
        /// </summary>
        /// <returns></returns>
        public ITimeQueue CreateQueue()
        {
            return new TimeQueue { Runner = this, Time = Time };
        }

        /// <summary>
        /// 运行一个队列
        /// </summary>
        /// <param name="runner">队列</param>
        /// <returns>是否成功加入</returns>
        public bool Runner(TimeQueue runner)
        {
            try
            {
                timeRunnerLocker.EnterWriteLock();
                try
                {
                    timeRuner.Remove(runner);
                    timeRuner.Add(runner);
                }
                finally { timeRunnerLocker.ExitWriteLock(); }
            }
            catch { return false; }
            return true;
        }

        /// <summary>
        /// 停止一个队列
        /// </summary>
        /// <param name="runner">队列</param>
        /// <returns>是否成功停止</returns>
        public bool StopRunner(TimeQueue runner)
        {
            try
            {
                timeRunnerLocker.EnterWriteLock();
                try
                {
                    timeRuner.Remove(runner);
                }
                finally { timeRunnerLocker.ExitWriteLock(); }
            }
            catch { return false; }
            return true;
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            if (timeRuner.Count <= 0)
            {
                return;
            }
            timeRunnerLocker.EnterWriteLock();
            try
            {
                if (timeRuner.Count <= 0)
                {
                    return;
                }
                var handlersToRemove = new bool[timeRuner.Count];
                for (var i = 0; i < timeRuner.Count; ++i)
                {
                    var runner = timeRuner[i];
                    runner.Update();
                    if (runner.IsComplete)
                    {
                        handlersToRemove[i] = true;
                    }
                }

                for (var i = handlersToRemove.Length - 1; i > -1; --i)
                {
                    if (handlersToRemove[i])
                    {
                        timeRuner.RemoveAt(i);
                    }
                }
            }
            finally
            {
                timeRunnerLocker.ExitWriteLock();
            }
        }
    }
}