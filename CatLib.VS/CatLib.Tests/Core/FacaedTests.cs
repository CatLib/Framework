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
    public class FacaedTests
    {

        public class TestClass
        {
            
        }

        public class TestClassFacaed : Facade<TestClass>
        {
            
        }

        /// <summary>
        /// 门面测试
        /// </summary>
        [TestMethod]
        public void FacadeTest()
        {
            var app = new Application();
            app.Bootstrap();
            var obj = new TestClass();
            app.Singleton<TestClass>((c, p) =>
            {
                return obj;
            });

            Assert.AreEqual(obj, TestClassFacaed.Instance);
            //double run
            Assert.AreEqual(obj, TestClassFacaed.Instance);
        }

        /// <summary>
        /// 无Application支持下测试
        /// </summary>
        [TestMethod]
        public void NullApplicationFacadeTest()
        {
            App.Instance = null;

            ExceptionAssert.Throws<NullReferenceException>(() =>
            {
                var f = TestClassFacaed.Instance;
            });
        }
    }
}
