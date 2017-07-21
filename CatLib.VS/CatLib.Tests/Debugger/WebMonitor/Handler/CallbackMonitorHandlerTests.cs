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

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using System.Collections.Generic;
using CatLib.Debugger.WebMonitor;
using CatLib.Debugger.WebMonitor.Handler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Debugger.WebMonitor.Handler
{
    [TestClass]
    public class CallbackMonitorHandlerTests
    {
        /// <summary>
        /// 监控处理器
        /// </summary>
        public class TempHandler : IMonitorHandler
        {
            /// <summary>
            /// 监控的名字(用于UI端显示)
            /// </summary>
            public string Title
            {
                get { return "TempHandlerTitle"; }
            }

            /// <summary>
            /// 监控值的单位描述
            /// </summary>
            public string Unit
            {
                get { return "TempHandlerUtil"; }
            }

            private string value = "TempHandlerValue";

            /// <summary>
            /// 实时的监控值
            /// </summary>
            public string Value
            {
                get { return value; }
            }

            /// <summary>
            /// 处理句柄
            /// </summary>
            /// <param name="value">值</param>
            public void Handler(object value)
            {
                this.value = value.ToString();
            }
        }

        [TestMethod]
        public void SimpleCallbackTest()
        {
            var handler = new CallbackMonitorHandler(new TempHandler(), () =>
            {
                return "helloworld";
            });
            handler.Handler(null);
            Assert.AreEqual("TempHandlerTitle", handler.Title);
            Assert.AreEqual("TempHandlerUtil", handler.Unit);
            Assert.AreEqual("helloworld", handler.Value);
        }
    }
}
