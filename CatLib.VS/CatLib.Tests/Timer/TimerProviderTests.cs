﻿/*
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
using CatLib.API.Config;
using CatLib.API.Time;
using CatLib.API.Timer;
using CatLib.Config;
using CatLib.Time;
using CatLib.Timer;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Timer
{
    [TestClass]
    public class TimerProviderTests
    {
        /// <summary>
        /// 这是一个调试的时间
        /// </summary>
        public class TimerTestTime : ITime
        {
            /// <summary>
            /// 从游戏开始到现在所用的时间(秒)
            /// </summary>
            public float Time { get; private set; }

            /// <summary>
            /// 上一帧到当前帧的时间(秒)
            /// </summary>
            public float DeltaTime { get; private set; }

            /// <summary>
            /// 从游戏开始到现在的时间（秒）使用固定时间来更新
            /// </summary>
            public float FixedTime { get; private set; }

            /// <summary>
            /// 从当前scene开始到目前为止的时间(秒)
            /// </summary>
            public float TimeSinceLevelLoad { get; private set; }

            /// <summary>
            /// 固定的上一帧到当前帧的时间(秒)
            /// </summary>
            public float FixedDeltaTime { get; set; }

            /// <summary>
            /// 能获取最大的上一帧到当前帧的时间(秒)
            /// </summary>
            public float MaximumDeltaTime { get; private set; }

            /// <summary>
            /// 平稳的上一帧到当前帧的时间(秒)，根据前N帧的加权平均值
            /// </summary>
            public float SmoothDeltaTime { get; private set; }

            /// <summary>
            /// 时间缩放系数
            /// </summary>
            public float TimeScale { get; set; }

            /// <summary>
            /// 总帧数
            /// </summary>
            public int FrameCount { get; set; }

            /// <summary>
            /// 自游戏开始后的总时间（暂停也会增加）
            /// </summary>
            public float RealtimeSinceStartup { get; private set; }

            /// <summary>
            /// 每秒的帧率
            /// </summary>
            public int CaptureFramerate { get; set; }

            /// <summary>
            /// 不考虑时间缩放上一帧到当前帧的时间(秒)
            /// </summary>
            public float UnscaledDeltaTime { get; private set; }

            /// <summary>
            /// 不考虑时间缩放的从游戏开始到现在的时间
            /// </summary>
            public float UnscaledTime { get; private set; }

            public TimerTestTime()
            {
                Time = 0.25f;
                DeltaTime = 0.25f;
                FixedTime = 0.25f;
                TimeSinceLevelLoad = 0;
                FixedDeltaTime = 0;
                MaximumDeltaTime = 0;
                SmoothDeltaTime = 0;
                TimeScale = 0;
                FrameCount = 0;
                RealtimeSinceStartup = 0;
                CaptureFramerate = 0;
                UnscaledDeltaTime = 0;
                UnscaledTime = 0;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var app = new CatLib.Core.Application().Bootstrap();
            app.Register(typeof(TimeProvider));
            app.Register(typeof(ConfigProvider));
            app.Register(typeof(TimerProvider));
            app.Init();

            var manager = app.Make<ITimeManager>();
            manager.Extend(() => new TimerTestTime(), "test");

            var config = app.Make<IConfigManager>();
            config.Default.Set("times.default", "test");
        }

        [TestMethod]
        public void TestTimerSimpleDelay()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = false;
            timer.Make(() =>
            {
                statu = !statu;
            }).Delay(1);

            var app = App.Instance as CatLib.Core.Application;

            //0.25
            Assert.AreEqual(false, statu);
            NextFrame();
            app.Update();

            //0.5
            Assert.AreEqual(false, statu);
            NextFrame();
            app.Update();

            //0.75
            Assert.AreEqual(false, statu);
            NextFrame();
            app.Update();

            //1.0
            Assert.AreEqual(false, statu);
            NextFrame();
            app.Update();

            Assert.AreEqual(true, statu);
            NextFrame();
            app.Update();

            Assert.AreEqual(true, statu);
        }

        [TestMethod]
        public void TestInvalidTimerDelay()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = false;

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                timer.Make(() =>
                {
                    statu = !statu;
                }).Delay(0);
            });
        }

        [TestMethod]
        public void TestTimerSimpleLoop()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            timer.Make(() =>
            {
                ++statu;
            }).Loop(1);

            RunTime(App.Instance, 2);
            Assert.AreEqual(4, statu);
        }

        [TestMethod]
        public void TestInvalidTimerSimpleLoop()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                timer.Make(() =>
                {
                    ++statu;
                }).Loop(0);
            });
        }

        [TestMethod]
        public void TestTimerLoopFrame()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            timer.Make(() =>
            {
                ++statu;
            }).LoopFrame(3);

            RunFrame(App.Instance, 3);
            Assert.AreEqual(3, statu);
        }

        [TestMethod]
        public void TestInvalidTimerLoopFrame()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                timer.Make(() =>
                {
                    ++statu;
                }).LoopFrame(0);
            });
        }

        [TestMethod]
        public void TestTimerLoopWithFunc()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            timer.Make(() =>
            {
                ++statu;
            }).Loop(() =>
            {
                return statu < 3;
            });

            RunTime(App.Instance, 5);
            Assert.AreEqual(3, statu);
        }

        [TestMethod]
        public void TestTimerSimpleDelayFrame()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = false;
            timer.Make(() =>
            {
                statu = !statu;
            }).DelayFrame(3);

            RunFrame(App.Instance, 2);

            Assert.AreEqual(false , statu);
            RunFrame(App.Instance, 1);
            Assert.AreEqual(true, statu);
            RunFrame(App.Instance, 1);
            Assert.AreEqual(true, statu);
            RunFrame(App.Instance, 1);
            Assert.AreEqual(true, statu);
        }

        [TestMethod]
        public void TestTimerSimpleDelayFrameOne()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = false;
            timer.Make(() =>
            {
                statu = !statu;
            }).DelayFrame(1);

            RunFrame(App.Instance, 1);
            Assert.AreEqual(true, statu);
            RunFrame(App.Instance, 1);
            Assert.AreEqual(true, statu);
            RunFrame(App.Instance, 1);
            Assert.AreEqual(true, statu);
        }

        [TestMethod]
        public void TimerCompleteToCall()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = false;
            var t = timer.Make(() =>
            {
                statu = !statu;
            });
            t.DelayFrame(1);
            RunFrame(App.Instance, 1);
            Assert.AreEqual(true, statu);

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                t.Delay(1);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                t.DelayFrame(1);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                t.Loop(1);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                t.LoopFrame(1);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                t.Loop(() =>
                {
                    return false;
                });
            });
        }

        [TestMethod]
        public void TimerGroupMultitaskingTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            var isComplete = false;

            timer.MakeQueue(() =>
            {
                timer.Make(() =>
                {
                    statu++;
                }).Delay(1);

                timer.Make(() =>
                {
                    statu++;
                }).LoopFrame(4);
            }).OnComplete(() =>
            {
                isComplete = true;
            });

            RunTime(App.Instance, 10);
            Assert.AreEqual(5 , statu);
            Assert.AreEqual(true, isComplete);
        }

        [TestMethod]
        public void TimerGroupMergeMultitaskingTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            var isComplete = false;

            timer.MakeQueue(() =>
            {
                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.1f);

                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.1f);
            }).OnComplete(() =>
            {
                isComplete = true;
            });

            //在测试下一帧为0.25秒，这里2个延迟总共为0.2秒所以可以在1帧内完成
            RunFrame(App.Instance, 1);

            Assert.AreEqual(2, statu);
            Assert.AreEqual(true, isComplete);
        }

        [TestMethod]
        public void TimerGroupMergeComplexMultitaskingTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            var isComplete = false;

            timer.MakeQueue(() =>
            {
                timer.Make(() =>
                {
                    statu++;
                }).DelayFrame(1);

                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.1f);

                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.15f);
            }).OnComplete(() =>
            {
                isComplete = true;
            });

            //固定的等待帧占据整个帧的时间
            //在测试下一帧为0.25秒，这里2个延迟总共为0.25秒所以可以在1帧内完成
            RunFrame(App.Instance, 2);

            Assert.AreEqual(3, statu);
            Assert.AreEqual(true, isComplete);
        }

        [TestMethod]
        public void GroupCompleteCallTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var group = timer.MakeQueue(() =>
            {
            });

            RunFrame(App.Instance, 1);

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                group.OnComplete(() =>
                {

                });
            });
        }

        [TestMethod]
        public void CancelTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            var isComplete = false;

            ITimerQueue queue = null;
            queue = timer.MakeQueue(() =>
            {
                timer.Make(() =>
                {
                    timer.Cancel(queue);
                    statu++;
                }).DelayFrame(1);

                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.1f);

                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.3f);
            }).OnComplete(() =>
            {
                isComplete = true;
            });

            RunFrame(App.Instance, 5);

            Assert.AreEqual(2, statu);
            Assert.AreEqual(false, isComplete);
        }

        [TestMethod]
        public void PauseTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            var isComplete = false;

            ITimerQueue queue = null;
            queue = timer.MakeQueue(() =>
            {
                timer.Make(() =>
                {
                    timer.Pause(queue);
                    statu++;
                }).DelayFrame(1);

                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.1f);

                timer.Make(() =>
                {
                    statu++;
                }).Delay(0.3f);
            }).OnComplete(() =>
            {
                isComplete = true;
            });

            RunFrame(App.Instance, 5);

            Assert.AreEqual(2, statu);
            Assert.AreEqual(false, isComplete);
            Assert.AreEqual(true, queue.IsPause);

            timer.Play(queue);

            RunFrame(App.Instance, 5);

            Assert.AreEqual(3, statu);
            Assert.AreEqual(true, isComplete);
            Assert.AreEqual(false, queue.IsPause);
        }

        [TestMethod]
        public void TimerGetGroupTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            ITimer result = null;
            var group = timer.MakeQueue(() =>
            {
                result = timer.Make();
            });

            Assert.AreSame(group, result.Queue);
        }

        [TestMethod]
        public void NoParamsTest()
        {
            var timer = App.Instance.Make<ITimerManager>();

            timer.MakeQueue(() =>
            {
                timer.Make();
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                RunTime(App.Instance, 1);
            });
        }

        [TestMethod]
        public void NoParamsHasTaskTest()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;
            timer.MakeQueue(() =>
            {
                timer.Make(() =>
                {
                    statu++;
                });
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                RunTime(App.Instance, 1);
            });

            Assert.AreEqual(0, statu);
        }

        /// <summary>
        /// 间隔执行测试
        /// </summary>
        [TestMethod]
        public void TestInterval()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            timer.Make(() =>
            {
                statu++;
            }).Interval(3);

            RunTime(App.Instance, 9);

            Assert.AreEqual(3, statu);
        }

        /// <summary>
        /// 间隔执行测试(边界)
        /// </summary>
        [TestMethod]
        public void TestIntervalBound()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            timer.Make(() =>
            {
                statu++;
            }).Interval(3);

            RunTime(App.Instance, 8.74999f);

            Assert.AreEqual(2, statu);
        }

        [TestMethod]
        public void TestIntervalLifeBound()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            timer.Make(() =>
                {
                    statu++;
                })
                .Interval(3, 1);

            RunTime(App.Instance, 30);

            Assert.AreEqual(1, statu);
        }

        [TestMethod]
        public void TestIntervalInvalid()
        {
            var timer = App.Instance.Make<ITimerManager>();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                timer.Make().Interval(3);
            });
        }

        /// <summary>
        /// 间隔执行测试
        /// </summary>
        [TestMethod]
        public void TestIntervalFrame()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            timer.Make(() =>
            {
                statu++;
            }).IntervalFrame(3);

            RunFrame(App.Instance, 9);

            Assert.AreEqual(3, statu);
        }

        /// <summary>
        /// 间隔执行测试(边界)
        /// </summary>
        [TestMethod]
        public void TestIntervalFrameBound()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            timer.Make(() =>
            {
                statu++;
            }).IntervalFrame(3);

            RunFrame(App.Instance, 8);

            Assert.AreEqual(2, statu);
        }

        [TestMethod]
        public void TestIntervalFrameLife()
        {
            var timer = App.Instance.Make<ITimerManager>();
            var statu = 0;

            timer.Make(() =>
                {
                    statu++;
                })
                .IntervalFrame(3, 2);

            RunFrame(App.Instance, 30);

            Assert.AreEqual(2, statu);
        }

        [TestMethod]
        public void TestIntervalFrameInvalid()
        {
            var timer = App.Instance.Make<ITimerManager>();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                timer.Make().IntervalFrame(3);
            });
        }

        [TestMethod]
        public void TestSetTime()
        {
            var timerManager = App.Instance.Make<ITimerManager>();
            var statu = 0;

            var timer = timerManager.Make(() =>
            {
                statu++;
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                timer.Queue.SetTime(App.Instance.Make<ITimeManager>().Default);
            });
        }

        /// <summary>
        /// 运行的时长(已经忽略当前帧)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="time"></param>
        private void RunTime(IApplication app, float time)
        {
            var application = app as CatLib.Core.Application;
            var num = Math.Ceiling(time / 0.25f);

            for (int i = 0; i < num; i++)
            {
                NextFrame();
                application.Update();
            }
        }

        /// <summary>
        /// 运行的帧数(已经忽略当前帧)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="frame"></param>
        private void RunFrame(IApplication app, int frame)
        {
            var application = app as CatLib.Core.Application;
            for (var i = 0; i < frame; i++)
            {
                NextFrame();
                application.Update();
            }
        }

        /// <summary>
        /// 模拟到下一帧
        /// </summary>
        private void NextFrame()
        {
            var time = App.Instance.Make<ITimeManager>();
            (time.Default as TimerTestTime).FrameCount += 1;
        }
    }
}
