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
    public class Int64StringConverterTests
    {
        /// <summary>
        /// 转换为字符串测试
        /// </summary>
        [TestMethod]
        public void Int64StringConvertToStringTest()
        {
            var convert = new Int64StringConverter();
            Int64 v = 1111111;
            Assert.AreEqual("1111111", convert.ConvertToString(v));
        }

        /// <summary>
        /// 从字符串转为对象测试
        /// </summary>
        [TestMethod]
        public void Int64StringConvertFromStringTest()
        {
            var convert = new Int64StringConverter();
            Int64 v = 1111111;
            Assert.AreEqual(v, convert.ConvertFromString("1111111", typeof(Int64)));
        }
    }
}
