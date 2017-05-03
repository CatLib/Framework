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

using CatLib.API.TimeQueue;
using System;
using System.Collections.Generic;

namespace CatLib.TimeQueue
{
    /// <summary>
    /// 时间任务
    /// </summary>
    public sealed class TimeTask : ITimeTask, ITimeTaskHandler
    {
        /// <summary>
        /// 父级队列
        /// </summary>
        private readonly TimeQueue queue;

        /// <summary>
        /// 时间线，时间线规定了运行计划
        /// </summary>
        private readonly List<TimeTaskAction> timeLine = new List<TimeTaskAction>();

        /// <summary>
        /// 时间线
        /// </summary>
        internal List<TimeTaskAction> TimeLine
        {
            get { return timeLine; }
        }

        /// <summary>
        /// 任务实现
        /// </summary>
        public Action TaskCall { get; set; }

        /// <summary>
        /// 任务实现并允许附加一个上下文
        /// </summary>
        public Action<object> TaskCallWithContext { get; set; }

        /// <summary>
        /// 当任务完成时
        /// </summary>
        public Action OnCompleteTask { get; set; }

        /// <summary>
        /// 当任务完成时并附加一个上下文
        /// </summary>
        public Action<object> OnCompleteTaskWithContext { get; set; }

        /// <summary>
        /// 任务是否完成
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// 时间线下标
        /// </summary>
        public int TimeLineIndex { get; set; }

        /// <summary>
        /// 构建一个时间任务
        /// </summary>
        /// <param name="queue">父级队列</param>
        public TimeTask(TimeQueue queue)
        {
            this.queue = queue;
            TimeLineIndex = 0;
        }

        /// <summary>
        /// 延迟帧执行
        /// </summary>
        /// <param name="frame">帧数</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask DelayFrame(int frame)
        {
            timeLine.Add(new TimeTaskAction
            {
                Type = TimeTaskActionTypes.DelayFrame,
                IntArgs = new[] { frame, 0 }
            });
            return this;
        }

        /// <summary>
        /// 延迟时间执行
        /// </summary>
        /// <param name="time">延迟时间(秒)</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask Delay(float time)
        {
            timeLine.Add(new TimeTaskAction
            {
                Type = TimeTaskActionTypes.DelayTime,
                FloatArgs = new[] { time, 0 }
            });
            return this;
        }

        /// <summary>
        /// 循环执行指定时间
        /// </summary>
        /// <param name="time">循环时间(秒)</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask Loop(float time)
        {
            timeLine.Add(new TimeTaskAction
            {
                Type = TimeTaskActionTypes.LoopTime,
                FloatArgs = new[] { time, 0 }
            });
            return this;
        }

        /// <summary>
        /// 循环执行，直到函数返回false
        /// </summary>
        /// <param name="loopFunc">循环状态函数</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask Loop(Func<bool> loopFunc)
        {
            timeLine.Add(new TimeTaskAction
            {
                Type = TimeTaskActionTypes.LoopFunc,
                FuncBoolArg = loopFunc
            });
            return this;
        }

        /// <summary>
        /// 循环执行指定帧数
        /// </summary>
        /// <param name="frame">循环的帧数</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask LoopFrame(int frame)
        {
            timeLine.Add(new TimeTaskAction
            {
                Type = TimeTaskActionTypes.LoopFrame,
                IntArgs = new[] { frame, 0 }
            });
            return this;
        }

        /// <summary>
        /// 当任务完成时
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask OnComplete(Action onComplete)
        {
            OnCompleteTask = onComplete;
            return this;
        }

        /// <summary>
        /// 当任务完成时并附加一个上下文
        /// </summary>
        /// <param name="onComplete">完成时的回调</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask OnComplete(Action<object> onComplete)
        {
            OnCompleteTaskWithContext = onComplete;
            return this;
        }

        /// <summary>
        /// 增加一个任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask Task(Action task)
        {
            Push();
            return queue.Task(task);
        }

        /// <summary>
        /// 增加一个任务并接受一个上下文
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>当前任务实例</returns>
        public ITimeTask Task(Action<object> task)
        {
            Push();
            return queue.Task(task);
        }

        /// <summary>
        /// 将任务推入队列
        /// </summary>
        /// <returns>当前任务句柄</returns>
        public ITimeTaskHandler Push()
        {
            return queue.Push(this);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <returns>时间队列</returns>
        public ITimeQueue Play()
        {
            Push();
            queue.Play();
            return queue;
        }

        /// <summary>
        /// 撤销任务执行
        /// </summary>
        public void Cancel()
        {
            queue.Cancel(this);
        }

        /// <summary>
        /// 重置任务状态
        /// </summary>
        public void Reset()
        {
            for (var i = 0; i < TimeLine.Count; i++)
            {
                switch (TimeLine[i].Type)
                {
                    case TimeTaskActionTypes.LoopFrame:
                    case TimeTaskActionTypes.DelayFrame:
                        TimeLine[i].IntArgs[1] = 0;
                        break;
                    case TimeTaskActionTypes.DelayTime:
                    case TimeTaskActionTypes.LoopTime:
                        TimeLine[i].FloatArgs[1] = 0;
                        break;
                    case TimeTaskActionTypes.LoopFunc:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            TimeLineIndex = 0;
        }
    }
}