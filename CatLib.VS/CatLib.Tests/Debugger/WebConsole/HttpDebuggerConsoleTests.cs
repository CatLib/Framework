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

using System.Collections.Generic;
using System.Net;
using CatLib.API.Routing;
using CatLib.Debugger.WebConsole;
using CatLib.Debugger.WebMonitor;
using CatLib.Debugger.WebMonitor.Handler;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Debugger.WebConsole
{
    [Routed]
    [TestClass]
    public class HttpDebuggerConsoleTests
    {
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
            response.SetContext(new SimpleResponse(new Dictionary<string, string> { { "hello", "world" } }));
        }

        [TestMethod]
        public void TestDebuggerConsole()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/catlib/http-debugger-console-tests/simple-call", out ret);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":{\"hello\":\"world\"}}", ret);
        }
    }
}
