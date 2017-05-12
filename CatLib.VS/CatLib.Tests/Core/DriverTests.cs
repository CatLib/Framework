using System;

#if UNITY_EDITOR
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif


namespace CatLib.Tests.Core
{
    [TestClass]
    public class DriverTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        public Driver MakeDriver()
        {
            var driver = new Driver();
            return driver;
        }
    }
}
