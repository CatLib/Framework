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

namespace CatLib.Tests.Debugger.WebMonitor.Controller
{
    [TestClass]
    public class MonitorTests
    {
        [TestMethod]
        public void TestGetMonitor()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var monitor = app.Make<IMonitor>();
            var handler = new OnceRecordMonitorHandler("title", "ms");
            monitor.DefinedMoitor("test", handler, -1);
            monitor.Monitor("test", 100);

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/monitor/get-monitors/1", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":[{\"name\":\"title\",\"value\":\"100\",\"unit\":\"ms\"}]}", ret);
        }
    }
}
