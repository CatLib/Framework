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
using System.Collections.Generic;
using CatLib.API.Time;
using CatLib.API.Timer;

namespace CatLib.Timer
{
    /// <summary>
    /// 计时器组
    /// </summary>
    internal sealed class TimerGroup : ITimerGroup
    {
        /// <summary>
        /// 时间实现
        /// </summary>
        private readonly ITime time;

        /// <summary>
        /// 计时器管理器
        /// </summary>
        private readonly TimerManager manager;

        /// <summary>
        /// 计时器列表
        /// </summary>
        private readonly List<Timer> timers;

        /// <summary>
        /// 当队列中的所有任务完成时
        /// </summary>
        private Action onComplete;

        /// <summary>
        /// 构建一个计时器组
        /// </summary>
        /// <param name="manager">计时器管理器</param>
        /// <param name="time">时间实现</param>
        public TimerGroup(TimerManager manager, ITime time)
        {
            this.time = time;
            this.manager = manager;
            timers = new List<Timer>();
        }

        /// <summary>
        /// 将计时器加入组
        /// </summary>
        /// <param name="timer">计时器</param>
        public void Add(Timer timer)
        {
            timers.Add(timer);
        }

        /// <summary>
        /// 当组的所有计时器完成时
        /// </summary>
        /// <param name="onComplete">完成时</param>
        /// <returns>当前组实例</returns>
        public ITimerGroup OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            var isAllComplete = true;
            var deltaTime = time.DeltaTime;
            bool jumpFlag;
            for (var i = 0; i < timers.Count; ++i)
            {
                if (queueTasks[i].IsComplete)
                {
                    continue;
                }

                isAllComplete = false;
                jumpFlag = false;

                while (queueTasks[i].TimeLineIndex < queueTasks[i].TimeLine.Count)
                {
                    if (RunTask(queueTasks[i], ref deltaTime))
                    {
                        continue;
                    }
                    jumpFlag = true;
                    break;
                }

                if (jumpFlag)
                {
                    break;
                }
                if (deltaTime <= 0)
                {
                    break;
                }

                CallTaskComplete(queueTasks[i]);
            }

            if (isAllComplete)
            {
                QueueComplete();
            }
        }
    }
}
