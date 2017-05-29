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
        /// 是否是暂停的
        /// </summary>
        internal bool IsPause { get; set; }

        /// <summary>
        /// 时间实现
        /// </summary>
        private readonly ITime time;

        /// <summary>
        /// 计时器列表
        /// </summary>
        private readonly List<Timer> timers;

        /// <summary>
        /// 当队列中的所有任务完成时
        /// </summary>
        private Action onComplete;

        /// <summary>
        /// 游标,确定了当前执行的timer位置
        /// </summary>
        private int cursor;

        /// <summary>
        /// 当前计时器组是否完成的
        /// </summary>
        private bool IsComplete
        {
            get { return cursor < timers.Count; }
        }

        /// <summary>
        /// 构建一个计时器组
        /// </summary>
        /// <param name="time">时间实现</param>
        public TimerGroup(ITime time)
        {
            this.time = time;
            timers = new List<Timer>();
            cursor = 0;
            IsPause = false;
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
        /// 触发计时器
        /// </summary>
        /// <returns>计时器组是否已经完成</returns>
        internal bool Tick()
        {
            if (IsPause)
            {
                return IsComplete;
            }
            var deltaTime = time.DeltaTime;
            Timer timer;
            while ((timer = GetTimer()) != null)
            {
                if (!timer.Tick(ref deltaTime))
                {
                    break;
                }
                ++cursor;
            }

            if (!IsComplete)
            {
                return false;
            }

            if (onComplete != null)
            {
                onComplete.Invoke();
            }

            return true;
        }

        /// <summary>
        /// 将计时器加入组
        /// </summary>
        /// <param name="timer">计时器</param>
        internal void Add(Timer timer)
        {
            timers.Add(timer);
        }

        /// <summary>
        /// 获取计时器
        /// </summary>
        /// <returns></returns>
        private Timer GetTimer()
        {
            return !IsComplete ? timers[cursor] : null;
        }
    }
}
