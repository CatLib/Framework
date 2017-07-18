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

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Core
{
    [TestClass]
    public class EventImplTests
    {
        /// <summary>
        /// 测试On
        /// </summary>
        [TestMethod]
        public void EventOn()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();

            Assert.AreNotEqual(null , eventImpl);

            var isCall = false;
            eventImpl.On("TestOn" ,(sender , e)=>
            {
                isCall = true;
            });

            eventImpl.Trigger("TestOn");
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 测试On，并给予生命次数
        /// </summary>
        [TestMethod]
        public void EventOnLife()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();

            Assert.AreNotEqual(null, eventImpl);

            var isCall = false;
            eventImpl.On("TestOn", (sender, e) =>
            {
                isCall = !isCall;
            }, 2);

            eventImpl.Trigger("TestOn");
            eventImpl.Trigger("TestOn");
            eventImpl.Trigger("TestOn");
            Assert.AreEqual(false, isCall);
        }

        /// <summary>
        /// 测试One
        /// </summary>
        [TestMethod]
        public void EventOne()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            eventImpl.One("TestOne", (sender, e) =>
            {
                isCall = !isCall;
            });
            eventImpl.Trigger("TestOne");
            eventImpl.Trigger("TestOne");
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 根据名称发送者触发事件
        /// </summary>
        [TestMethod]
        public void TriggerEventWithSender()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            var sender = new object();
            eventImpl.One("TestOne", (s, e) =>
            {
                if (s == sender)
                {
                    isCall = !isCall;
                }
            });

            eventImpl.Trigger("TestOne", sender);
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 根据名字发送者参数触发事件
        /// </summary>
        [TestMethod]
        public void TriggerEventWithSenderArgs()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            var sender = new object();
            var args = new EventArgs();
            eventImpl.One("TestOne", (s, e) =>
            {
                if (s == sender && e == args)
                {
                    isCall = !isCall;
                }
            });

            eventImpl.Trigger("TestOne", sender , args);
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 只根据名字和参数触发事件
        /// </summary>
        [TestMethod]
        public void TriggerEventWithArgs()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            var args = new EventArgs();
            eventImpl.One("TriggerEventWithArgs", (s, e) =>
            {
                if (s == null && e == args)
                {
                    isCall = !isCall;
                }
            });

            eventImpl.Trigger("TriggerEventWithArgs", args);
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 解除不存在的事件测试
        /// </summary>
        [TestMethod]
        public void OffNotExistsEvent()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();

            var handler = eventImpl.One("OffNotExistsEvent", (s, e) =>
            {
            });

            //这里为了测试需要强制转换
            eventImpl.Off(handler as EventHandler);
            eventImpl.Off(handler as EventHandler);
        }

        /// <summary>
        /// 测试Off
        /// </summary>
        [TestMethod]
        public void EventOff()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            var handler = eventImpl.One("TestOff", (sender, e) =>
            {
                isCall = !isCall;
            });

            handler.Cancel();
            eventImpl.Trigger("TestOff");
            Assert.AreEqual(false, isCall);
        }

        /// <summary>
        /// 重复的撤销
        /// </summary>
        [TestMethod]
        public void RepeatCancel()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            var handler = eventImpl.One("RepeatCancel", (sender, e) =>
            {
                isCall = !isCall;
            });

            Assert.AreEqual(true, handler.Cancel());
            Assert.AreEqual(false, handler.Cancel());
            eventImpl.Trigger("RepeatCancel");
            Assert.AreEqual(false , isCall);
        }

        /// <summary>
        /// 非法的触发
        /// </summary>
        [TestMethod]
        public void IllegalTrigger()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            eventImpl.One("IllegalTrigger", (sender, e) =>
            {
                isCall = !isCall;
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                eventImpl.Trigger(null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                eventImpl.Trigger("");
            });
        }

        /// <summary>
        /// 无效的On
        /// </summary>
        [TestMethod]
        public void IllegalOn()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<Event>();
            var isCall = false;
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                eventImpl.On("", (sender, e) =>
                {
                    isCall = !isCall;
                });
            });
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                eventImpl.On(null, (sender, e) =>
                {
                    isCall = !isCall;
                });
            });
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                eventImpl.On("IllegalOn", null);
            });

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                eventImpl.On("IllegalOn", (sender, e) =>
                {
                    isCall = !isCall;
                },-10);
            });
        }

        /// <summary>
        /// 无效的事件句柄测试
        /// </summary>
        [TestMethod]
        public void IllegalEventHandlerTest()
        {
            var e = new EventHandler(null, "test", null, 1);
            e.Call(null, null);

            Assert.AreEqual(false, e.IsLife);

            bool isCall = false;
            e = new EventHandler(null, "test", (s, arg) =>
            {
                isCall = !isCall;
            }, 1);
            e.Call(null, null);
            e.Call(null, null);

            Assert.AreEqual(true, isCall);
            Assert.AreEqual(false, e.IsLife);
            e.Call(null, null);
            e.Call(null, null);
            e.Call(null, null);
            Assert.AreEqual(0 , e.Life);
        }

        private Application MakeApplication()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap().Init();
            return app;
        }
    }
}
