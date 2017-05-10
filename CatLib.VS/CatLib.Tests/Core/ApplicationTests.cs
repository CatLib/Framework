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


namespace CatLib.Tests.Core
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public void MakeApplicationTest()
        {
            var app = MakeApplication();
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
