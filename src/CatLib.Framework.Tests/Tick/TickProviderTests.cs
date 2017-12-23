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
using System.Threading;
using CatLib.Tick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Tick
{
    [TestClass]
    public class TickProviderTests
    {
        /// <summary>
        /// 构建环境
        /// </summary>
        /// <returns></returns>
        private IApplication MakeEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new TickProvider());
            app.Init();
            return app;
        }

        public class TickTestClass : ITick
        {
            private bool isTick = false;

            /// <summary>
            /// 是否是Tick
            /// </summary>
            public bool IsTick
            {
                get { return isTick; }
            }

            /// <summary>
            /// 滴答间流逝的时间
            /// </summary>
            /// <param name="elapseMillisecond">滴答间流逝的时间(MS)</param>
            public void Tick(int elapseMillisecond)
            {
                isTick = true;
            }
        }

        [TestMethod]
        public void SimpleTest()
        {
            var app = MakeEnv();
            app.Singleton<TickTestClass>();
            var cls = app.Make<TickTestClass>();

            Thread.Sleep(1000);

            Assert.AreEqual(true, cls.IsTick);
        }

        [TestMethod]
        public void ReleaseClassTest()
        {
            var app = MakeEnv();

            ExceptionAssert.DoesNotThrow(() =>
            {
                app.Singleton<TickTestClass>();
                app.Make<TickTestClass>();
                app.Release<TickTestClass>();
            });
        }

        [TestMethod]
        public void ReleaseTickerTest()
        {
            var app = MakeEnv();
            app.Release<TimeTicker>();

            app.Singleton<TickTestClass>();
            var cls = app.Make<TickTestClass>();

            Thread.Sleep(1000);
            Assert.AreEqual(false, cls.IsTick);
        }

        [TestMethod]
        public void GetFpsTest()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new TickProvider
            {
                Fps = 65
            });
            app.Init();

            Assert.AreEqual(65, app.Make<TimeTicker>().Fps);
        }
    }
}
