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

using CatLib.API.Debugger;
using CatLib.Debugger.Log;
using CatLib.Debugger.WebLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Debugger.WebLog
{
    [TestClass]
    public class LogStoreTests
    {
        [TestMethod]
        public void TestLogOverflow()
        {
            var store = new LogStore(new Logger());

            store.SetMaxStore(10);

            for (var i = 0; i < 20; i++)
            {
                store.Log(new LogEntry(LogLevels.Info, "hello", 0));
            }

            Assert.AreEqual(10, store.GetUnloadEntrysByClientId("test").Count);
        }
    }
}
