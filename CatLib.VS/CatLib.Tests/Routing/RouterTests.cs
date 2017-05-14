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
            
            //由于熟悉框架流程所以这么写，项目中使用请接受指定事件再生成路由服务
            var router = App.Instance.Make<IRouter>();
            app.On(RouterEvents.OnRouterAttrCompiler, (sender, args) =>
            {
                router.Group("default-group").Where("sex", "[0-1]").Defaults("str", "group-str").Middleware(
                    (req, res, next) =>
                    {
                        next(req, res);
                        res.SetContext(res.GetContext() + "[with group middleware]");
                    });
            });

            app.Init();

            router = App.Instance.Make<IRouter>();
            router.OnError((req, res, ex, next) =>
            {
                Assert.Fail(ex.Message);
            });

            router.OnNotFound((req, next) =>
            {
                Assert.Fail("not found route!");
            });
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

        [TestMethod]
        public void SimpleCallMTest()
        {
            var router = App.Instance.Make<IRouter>();
            var response = router.Dispatch("catlib://attr-routing-simple/call-mtest");

            Assert.AreEqual("AttrRoutingSimple.CallMTest", response.GetContext().ToString());
        }

        /// <summary>
        /// 多重路由名测试
        /// </summary>
        [TestMethod]
        public void MultAttrRoutingTest()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("mult-attr-routing-simple/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());

            response = router.Dispatch("catlib://mult-attr-routing-simple/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());

            response = router.Dispatch("catlib://hello-world/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());
            
            response = router.Dispatch("cat://mult-attr-routing-simple/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());

            response = router.Dispatch("catlib://mult-attr-routing-simple/my-hello");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());

            response = router.Dispatch("catlib://hello-world/my-hello");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());

            response = router.Dispatch("cat://mult-attr-routing-simple/my-hello");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());

            response = router.Dispatch("dog://myname/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call", response.GetContext().ToString());
        }

        /// <summary>
        /// 带有中间件的路由请求
        /// </summary>
        [TestMethod]
        public void RoutingMiddlewareTest()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("rm://call");
            Assert.AreEqual("RoutingMiddleware.Call[with middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 参数路由测试
        /// </summary>
        [TestMethod]
        public void ParamsAttrTest()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("catlib://params-attr-routing/params-call/18");
            Assert.AreEqual("ParamsAttrRouting.ParamsCall.18.hello.catlib", response.GetContext().ToString());
        }

        /// <summary>
        /// 使用路由组的参数路由
        /// </summary>
        [TestMethod]
        public void ParamsAttrWithGroup()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("catlib://params-attr-routing/params-call-with-group/18");
            Assert.AreEqual("ParamsAttrRouting.ParamsCall.18.hello.group-str[with group middleware]", response.GetContext().ToString());
        }
    }
}
