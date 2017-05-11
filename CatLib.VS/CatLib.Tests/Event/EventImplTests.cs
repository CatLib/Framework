using System;
using CatLib.API;
using CatLib.Event;
#if UNITY_EDITOR
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Event
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
            var eventImpl = app.Make<EventImpl>();

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
            var eventImpl = app.Make<EventImpl>();

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
            var eventImpl = app.Make<EventImpl>();
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
        /// 根据名称触发事件
        /// </summary>
        [TestMethod]
        public void TriggerEventWithName()
        {
            
        }

        /// <summary>
        /// 测试Off
        /// </summary>
        [TestMethod]
        public void EventOff()
        {
            var app = MakeApplication();
            var eventImpl = app.Make<EventImpl>();
            var isCall = false;
            var handler = eventImpl.One("TestOff", (sender, e) =>
            {
                isCall = !isCall;
            });

            eventImpl.Off("TestOff", handler);
            eventImpl.Trigger("TestOff");
            Assert.AreEqual(false, isCall);
        }

        private Application MakeApplication()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap(typeof(BootstrapClass)).Init();
            return app;
        }

        private class BootstrapClass : IBootstrap
        {
            public void Bootstrap()
            {
                App.Instance.Register(typeof(EventProvider));
            }
        }
    }
}
