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
    public class DecimalStringConverterTests
    {
        /// <summary>
        /// 转换为字符串测试
        /// </summary>
        [TestMethod]
        public void DecimalStringConvertToStringTest()
        {
            var convert = new DecimalStringConverter();
            Assert.AreEqual("300.57812938712893712361812324", convert.ConvertToString(300.57812938712893712361812324m));
        }

        /// <summary>
        /// 从字符串转为对象测试
        /// </summary>
        [TestMethod]
        public void DecimalStringConvertFromStringTest()
        {
            var convert = new DecimalStringConverter();
            Assert.AreEqual(300.57812938712893712361812324m, convert.ConvertFromString("300.57812938712893712361812324", typeof(decimal)));
        }
    }
}
