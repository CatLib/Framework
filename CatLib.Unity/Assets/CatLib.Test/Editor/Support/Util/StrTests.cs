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

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.API.Stl
{
    [TestClass]
    public class StrTests
    {
        [TestMethod]
        public void TestRegexQuote()
        {
            var input = "(.*?) from table";
            input = Str.RegexQuote(input);

            Assert.AreEqual(@"\(\.\*\?\) from table", input);
        }

        [TestMethod]
        public void TestAsteriskWildcard()
        {
            var input = "path.?+/hello/wor*d";
            input = Str.AsteriskWildcard(input);

            Assert.AreEqual(@"path\.\?\+/hello/wor.*?d", input);
        }

        [TestMethod]
        public void TestStrIs()
        {
            Console.WriteLine(Str.AsteriskWildcard("path.?+/hello/w*d"));
            Assert.AreEqual(true, Str.Is("path.?+/hello/w*d", @"path.?+/hello/world"));
            Assert.AreEqual(false, Str.Is("path.?+/hello/w*d", @"hellopath.?+/hello/world"));
            Assert.AreEqual(true, Str.Is("path.?+/hello/w*d", @"path.?+/hello/worlddddddddd"));
            Assert.AreEqual(false, Str.Is("path.?+/hello/w*d", @"path.?+/hello/worldddddddddppppp"));
        }
    }
}
