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
    }
}
