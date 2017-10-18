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
using System.Diagnostics;
using System.Threading;

namespace CatLib.Tick
{
    /// <summary>
    /// 时间摆钟
    /// </summary>
    internal sealed class TimeTicker : IDisposable
    {
        /// <summary>
        /// Bool释放器
        /// </summary>
        private class BooleanDisposable : IDisposable
        {
            /// <summary>
            /// 是否被释放
            /// </summary>
            public bool IsDispose { get; private set; }

            /// <summary>
            /// Bool释放器
            /// </summary>
            public BooleanDisposable()
            {
                IsDispose = false;
            }

            /// <summary>
            /// 释放
            /// </summary>
            public void Dispose()
            {
                IsDispose = true;
            }
        }

        /// <summary>
        /// 需要进行滴答的列表
        /// </summary>
        private readonly List<ITick> ticks;

        /// <summary>
        /// 释放句柄
        /// </summary>
        private readonly BooleanDisposable disposable;

        /// <summary>
        /// 每秒的帧率
        /// </summary>
        private readonly int fps;

        /// <summary>
        /// 每秒的帧率
        /// </summary>
        public int Fps
        {
            get { return fps; }
        }

        /// <summary>
        /// 每帧所需求的毫秒
        /// </summary>
        private readonly int frame;

        /// <summary>
        /// 计时器
        /// </summary>
        private readonly Stopwatch stopWatch;

        /// <summary>
        /// 最后一帧运行时长
        /// </summary>
        private int lastDuration;

        /// <summary>
        /// 时间摆钟
        /// </summary>
        /// <param name="fps">每秒的帧率(0-1000)</param>
        /// <param name="container">容器</param>
        public TimeTicker(int fps, [Inject(Required = true)]IContainer container)
        {
            Guard.Requires<ArgumentNullException>(container != null);
            Guard.Requires<ArgumentOutOfRangeException>(fps >= 1);
            Guard.Requires<ArgumentOutOfRangeException>(fps <= 1000);

            ticks = new List<ITick>();
            disposable = new BooleanDisposable();
            this.fps = fps;
            lastDuration = frame = Math.Max(1, (int) Math.Floor(1000.0f / fps));
            stopWatch = new Stopwatch();

            container.OnResolving(OnResolving);
            container.OnRelease(OnRelease);

            ThreadPool.QueueUserWorkItem(TickThread, disposable);
        }

        /// <summary>
        /// 释放时
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }

        /// <summary>
        /// 当解决时
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">实例</param>
        /// <returns>处理后的实例</returns>
        private object OnResolving(IBindData binder, object obj)
        {
            var tick = obj as ITick;
            if (tick == null || !binder.IsStatic)
            {
                return obj;
            }

            lock (ticks)
            {
                ticks.Add(tick);
            }

            return obj;
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">实例</param>
        /// <returns>处理后的实例</returns>
        private void OnRelease(IBindData binder, object obj)
        {
            var tick = obj as ITick;
            if (tick == null)
            {
                return;
            }

            lock (ticks)
            {
                ticks.Remove(tick);
            }
        }

        /// <summary>
        /// 计时线程
        /// </summary>
        /// <param name="payload">载荷</param>
        private void TickThread(object payload)
        {
            while (!disposable.IsDispose)
            {
                var time = ClockTime(RunTicks);
                lastDuration = Math.Max(frame, time);
                if ((time = frame - time) > 0)
                {
                    Thread.Sleep(time);
                }
            }
        }

        /// <summary>
        /// 运行Ticks
        /// </summary>
        private void RunTicks()
        {
            lock (ticks)
            {
                foreach (var tick in ticks)
                {
                    tick.Tick(lastDuration);
                }
            }
        }

        /// <summary>
        /// 计算时间
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private int ClockTime(Action action)
        {
            try
            {
                stopWatch.Start();
                action.Invoke();
                stopWatch.Stop();
                return (int)stopWatch.ElapsedMilliseconds;
            }
            finally
            {
                stopWatch.Reset();
            }
        }
    }
}
