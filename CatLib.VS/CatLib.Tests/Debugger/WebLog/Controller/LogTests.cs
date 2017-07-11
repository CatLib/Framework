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

using System.Net;
using CatLib.API.Debugger;
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
            var statu = HttpHelper.Get("http://localhost:9478/debug/log/get-log", out ret);
            string ret2;
            var statu2 = HttpHelper.Get("http://localhost:9478/debug/log/get-log/1", out ret2);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":[{\"id\":1,\"level\":7,\"namespace\":\"CatLib.Tests.Debugger.WebLog.Controller\",\"message\":\"hello world\",\"callStack\":[\"Void TestGetLog()(at D:\\\\Work\\\\catlib\\\\CatLib.VS\\\\CatLib.Tests\\\\Debugger\\\\WebLog\\\\Controller\\\\LogTests.cs:53)\"]},{\"id\":2,\"level\":4,\"namespace\":\"CatLib.Tests.Debugger.WebLog.Controller\",\"message\":\"my name is catlib\",\"callStack\":[\"Void TestGetLog()(at D:\\\\Work\\\\catlib\\\\CatLib.VS\\\\CatLib.Tests\\\\Debugger\\\\WebLog\\\\Controller\\\\LogTests.cs:54)\"]}]}", ret);
            Assert.AreEqual(HttpStatusCode.OK, statu2);
            Assert.AreEqual("{\"Response\":[{\"id\":2,\"level\":4,\"namespace\":\"CatLib.Tests.Debugger.WebLog.Controller\",\"message\":\"my name is catlib\",\"callStack\":[\"Void TestGetLog()(at D:\\\\Work\\\\catlib\\\\CatLib.VS\\\\CatLib.Tests\\\\Debugger\\\\WebLog\\\\Controller\\\\LogTests.cs:54)\"]}]}", ret2);
        }
    }
}
