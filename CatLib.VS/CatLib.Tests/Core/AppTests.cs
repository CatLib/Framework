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
    public class AppTests
    {
        /// <summary>
        /// 空的App实例测试
        /// </summary>
        [TestMethod]
        public void NullAppInstanceTest()
        {
            App.Instance = null;
            ExceptionAssert.Throws<NullReferenceException>(() =>
            {
                var inst = App.Instance;
            });
        }
    }
}
