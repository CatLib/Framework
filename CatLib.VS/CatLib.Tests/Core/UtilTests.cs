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
