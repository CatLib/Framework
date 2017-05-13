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
using CatLib.API.Routing;
using CatLib.Event;
using CatLib.FilterChain;
using CatLib.Routing;
#if UNITY_EDITOR
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Routing
{
    [TestClass]
    public class RouterTests
    {

        [TestInitialize]
        public void TestInitialize()
        {
            var app = new Application().Bootstrap();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Register(typeof(EventProvider));
            app.Register(typeof(FilterChainProvider));
            app.Register(typeof(RoutingProvider));
            app.Init();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            App.Instance = null;
        }

        [TestMethod]
        public void SimpleCall()
        {
            var router = App.Instance.Make<IRouter>();
            var response = router.Dispatch("catlib://attr-routing-simple/call");

            Assert.AreEqual("AttrRoutingSimple.Call", response.GetContext().ToString());
        }
    }
}
