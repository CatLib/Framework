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
