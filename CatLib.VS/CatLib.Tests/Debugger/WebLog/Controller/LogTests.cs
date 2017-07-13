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
using System.Threading;
using CatLib.API.Debugger;
using CatLib.API.Json;
using CatLib.Debugger.WebConsole;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Debugger.WebLog.Controller
{
    [TestClass]
    public class LogTests
    {
        [TestMethod]
        public void TestGetCatergroy()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var logCatergroy = app.Make<ILogWebCategory>();
            logCatergroy.DefinedCategory("CatLib.Tests.Debugger", "Debugger");
            logCatergroy.DefinedCategory("CatLib.Tests.Routing", "Router");

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/log/get-catergroy", out ret);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":{\"CatLib.Tests.Debugger\":\"Debugger\",\"CatLib.Tests.Routing\":\"Router\"}}", ret);
        }

        [TestMethod]
        public void TestGetLog()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            var logger = app.Make<ILogger>();

            logger.Debug("hello world");
            logger.Warning("my name is {0}" , "catlib");

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/log/get-log/1", out ret);
            logger.Warning("my name is {0}", "catlib2");
            string ret2;
            var statu2 = HttpHelper.Get("http://localhost:9478/debug/log/get-log/1", out ret2);
            console.Stop();

            var json = app.Make<IJson>();
            var retJson = json.Decode(ret)["Response"] as IList<object>;
            var ret2Json = json.Decode(ret2)["Response"] as IList<object>;

            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual((long)1, (retJson[0] as IDictionary<string,object>)["id"]);
            Assert.AreEqual((long)2, (retJson[1] as IDictionary<string, object>)["id"]);
            Assert.AreEqual(HttpStatusCode.OK, statu2);
            Assert.AreEqual((long)3, (ret2Json[0] as IDictionary<string, object>)["id"]);
        }

        //[TestMethod]
        public void StartServer()
        {
            var app = DebuggerHelper.GetApplication();
            var logger = app.Make<ILogger>();
            logger.Debug("hello world");

            var i = 0;
            while (true)
            {
                logger.Debug("hello world" + i);
                i++;
                Thread.Sleep(1000);
            }
        }
    }
}
