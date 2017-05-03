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
using CatLib.API;
using CatLib.API.Time;

namespace CatLib.TimeQueue
{
    /// <summary>
    /// 时间队列
    /// </summary>
    public sealed class TimeQueue : ITimeQueue
    {
        /// <summary>
        /// 时间
        /// </summary>
        public ITime Time { get; set; }

        /// <summary>
        /// 当队列中的所有任务完成时
        /// </summary>
        private Action queueOnComplete;

        /// <summary>
        /// 当队列中的所有任务完成时（允许携带一个上下文）
        /// </summary>
        private Action<object> queueOnCompleteWithContext;

        /// <summary>
        /// 上下文
        /// </summary>
        private object context;

        /// <summary>
        /// 队列中的任务
        /// </summary>
        private readonly List<TimeTask> queueTasks = new List<TimeTask>();

        /// <summary>
        /// 队列是否完成
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// 运行器
        /// </summary>
        public TimeRunner Runner { get; set; }

        /// <summary>
        /// 推入队列
        /// </summary>
        /// <param name="task">执行的任务</param>
        /// <returns></returns>
        public ITimeTaskHandler Push(TimeTask task)
        {
            queueTasks.Add(task);
            return task;
        }

        /// <summary>
        /// 撤销执行
        /// </summary>
        /// <param name="task">执行的任务</param>
        public void Cancel(TimeTask task)
        {
            queueTasks.Remove(task);
        }

        /// <summary>
        /// 创建一个任务
        /// </summary>
        /// <param name="task">任务实现</param>
        /// <returns>执行的任务</returns>
        public ITimeTask Task(Action task)
        {
            return new TimeTask(this)
            {
                TaskCall = task
            };
        }

        /// <summary>
        /// 创建一个任务
        /// </summary>
        /// <param name="task">任务实现</param>
        /// <returns>执行的任务</returns>
        public ITimeTask Task(Action<object> task)
        {
            return new TimeTask(this)
            {
                TaskCallWithContext = task
            };
        }

        /// <summary>
        /// 当完成时
        /// </summary>
        /// <param name="onComplete">完成时</param>
        /// <returns>当前队列实例</returns>
        public ITimeQueue OnComplete(Action<object> onComplete)
        {
            queueOnCompleteWithContext = onComplete;
            return this;
        }

        /// <summary>
        /// 当完成时
        /// </summary>
        /// <param name="onComplete">完成时</param>
        /// <returns>当前队列实例</returns>
        public ITimeQueue OnComplete(Action onComplete)
        {
            queueOnComplete = onComplete;
            return this;
        }

        /// <summary>
        /// 设定上下文
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns>当前队列实例</returns>
        public ITimeQueue SetContext(object context)
        {
            this.context = context;
            return this;
        }

        /// <summary>
        /// 暂停队列执行
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Pause()
        {
            return Runner.StopRunner(this);
        }

        /// <summary>
        /// 启动队列
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Play()
        {
            IsComplete = false;
            return Runner.Runner(this);
        }

        /// <summary>
        /// 停止队列执行
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Stop()
        {
            var statu = Runner.StopRunner(this);
            if (statu)
            {
                Reset();
            }
            return statu;
        }

        /// <summary>
        /// 重播队列
        /// </summary>
        /// <returns></returns>
        public bool Replay()
        {
            var statu = Stop();
            if (statu)
            {
                statu = Play();
            }
            return statu;
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            TimeTask task;
            for (var i = 0; i < queueTasks.Count; ++i)
            {
                task = queueTasks[i];
                task.IsComplete = false;
                task.Reset();
            }
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            var isAllComplete = true;
            var deltaTime = Time.DeltaTime;
            bool jumpFlag;
            for (var i = 0; i < queueTasks.Count; ++i)
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

        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private bool RunTask(TimeTask task, ref float deltaTime)
        {
            var action = task.TimeLine[task.TimeLineIndex];

            switch (action.Type)
            {
                case TimeTaskActionTypes.DelayFrame:
                    return TaskDelayFrame(task, ref deltaTime);
                case TimeTaskActionTypes.DelayTime:
                    return TaskDelayTime(task, ref deltaTime);
                case TimeTaskActionTypes.LoopFunc:
                    return TaskLoopFunc(task, ref deltaTime);
                case TimeTaskActionTypes.LoopTime:
                    return TaskLoopTime(task, ref deltaTime);
                case TimeTaskActionTypes.LoopFrame:
                    return TaskLoopFrame(task, ref deltaTime);
                default:
                    return true;
            }
        }

        /// <summary>
        /// 延迟帧执行
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private bool TaskDelayFrame(TimeTask task, ref float deltaTime)
        {
            var action = task.TimeLine[task.TimeLineIndex];
            if (action.IntArgs[0] < 0 || action.IntArgs[1] >= action.IntArgs[0])
            {
                return false;
            }
            action.IntArgs[1] += 1;
            deltaTime = 0;
            if (action.IntArgs[1] < action.IntArgs[0])
            {
                return false;
            }
            task.TimeLineIndex++;
            CallTask(task);
            return true;
        }

        /// <summary>
        /// 延迟时间执行
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private bool TaskDelayTime(TimeTask task, ref float deltaTime)
        {
            var action = task.TimeLine[task.TimeLineIndex];

            if (!(action.FloatArgs[0] >= 0) || !(action.FloatArgs[1] < action.FloatArgs[0]))
            {
                return false;
            }

            action.FloatArgs[1] += deltaTime;

            if (!(action.FloatArgs[1] >= action.FloatArgs[0]))
            {
                return false;
            }

            deltaTime = action.FloatArgs[1] - action.FloatArgs[0];
            task.TimeLineIndex++;
            CallTask(task);
            return true;
        }

        /// <summary>
        /// 根据函数结果决定是否循环
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private bool TaskLoopFunc(TimeTask task, ref float deltaTime)
        {
            var action = task.TimeLine[task.TimeLineIndex];

            if (!action.FuncBoolArg())
            {
                task.TimeLineIndex++;
                return true;
            }

            CallTask(task);
            return false;
        }

        /// <summary>
        /// 循环执行指定的时间
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private bool TaskLoopTime(TimeTask task, ref float deltaTime)
        {
            var action = task.TimeLine[task.TimeLineIndex];

            if (action.FloatArgs[0] >= 0 && action.FloatArgs[1] <= action.FloatArgs[0])
            {
                action.FloatArgs[1] += deltaTime;

                if (action.FloatArgs[1] > action.FloatArgs[0])
                {

                    deltaTime = action.FloatArgs[1] - action.FloatArgs[0];
                    task.TimeLineIndex++;
                    return true;

                }
            }

            CallTask(task);
            return false;
        }

        /// <summary>
        /// 循环执行指定帧数
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private bool TaskLoopFrame(TimeTask task, ref float deltaTime)
        {
            var action = task.TimeLine[task.TimeLineIndex];
            if (action.IntArgs[0] >= 0 && action.IntArgs[1] <= action.IntArgs[0])
            {
                action.IntArgs[1] += 1;
                deltaTime = 0;
                if (action.IntArgs[1] > action.IntArgs[0])
                {
                    task.TimeLineIndex++;
                    return true;
                }
            }

            CallTask(task);
            return false;
        }

        /// <summary>
        /// 激活当任务完成时事件
        /// </summary>
        /// <param name="task">任务</param>
        private void CallTaskComplete(TimeTask task)
        {
            task.IsComplete = true;
            if (task.OnCompleteTask != null)
            {
                task.OnCompleteTask.Invoke();
            }
            if (task.OnCompleteTaskWithContext != null)
            {
                task.OnCompleteTaskWithContext.Invoke(context);
            }
        }

        /// <summary>
        /// 调用任务实现
        /// </summary>
        /// <param name="task">任务</param>
        private void CallTask(TimeTask task)
        {
            if (task.TaskCall != null)
            {
                task.TaskCall.Invoke();
            }
            if (task.TaskCallWithContext != null)
            {
                task.TaskCallWithContext.Invoke(context);
            }
        }

        /// <summary>
        /// 触发队列完成
        /// </summary>
        private void QueueComplete()
        {
            if (queueOnComplete != null)
            {
                queueOnComplete.Invoke();
            }
            if (queueOnCompleteWithContext != null)
            {
                queueOnCompleteWithContext.Invoke(context);
            }
            IsComplete = true;
        }
    }
}