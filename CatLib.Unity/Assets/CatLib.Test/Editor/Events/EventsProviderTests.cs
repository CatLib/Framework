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

using CatLib.API.Events;
using CatLib.Events;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Events
{
    [TestClass]
    public class EventsProviderTests
    {
        /// <summary>
        /// 生成测试环境
        /// </summary>
        /// <returns></returns>
        private IContainer MakeEnv()
        {
            var app = new Application();

            app.Bootstrap();
            app.Register(new EventsProvider());
            app.Init();

            return app;
        }

        [TestMethod]
        public void TestSimpleOnEvents()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
            });

            Assert.AreEqual(null , (dispatcher.Trigger("event.name", 123) as object[])[0]);
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestTriggerReturnResult()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.On("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });

            var result = dispatcher.Trigger("event.name", 123) as object[];

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestAsteriskWildcard()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.On("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.On("event.age", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.On("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            });

            var result = dispatcher.Trigger("event.name", 123) as object[];
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(true, isCall);

            result = dispatcher.Trigger("event.age", 123) as object[];
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(3, result[1]);
        }

        [TestMethod]
        public void TestHalfTrigger()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.On("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            });

            Assert.AreEqual(1, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestCancelHandler()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            var handler = dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.On("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.On("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            });

            handler.Cancel();

            Assert.AreEqual(2, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(false, isCall);
        }

        [TestMethod]
        public void TestLifeCall()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            }, 1);

            Assert.AreEqual(1, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(null, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestDepthSelfTrigger()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var callNum = 0;
            dispatcher.On("event.name", (payload) =>
            {
                dispatcher.Trigger("event.name", payload);
                callNum++;
                Assert.AreEqual(123, payload);
                return 1;
            }, 3);

            Assert.AreEqual(1, (dispatcher.Trigger("event.name", 123) as object[]).Length);
            Assert.AreEqual(3, callNum);

            Assert.AreEqual(null, dispatcher.Trigger("event.name", 123, true));
        }

        [TestMethod]
        public void TestOrderCancel()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var handler = dispatcher.On("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            },1);
            dispatcher.On("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            },1);
            dispatcher.On("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            },1);

            Assert.AreEqual(1, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(2, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(3, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(null, dispatcher.Trigger("event.name", 123, true));
        }

        [TestMethod]
        public void TestRepeatOn()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            dispatcher.On("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            }, 1);

            dispatcher.On("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            }, 1);

            Assert.AreEqual(1, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(2, dispatcher.Trigger("event.name", 123, true));
            Assert.AreEqual(null, dispatcher.Trigger("event.name", 123, true));
        }

        [TestMethod]
        public void TestOtherWildcardOn()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            dispatcher.On("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            }, 1);

            dispatcher.On("event.call.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            }, 1);

            dispatcher.On("event2.call.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            }, 1);

            Assert.AreEqual(1, dispatcher.Trigger("event.call.name", 123, true));
            Assert.AreEqual(2, dispatcher.Trigger("event.call.name", 123, true));
            Assert.AreEqual(null, dispatcher.Trigger("event.call.name", 123, true));
        }
    }
}
