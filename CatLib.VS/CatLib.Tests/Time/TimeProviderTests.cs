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

using CatLib.API.Config;
using CatLib.API.Time;
using CatLib.Config;
using CatLib.Time;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Time
{
    [TestClass]
    public class TimeProviderTests
    {
        [TestMethod]
        public void TestInitialize()
        {
            var app = new Application().Bootstrap();
            app.Register(typeof(TimeProvider));
            app.Register(typeof(ConfigProvider));
            app.Init();

            var timeManager = app.Make<ITimeManager>();
            timeManager.Extend(() => null, "test");

            var config = app.Make<IConfigManager>();
            config.Default.Set("times.default", "test");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var timeManager = App.Instance.Make<ITimeManager>();
            
        }
    }
}
