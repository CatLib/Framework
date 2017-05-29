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
        /// 计时器组
        /// </summary>
        public ITimerGroup Group { get; private set; }

        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="task">任务实现</param>
        /// <returns>执行的任务</returns>
        public Timer(ITimerGroup group, Action task)
        {
            this.task = task;
            Group = group;
        }

        /// <summary>
        /// 延迟时间执行
        /// </summary>
        /// <param name="time">延迟时间(秒)</param>
        public void Delay(float time)
        {
            args = new TimerArgs
            {
                Type = TimerTypes.DelayTime,
                FloatArgs = new[] { time, 0 }
            };
        }

        /// <summary>
        /// 延迟帧执行
        /// </summary>
        /// <param name="frame">帧数</param>
        public void DelayFrame(int frame)
        {
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
            args = new TimerArgs
            {
                Type = TimerTypes.LoopTime,
                FloatArgs = new[] { time, 0 }
            };
        }

        /// <summary>
        /// 循环执行，直到函数返回false
        /// </summary>
        /// <param name="loopFunc">循环状态函数</param>
        public void Loop(Func<bool> loopFunc)
        {
            args = new TimerArgs
            {
                Type = TimerTypes.LoopFunc,
                FuncBoolArg = loopFunc
            };
        }

        /// <summary>
        /// 循环执行指定帧数
        /// </summary>
        /// <param name="frame">循环的帧数</param>
        public void LoopFrame(int frame)
        {
            args = new TimerArgs
            {
                Type = TimerTypes.LoopFrame,
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
                if (task != null)
                {
                    task.Invoke();
                }
                return true;
            }

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
            if (timer.args.IntArgs[0] < 0 || timer.args.IntArgs[1] >= timer.args.IntArgs[0])
            {
                return false;
            }
            timer.args.IntArgs[1] += 1;
            deltaTime = 0;
            if (timer.args.IntArgs[1] < timer.args.IntArgs[0])
            {
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
            if (!(timer.args.FloatArgs[0] >= 0) || !(timer.args.FloatArgs[1] < timer.args.FloatArgs[0]))
            {
                return false;
            }

            timer.args.FloatArgs[1] += deltaTime;

            if (!(timer.args.FloatArgs[1] >= timer.args.FloatArgs[0]))
            {
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
            if (timer.args.FloatArgs[0] >= 0 && timer.args.FloatArgs[1] <= timer.args.FloatArgs[0])
            {
                timer.args.FloatArgs[1] += deltaTime;

                if (timer.args.FloatArgs[1] > timer.args.FloatArgs[0])
                {
                    deltaTime = timer.args.FloatArgs[1] - timer.args.FloatArgs[0];
                    return true;
                }
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
            if (timer.args.IntArgs[0] >= 0 && timer.args.IntArgs[1] <= timer.args.IntArgs[0])
            {
                timer.args.IntArgs[1] += 1;
                if (timer.args.IntArgs[1] > timer.args.IntArgs[0])
                {
                    deltaTime = 0;
                    return true;
                }
            }

            if (timer.task != null)
            {
                timer.task.Invoke();
            }
            return false;
        }
    }
}
