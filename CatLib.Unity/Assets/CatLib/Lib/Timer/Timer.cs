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
using CatLib.API;
using CatLib.API.Timer;
using CatLib.Stl;

namespace CatLib.Timer
{
    /// <summary>
    /// 计时器
    /// </summary>
    internal sealed class Timer : ITimer
    {
        /// <summary>
        /// 计时器参数
        /// </summary>
        private class TimerArgs
        {
            /// <summary>
            /// 任务类型
            /// </summary>
            internal TimerTypes Type { get; set; }

            /// <summary>
            /// 整型参数
            /// </summary>
            internal int[] IntArgs { get; set; }

            /// <summary>
            /// 浮点型参数
            /// </summary>
            internal float[] FloatArgs { get; set; }

            /// <summary>
            /// 布尔回调函数
            /// </summary>
            internal Func<bool> FuncBoolArg { get; set; }
        }

        /// <summary>
        /// 任务行为
        /// </summary>
        private readonly Action task;

        /// <summary>
        /// 计时器参数
        /// </summary>
        private TimerArgs args;

        /// <summary>
        /// 当前计时器是否已经被完成
        /// </summary>
        private bool isComplete;

        /// <summary>
        /// 计时器组
        /// </summary>
        public ITimerGroup Group { get; private set; }

        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="task">任务实现</param>
        /// <param name="group">当前逻辑帧</param>
        /// <param name="frame">当前逻辑帧</param>
        /// <returns>执行的任务</returns>
        public Timer(ITimerGroup group, Action task)
        {
            this.task = task;
            Group = group;
            isComplete = false;
        }

        /// <summary>
        /// 延迟指定时间后执行
        /// </summary>
        /// <param name="time">延迟时间(秒)</param>
        public void Delay(float time)
        {
            GuardComplete("Delay");
            Guard.Requires<ArgumentOutOfRangeException>(time > 0);
            args = new TimerArgs
            {
                Type = TimerTypes.DelayTime,
                FloatArgs = new[] { time, 0 }
            };
        }

        /// <summary>
        /// 延迟指定帧数帧后执行
        /// </summary>
        /// <param name="frame">帧数</param>
        public void DelayFrame(int frame)
        {
            GuardComplete("DelayFrame");
            Guard.Requires<ArgumentOutOfRangeException>(frame > 0);
            args = new TimerArgs
            {
                Type = TimerTypes.DelayFrame,
                IntArgs = new[] { frame, 0 }
            };
        }

        /// <summary>
        /// 循环执行指定时间
        /// </summary>
        /// <param name="time">循环时间(秒)</param>
        public void Loop(float time)
        {
            GuardComplete("Loop");
            Guard.Requires<ArgumentOutOfRangeException>(time > 0);
            args = new TimerArgs
            {
                Type = TimerTypes.LoopTime,
                FloatArgs = new[] { time, 0 }
            };
        }

        /// <summary>
        /// 循环执行，直到函数返回false
        /// </summary>
        /// <param name="callback">循环状态函数</param>
        public void Loop(Func<bool> callback)
        {
            GuardComplete("Loop");
            Guard.NotNull(callback, "callback");
            args = new TimerArgs
            {
                Type = TimerTypes.LoopFunc,
                FuncBoolArg = callback
            };
        }

        /// <summary>
        /// 循环执行指定帧数
        /// </summary>
        /// <param name="frame">循环执行的帧数</param>
        public void LoopFrame(int frame)
        {
            GuardComplete("LoopFrame");
            Guard.Requires<ArgumentOutOfRangeException>(frame > 0);
            frame = Math.Max(0, frame);
            args = new TimerArgs
            {
                Type = TimerTypes.LoopFrame,
                IntArgs = new[] { frame, 0 }
            };
        }

        /// <summary>
        /// 间隔多少时间执行一次
        /// </summary>
        /// <param name="time">间隔的时间</param>
        public void Interval(float time)
        {
            GuardComplete("Interval");
            Guard.Requires<ArgumentOutOfRangeException>(time > 0);
            if (task == null)
            {
                throw new RuntimeException("Timer Task can not be null");
            }

            time = Math.Max(0, time);
            args = new TimerArgs
            {
                Type = TimerTypes.Interval,
                FloatArgs = new[] { time, 0 }
            };
        }

        /// <summary>
        /// 间隔多少帧执行一次
        /// </summary>
        /// <param name="frame">间隔的帧数</param>
        public void IntervalFrame(int frame)
        {
            GuardComplete("IntervalFrame");
            Guard.Requires<ArgumentOutOfRangeException>(frame > 0);
            if (task == null)
            {
                throw new RuntimeException("Timer Task can not be null");
            }

            args = new TimerArgs
            {
                Type = TimerTypes.IntervalFrame,
                IntArgs = new[] { frame, 0 }
            };
        }

        /// <summary>
        /// 触发计时器
        /// </summary>
        /// <param name="deltaTime">上一帧到当前帧的时间(秒)</param>
        /// <returns>当前计时器是否完成了任务</returns>
        internal bool Tick(ref float deltaTime)
        {
            deltaTime = Math.Max(0, deltaTime);

            if (args == null)
            {
                isComplete = true;
                return true;
            }

            return isComplete = ExecTask(ref deltaTime);
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="deltaTime">上一帧到当前帧的时间(秒)</param>
        /// <returns>计时器是否已经完成</returns>
        private bool ExecTask(ref float deltaTime)
        {
            switch (args.Type)
            {
                case TimerTypes.DelayFrame:
                    return TaskDelayFrame(this, ref deltaTime);
                case TimerTypes.DelayTime:
                    return TaskDelayTime(this, ref deltaTime);
                case TimerTypes.LoopFunc:
                    return TaskLoopFunc(this, ref deltaTime);
                case TimerTypes.LoopTime:
                    return TaskLoopTime(this, ref deltaTime);
                case TimerTypes.LoopFrame:
                    return TaskLoopFrame(this, ref deltaTime);
                case TimerTypes.Interval:
                    return TaskInterval(this, ref deltaTime);
                case TimerTypes.IntervalFrame:
                    return TaskIntervalFrame(this, ref deltaTime);
                default:
                    throw new RuntimeException("Undefined TimerTypes.");
            }
        }

        /// <summary>
        /// 延迟帧执行
        /// </summary>
        /// <param name="timer">计时器</param>
        /// <param name="deltaTime">上一帧到当前帧的时间(秒)</param>
        /// <returns>是否完成</returns>
        private static bool TaskDelayFrame(Timer timer, ref float deltaTime)
        {
            if (++timer.args.IntArgs[1] < timer.args.IntArgs[0])
            {
                deltaTime = 0;
                return false;
            }

            if (timer.task != null)
            {
                timer.task.Invoke();
            }
            return true;
        }

        /// <summary>
        /// 延迟时间执行
        /// </summary>
        /// <param name="timer">计时器</param>
        /// <param name="deltaTime">上一帧到当前帧的时间(秒)</param>
        /// <returns>是否完成</returns>
        private static bool TaskDelayTime(Timer timer, ref float deltaTime)
        {
            timer.args.FloatArgs[1] += deltaTime;
            if (timer.args.FloatArgs[1] < timer.args.FloatArgs[0])
            {
                deltaTime = 0;
                return false;
            }

            deltaTime = timer.args.FloatArgs[1] - timer.args.FloatArgs[0];

            if (timer.task != null)
            {
                timer.task.Invoke();
            }
            return true;
        }

        /// <summary>
        /// 根据函数结果决定是否循环
        /// </summary>
        /// <param name="timer">计时器</param>
        /// <param name="deltaTime">上一帧到当前帧的时间(秒)</param>
        /// <returns>是否完成</returns>
        private static bool TaskLoopFunc(Timer timer, ref float deltaTime)
        {
            if (!timer.args.FuncBoolArg.Invoke())
            {
                return true;
            }

            if (timer.task != null)
            {
                timer.task.Invoke();
            }
            return false;
        }

        /// <summary>
        /// 循环执行指定的时间
        /// </summary>
        /// <param name="timer">计时器</param>
        /// <param name="deltaTime">上一帧到当前帧的时间(秒)</param>
        /// <returns>是否完成</returns>
        private static bool TaskLoopTime(Timer timer, ref float deltaTime)
        {
            timer.args.FloatArgs[1] += deltaTime;
            if (timer.args.FloatArgs[1] > timer.args.FloatArgs[0])
            {
                deltaTime = timer.args.FloatArgs[1] - timer.args.FloatArgs[0];
                return true;
            }

            if (timer.task != null)
            {
                timer.task.Invoke();
            }
            return false;
        }

        /// <summary>
        /// 循环执行指定帧数
        /// </summary>
        /// <param name="timer">计时器</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private static bool TaskLoopFrame(Timer timer, ref float deltaTime)
        {
            if (++timer.args.IntArgs[1] > timer.args.IntArgs[0])
            {
                deltaTime = 0;
                return true;
            }

            if (timer.task != null)
            {
                timer.task.Invoke();
            }
            return false;
        }

        /// <summary>
        /// 间隔指定时间执行
        /// </summary>
        /// <param name="timer">计时器</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private static bool TaskInterval(Timer timer, ref float deltaTime)
        {
            timer.args.FloatArgs[1] += deltaTime;
            while (timer.args.FloatArgs[1] >= timer.args.FloatArgs[0])
            {
                timer.args.FloatArgs[1] -= timer.args.FloatArgs[0];
                if (timer.task != null)
                {
                    timer.task.Invoke();
                }
            }

            return false;
        }

        /// <summary>
        /// 间隔指定时间执行
        /// </summary>
        /// <param name="timer">计时器</param>
        /// <param name="deltaTime">一帧的时间</param>
        /// <returns>是否完成</returns>
        private static bool TaskIntervalFrame(Timer timer, ref float deltaTime)
        {
            ++timer.args.IntArgs[1];
            while (timer.args.IntArgs[1] >= timer.args.IntArgs[0])
            {
                timer.args.IntArgs[1] -= timer.args.IntArgs[0];
                if (timer.task != null)
                {
                    timer.task.Invoke();
                }
            }

            return false;
        }

        /// <summary>
        /// 检测完成状态
        /// </summary>
        /// <param name="func">函数名</param>
        private void GuardComplete(string func)
        {
            if (isComplete)
            {
                throw new RuntimeException("Timer is complete , Can not call " + func + "();");
            }
        }
    }
}
