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
    public class ContainerHelperTests
    {
        /// <summary>
        /// 生成服务和转为目标
        /// </summary>
        [TestMethod]
        public void MakeTConvert()
        {
            var container = MakeContainer();
            var obj = container.Make<ContainerHelperTests>("ContainerHelperTests");
            Assert.AreSame(this, obj);
        }

        /// <summary>
        /// 生成服务和转为目标
        /// </summary>
        [TestMethod]
        public void MakeTService()
        {
            var container = MakeContainer();
            var obj = container.Make<ContainerHelperTests>();
            Assert.AreSame(this, obj);
        }

        /// <summary>
        /// 以单例形式绑定
        /// </summary>
        [TestMethod]
        public void BindSingleton()
        {
            var container = MakeContainer();
            container.Singleton("BindSingleton", (c, param) =>
            {
                return new object();
            });
            var obj = container.Make("BindSingleton");
            Assert.AreSame(obj, container.Make("BindSingleton"));
        }

        public class ContainerHelperTestClass
        {
            
        }

        public class TestClassService
        {
            
        }

        /// <summary>
        /// 以单列形式绑定
        /// </summary>
        [TestMethod]
        public void BindSingletonTServiceTConcrete()
        {
            var container = MakeContainer();
            container.Singleton<TestClassService, ContainerHelperTestClass>();
            var obj = container.Make(typeof(TestClassService).ToString());
            var obj2 = container.Make(typeof(TestClassService).ToString());

            Assert.AreSame(obj, obj2);
        }

        /// <summary>
        /// 以单列形式绑定
        /// </summary>
        [TestMethod]
        public void SingletonTService()
        {
            var container = MakeContainer();
            container.Singleton<TestClassService>((c, p) =>
            {
                return new object();
            });
            var obj = container.Make(typeof(TestClassService).ToString());
            var obj2 = container.Make(typeof(TestClassService).ToString());

            Assert.AreSame(obj, obj2);
        }

        /// <summary>
        /// 生成容器
        /// </summary>
        /// <returns>容器</returns>
        private CatLib.Container.Container MakeContainer()
        {
            var container = new CatLib.Container.Container();
            container.Instance("ContainerHelperTests", this);
            container.Instance(typeof(ContainerHelperTests).ToString(), this);
            return container;
        }
    }
}
