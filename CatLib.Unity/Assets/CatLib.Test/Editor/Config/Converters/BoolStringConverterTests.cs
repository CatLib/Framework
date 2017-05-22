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
    public class BoolStringConverterTests
    {
        /// <summary>
        /// 转换为字符串测试
        /// </summary>
        [TestMethod]
        public void BoolStringConvertToStringTest()
        {
            var coverter = new BoolStringConverter();
            Assert.AreEqual("True", coverter.ConvertToString(true));
            Assert.AreEqual("False", coverter.ConvertToString(false));
        }

        /// <summary>
        /// 从字符串转为对象测试
        /// </summary>
        [TestMethod]
        public void BoolStringConvertFromStringTest()
        {
            var coverter = new BoolStringConverter();
            Assert.AreEqual(true, coverter.ConvertFromString("True", typeof(bool)));
            Assert.AreEqual(true, coverter.ConvertFromString("true", typeof(bool)));
            Assert.AreEqual(true, coverter.ConvertFromString("1", typeof(bool)));
            Assert.AreEqual(true, coverter.ConvertFromString("yes", typeof(bool)));
            Assert.AreEqual(true, coverter.ConvertFromString("on", typeof(bool)));
            Assert.AreEqual(true, coverter.ConvertFromString("y", typeof(bool)));

            Assert.AreEqual(false, coverter.ConvertFromString("No", typeof(bool)));
            Assert.AreEqual(false, coverter.ConvertFromString("false", typeof(bool)));
            Assert.AreEqual(false, coverter.ConvertFromString("faLse", typeof(bool)));
            Assert.AreEqual(false, coverter.ConvertFromString("n", typeof(bool)));
            Assert.AreEqual(false, coverter.ConvertFromString("0", typeof(bool)));
            Assert.AreEqual(false, coverter.ConvertFromString("off", typeof(bool)));

            ExceptionAssert.Throws<ConverterException>(() =>
            {
                coverter.ConvertFromString("1111", typeof(bool));
            });
        }
    }
}
