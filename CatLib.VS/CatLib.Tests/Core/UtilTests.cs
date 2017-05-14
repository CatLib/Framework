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
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Core
{
    [TestClass]
    public class UtilTests
    {

        public interface ITest
        {
        }

        public class UtilTestClass : ITest
        {
            
        }

        /// <summary>
        /// 测试根据接口搜索类型
        /// </summary>
        [TestMethod]
        public void FindTypesWithInterfaceTest()
        {
            var lst = Util.FindTypesWithInterface(typeof(ITest));

            Assert.AreEqual(1 , lst.Length);
            Assert.AreEqual(typeof(UtilTestClass) , lst[0]);
        }

        /// <summary>
        /// 标准化路径测试
        /// </summary>
        [TestMethod]
        public void StandardPathTest()
        {
            var path = "hello\\world";
            Assert.AreEqual("hello/world", Util.StandardPath(path));
        }
    }
}
