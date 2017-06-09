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

using CatLib.Config;
using CatLib.Config.Converters;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Config
{
    [TestClass]
    public class EnumStringConverterTests
    {

        private enum TestEnums
        {
            Hello = 1,
            World = 2,
        }

        /// <summary>
        /// 转换为字符串测试
        /// </summary>
        [TestMethod]
        public void EnumStringConvertToStringTest()
        {
            var convert = new EnumStringConverter();
            Assert.AreEqual("World", convert.ConvertToString(TestEnums.World));
        }

        /// <summary>
        /// 从字符串转为对象测试
        /// </summary>
        [TestMethod]
        public void EnumStringConvertFromStringTest()
        {
            var convert = new EnumStringConverter();
            Assert.AreEqual(TestEnums.Hello, convert.ConvertFromString("TestEnums.Hello", typeof(TestEnums)));
            Assert.AreEqual(TestEnums.World, convert.ConvertFromString("World", typeof(TestEnums)));
            Assert.AreEqual(TestEnums.Hello, convert.ConvertFromString("1", typeof(TestEnums)));
        }
    }
}
