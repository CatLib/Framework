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
    public class DoubleStringConverterTests
    {
        /// <summary>
        /// 转换为字符串测试
        /// </summary>
        [TestMethod]
        public void DoubleStringConvertToStringTest()
        {
            var convert = new DoubleStringConverter();
            Assert.AreEqual("1023.9123911", convert.ConvertToString(1023.9123911d));
        }

        /// <summary>
        /// 从字符串转为对象测试
        /// </summary>
        [TestMethod]
        public void DoubleStringConvertFromStringTest()
        {
            var convert = new DoubleStringConverter();
            Assert.AreEqual(1023.9123911d, convert.ConvertFromString("1023.9123911", typeof(double)));
        }
    }
}
