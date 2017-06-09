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
    public class UInt32StringConverterTests
    {
        /// <summary>
        /// 转换为字符串测试
        /// </summary>
        [TestMethod]
        public void UInt32StringConvertToStringTest()
        {
            var convert = new UInt32StringConverter();
            UInt32 v = 123;
            Assert.AreEqual("123", convert.ConvertToString(v));
        }

        /// <summary>
        /// 从字符串转为对象测试
        /// </summary>
        [TestMethod]
        public void Int16StringConvertFromStringTest()
        {
            var convert = new UInt32StringConverter();
            UInt32 v = 123;
            Assert.AreEqual(v, convert.ConvertFromString("123", typeof(UInt32)));
        }
    }
}
