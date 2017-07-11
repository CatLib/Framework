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
using System.Collections.Generic;
using System.Net;
using CatLib.API.Config;
using CatLib.API.Debugger;
using CatLib.API.Routing;
using CatLib.Config;
using CatLib.Core;
using CatLib.Debugger;
using CatLib.Debugger.WebConsole;
using CatLib.Debugger.WebMonitor;
using CatLib.Debugger.WebMonitor.Handler;
using CatLib.Json;
using CatLib.Routing;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Debugger.WebConsole
{
    [Routed]
    [TestClass]
    public class HttpDebuggerConsoleTests
    {
        private Application GetApplication()
        {
            var app = new Application();
            app.OnFindType((str) => Type.GetType(str));
            app.Bootstrap();
            app.Register(new RoutingProvider());
            app.Register(new JsonProvider());
            app.Register(new DebuggerProvider());
            app.Register(new ConfigProvider());
            app.Make<IConfigManager>().Default.Set("debugger.logger.handler.unity", false);
            app.Init();
            return app;
        }

        private class SimpleResponse : IWebConsoleResponse
        {
            /// <summary>
            /// 响应
            /// </summary>
            public object Response { get; private set; }

            public SimpleResponse(object data)
            {
                Response = data;
            }
        }

        [Routed]
        public void SimpleCall(IResponse response)
        {
            response.SetContext(new SimpleResponse(new Dictionary<string, string> {{"hello", "world"}}));
        }

        [TestMethod]
        public void TestDebuggerConsole()
        {
            var app = GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/catlib/http-debugger-console-tests/simple-call", out ret);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":{\"hello\":\"world\"}}", ret);
        }

        [TestMethod]
        public void TestGetCatergroy()
        {
            var app = GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var logCatergroy = app.Make<ILogWebCategory>();
            logCatergroy.DefinedCategory("CatLib.Tests.Debugger" , "Debugger");
            logCatergroy.DefinedCategory("CatLib.Tests.Routing", "Router");

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/log/get-catergroy", out ret);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":{\"CatLib.Tests.Debugger\":\"Debugger\",\"CatLib.Tests.Routing\":\"Router\"}}", ret);
        }

        [TestMethod]
        public void TestGetMonitor()
        {
            var app = GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var monitor = app.Make<IMonitor>();
            var handler = new OnceRecordMonitorHandler("title", "ms");
            monitor.DefinedMoitor("test" , handler , -1);
            monitor.Monitor("test", 100);

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/monitor/get-monitors/1", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":[{\"name\":\"title\",\"value\":\"100\",\"unit\":\"ms\"}]}", ret);
        }

        [TestMethod]
        public void TestEcho()
        {
            var app = GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/util/echo/helloworld", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual(string.Empty, ret);
        }
    }
}
