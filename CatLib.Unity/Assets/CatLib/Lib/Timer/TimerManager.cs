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
using CatLib.API;
using CatLib.API.Time;
using CatLib.API.Timer;
using CatLib.Stl;

namespace CatLib.Timer
{
    /// <summary>
    /// 计时器管理器
    /// 创建计时器的当前逻辑帧不视作一个有效逻辑帧
    /// </summary>
    public sealed class TimerManager : Manager<ITimerGroup> , ITimerManager, IUpdate
    {
        /// <summary>
        /// 时间管理器
        /// </summary>
        private readonly ITimeManager timeManager;

        /// <summary>
        /// 运行列表
        /// </summary>
        private readonly SortSet<TimerGroup, int> executeList;

        /// <summary>
        /// 路由器组
        /// </summary>
        private readonly Stack<TimerGroup> timerGroup;

        /// <summary>
        /// 构建一个计时器管理器
        /// </summary>
        /// <param name="timeManager">时间管理器</param>
        public TimerManager([Inject(Required = true)]ITimeManager timeManager)
        {
            this.timeManager = timeManager;
            executeList = new SortSet<TimerGroup, int>();
            timerGroup = new Stack<TimerGroup>();
        }

        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="task">计时器执行的任务</param>
        /// <returns>计时器</returns>
        public ITimer Make(Action task = null)
        {
            var withGroupStack = timerGroup.Count > 0;
            var group = withGroupStack
                ? timerGroup.Peek()
                : new TimerGroup(timeManager.Default);
            var timer = new Timer(group, task);
            group.Add(timer);
            if (!withGroupStack)
            {
                executeList.Add(group, int.MaxValue);
            }
            return timer;
        }

        /// <summary>
        /// 创建一个计时器组
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>计时器组</returns>
        public ITimerGroup Group(Action area, int priority = int.MaxValue)
        {
            Guard.NotNull(area, "area");
            var group = new TimerGroup(timeManager.Default);
            timerGroup.Push(group);
            try
            {
                area.Invoke();
            }
            finally
            {
                timerGroup.Pop();
            }

            executeList.Add(group, priority);
            return group;
        }

        /// <summary>
        /// 停止计时器组的运行
        /// </summary>
        /// <param name="group">计时器组</param>
        public void Cancel(ITimerGroup group)
        {
            var timerGroup = group as TimerGroup;
            Guard.NotNull(timerGroup, "timerGroup");
            executeList.Remove(timerGroup);
        }

        /// <summary>
        /// 暂停计时器组
        /// </summary>
        /// <param name="group">计时器组</param>
        public void Pause(ITimerGroup group)
        {
            var timerGroup = group as TimerGroup;
            Guard.NotNull(timerGroup, "timerGroup");
            timerGroup.IsPause = true;
        }

        /// <summary>
        /// 重新开始播放计时器组
        /// </summary>
        /// <param name="group">计时器组</param>
        public void Play(ITimerGroup group)
        {
            var timerGroup = group as TimerGroup;
            Guard.NotNull(timerGroup, "timerGroup");
            timerGroup.IsPause = false;
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            foreach (var group in executeList)
            {
                if (group.Tick())
                {
                    executeList.Remove(group);
                }
            }
        }
    }
}
