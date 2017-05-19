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
    public class SingleStringConverterTests
    {
        /// <summary>
        /// 转换为字符串测试
        /// </summary>
        [TestMethod]
        public void SingleStringConvertToStringTest()
        {
            var convert = new SingleStringConverter();
            Assert.AreEqual("11.1121", convert.ConvertToString(11.1121f));
        }

        /// <summary>
        /// 从字符串转为对象测试
        /// </summary>
        [TestMethod]
        public void SingleStringConvertFromStringTest()
        {
            var convert = new SingleStringConverter();
            Assert.AreEqual(11.1121f, convert.ConvertFromString("11.1121", typeof(Single)));
        }
    }
}
