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

                router.Group("throw-error-group").OnError((req, res, ex, next) =>
                {
                    //这个组拦截了异常冒泡
                }).Middleware((req, res, next) =>
                {
                    next(req, res);
                    res.SetContext(res.GetContext() + "[with throw error group middleware]");
                });
            });

            app.On(RouterEvents.OnDispatcher, (sender, args) =>
            {
                var arg = args as DispatchEventArgs;
                Assert.AreNotEqual(null, arg.Request);
                Assert.AreNotEqual(null, arg.Route);
                Assert.AreNotEqual(string.Empty, (arg.Route as Route).Compiled.ToString());
            });

            app.Init();

            router = App.Instance.Make<IRouter>();
            router.OnError((req, res, ex, next) =>
            {
                Assert.Fail(ex.Message);
            });

            router.OnNotFound((req, next) =>
            {
                throw new NotFoundRouteException("not found route!");
            });

            router.Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[global middleware]");
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

            Assert.AreEqual("AttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());
        }

        [TestMethod]
        public void SimpleCallMTest()
        {
            var router = App.Instance.Make<IRouter>();
            var response = router.Dispatch("catlib://attr-routing-simple/call-mtest");

            Assert.AreEqual("AttrRoutingSimple.CallMTest[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 多重路由名测试
        /// </summary>
        [TestMethod]
        public void MultAttrRoutingTest()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("mult-attr-routing-simple/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://mult-attr-routing-simple/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://hello-world/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("cat://mult-attr-routing-simple/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://mult-attr-routing-simple/my-hello");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://hello-world/my-hello");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("cat://mult-attr-routing-simple/my-hello");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("dog://myname/call");
            Assert.AreEqual("MultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 带有中间件的路由请求
        /// </summary>
        [TestMethod]
        public void RoutingMiddlewareTest()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("rm://call");
            Assert.AreEqual("RoutingMiddleware.Call[with middleware][global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 参数路由测试
        /// </summary>
        [TestMethod]
        public void ParamsAttrTest()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("catlib://params-attr-routing/params-call/18");
            Assert.AreEqual("ParamsAttrRouting.ParamsCall.18.hello.catlib[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 使用路由组的参数路由
        /// </summary>
        [TestMethod]
        public void ParamsAttrWithGroup()
        {
            var router = App.Instance.Make<IRouter>();

            var response = router.Dispatch("catlib://params-attr-routing/params-call-with-group/18");
            Assert.AreEqual("ParamsAttrRouting.ParamsCall.18.hello.group-str[with group middleware][global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 直接回调测试
        /// </summary>
        [TestMethod]
        public void LambdaCall()
        {
            var router = App.Instance.Make<IRouter>();

            router.Reg("lambda://call/lambda-call", (req, res) =>
            {
                res.SetContext("RouterTests.LambdaCall");
            });

            var response = router.Dispatch("lambda://call/lambda-call");
            Assert.AreEqual("RouterTests.LambdaCall[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 引发异常的回调测试
        /// </summary>
        [TestMethod]
        public void ThrowErrorLambdaCall()
        {
            var router = App.Instance.Make<IRouter>();

            router.Reg("lambda://call/throw-error-lambda-call", (req, res) =>
            {
                res.SetContext("RouterTests.ThrowErrorLambdaCall");
                throw new Exception("unit test exception");
            }).Group("throw-error-group");

            var response = router.Dispatch("lambda://call/throw-error-lambda-call");
            Assert.AreEqual(null, response);
        }

        /// <summary>
        /// 无法找到路由的引发异常测试
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotFoundRouteException))]
        public void ThrowNotFoundExceptionCall()
        {
            var router = App.Instance.Make<IRouter>();
            var response = router.Dispatch("notfound://call");
        }

        /// <summary>
        /// 无法找到路由的引发异常测试(由scheme中引发)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotFoundRouteException))]
        public void ThrowNotFoundExceptionCallInScheme()
        {
            var router = App.Instance.Make<IRouter>();
            router.Reg("lambda://call/throw-not-found-exception", (req, res) =>
            {
                res.SetContext("RouterTests.ThrowNotFoundExceptionCallInScheme");
                throw new Exception("unit test exception");
            }).Group("throw-error-group");
            var response = router.Dispatch("lambda://call/throw-not-found-exception-abc");
        }

        /// <summary>
        /// 路由层的中间件
        /// </summary>
        [TestMethod]
        public void RouteMiddleware()
        {
            var router = App.Instance.Make<IRouter>();
            router.Reg("lambda://call/route-middleware", (req, res) =>
            {
                res.SetContext("RouterTests.RouteMiddleware");
            }).Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[route middleware]");
            });

            var response = router.Dispatch("lambda://call/route-middleware");
            Assert.AreEqual("RouterTests.RouteMiddleware[route middleware][global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 路由层异常
        /// </summary>
        [TestMethod]
        public void RouteException()
        {
            var router = App.Instance.Make<IRouter>();
            bool isException = false;
            router.Reg("lambda://call/route-exception", (req, res) =>
            {
                res.SetContext("RouterTests.RouteException");
                throw new Exception("unit test exception");
            }).Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[route middleware]");
            }).OnError((req, res, ex, next) =>
            {
                isException = true;
            });

            var response = router.Dispatch("lambda://call/route-exception");
            Assert.AreEqual(null, response);
            Assert.AreEqual(true, isException);
        }

        /// <summary>
        /// 大量的路由在一个组中测试
        /// </summary>
        [TestMethod]
        public void MoreRouteWithGroupTest()
        {
            var router = App.Instance.Make<IRouter>();

            bool isError = false;
            router.Group(() =>
            {
                router.Reg("lambda://call/more-1/{age}/{default?}", (req, res) =>
                {
                    res.SetContext("RouterTests.MoreRouteWithGroupTest-1." + req["age"] + "." + req["default"]);
                });
                router.Reg("lambda://call/more-2/{age}/{default?}", (req, res) =>
                {
                    res.SetContext("RouterTests.MoreRouteWithGroupTest-2." + req["age"] + "." + req["default"]);
                });

                router.Reg("lambda://call/more-error/{age}/{default?}", (req, res) =>
                {
                    res.SetContext("RouterTests.MoreRouteWithGroupTest-3." + req["age"] + "." + req["default"]);
                    throw  new Exception("unit test error!");
                });
            }).Defaults("default", "helloworld").Where("age", "[0-9]+").Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[local group middleware]");
            }).OnError((req, res, ex, next) =>
            {
                isError = true;
            });

            var response = router.Dispatch("lambda://call/more-1/18");
            Assert.AreEqual("RouterTests.MoreRouteWithGroupTest-1.18.helloworld[local group middleware][global middleware]", response.GetContext().ToString());

            response = router.Dispatch("lambda://call/more-2/6/catlib");
            Assert.AreEqual("RouterTests.MoreRouteWithGroupTest-2.6.catlib[local group middleware][global middleware]", response.GetContext().ToString());

            Assert.AreEqual(false ,isError);
            response = router.Dispatch("lambda://call/more-error/6/catlib");
            Assert.AreEqual(null, response);
            Assert.AreEqual(true, isError);
        }

        /// <summary>
        /// uri参数绑定
        /// </summary>
        [TestMethod]
        public void QueryParamsBind()
        {
            var router = App.Instance.Make<IRouter>();
            router.Reg("lambda://call/query-params-bind/{age}/{default?}", (req, res) =>
            {
                res.SetContext("RouterTests.QueryParamsBind." + req["age"] + "." + req["default"] + req["default2"]);
            });

            var response = router.Dispatch("lambda://call/query-params-bind/16?default=hello&default2=world");
            Assert.AreEqual("RouterTests.QueryParamsBind.16.helloworld[global middleware]", response.GetContext().ToString());
        }
    }
}
