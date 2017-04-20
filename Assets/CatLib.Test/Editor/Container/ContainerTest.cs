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
using NUnit.Framework;
using System.Collections.Generic;
using CatLib.API;

namespace CatLib.Test.Container
{
    /// <summary>
    /// 容器测试用例
    /// </summary>
    [TestFixture]
    public class ContainerTest
    {

        #region Tag
        /// <summary>
        /// 是否可以标记服务
        /// </summary>
        [Test]
        public void CanTagService()
        {
            var container = MakeContainer();
            Assert.DoesNotThrow(() =>
            {
                container.Tag("TestTag", "service1", "service2");
            });
        }

        /// <summary>
        /// 检测无效的Tag输入
        /// </summary>
        [Test]
        public void CheckIllegalTagInput()
        {
            var container = MakeContainer();
            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Tag("TestTag");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Tag("TestTag", null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Tag(null, "service1", "service2");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Tag(string.Empty, "service1", "service2");
            });
        }

        /// <summary>
        /// 是否可以根据标签生成服务
        /// </summary>
        [Test]
        public void CanMakeWithTaged()
        {
            var container = MakeContainer();
            container.Bind("TestService1", (app, param) => "hello");
            container.Bind("TestService2", (app, param) => "world");

            container.Tag("TestTag", "TestService1", "TestService2");

            Assert.DoesNotThrow(() =>
            {
                var obj = container.Tagged("TestTag");
                Assert.AreEqual(2, obj.Length);
                Assert.AreEqual("hello", obj[0]);
                Assert.AreEqual("world", obj[1]);
            });
        }

        #endregion

        #region Bind
        /// <summary>
        /// 是否能够进行如果不存在则绑定的操作
        /// </summary>
        [Test]
        public void CanBindIf()
        {
            var container = MakeContainer();
            var bind = container.BindIf("CanBindIf", (cont, param) => "Hello", true);
            var bind2 = container.BindIf("CanBindIf", (cont, param) => "World", false);

            Assert.AreSame(bind , bind2);
        }

        /// <summary>
        /// 检测无效的绑定
        /// </summary>
        [Test]
        public void CheckIllegalBind()
        {
            var container = MakeContainer();
            container.Bind("CheckIllegalBind", (cont, param) => "HelloWorld", true);

            Assert.Throws<RuntimeException>(() =>
            {
                container.Bind("CheckIllegalBind", (cont, param) => "Repeat Bind");
            });

            container.Instance("InstanceBind", "hello world");

            Assert.Throws<RuntimeException>(() =>
            {
                container.Bind("InstanceBind", (cont, param) => "Instance Repeat Bind");
            });

            container.Alias("Hello", "CheckIllegalBind");

            Assert.Throws<RuntimeException>(() =>
            {
                container.Bind("Hello", (cont, param) => "Alias Repeat Bind");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Bind(string.Empty, (cont, param) => "HelloWorld");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Bind(null, (cont, param) => "HelloWorld");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Bind("NoConcrete", null);
            });
        }

        /// <summary>
        /// 静态绑定方法
        /// </summary>
        [Test]
        public void CanBindFuncStatic()
        {
            var container = MakeContainer();
            container.Bind("CanBindFuncStatic", (cont, param) => "HelloWorld", true);

            var bind = container.GetBind("CanBindFuncStatic");
            var hasBind = container.HasBind("CanBindFuncStatic");
            var obj = container.Make("CanBindFuncStatic");

            Assert.AreNotEqual(null, bind);
            Assert.AreEqual(true, hasBind);
            Assert.AreEqual(true, bind.IsStatic);
            Assert.AreSame("HelloWorld", obj);
        }

        /// <summary>
        /// 非静态绑定
        /// </summary>
        [Test]
        public void CanBindFunc()
        {
            var container = MakeContainer();
            container.Bind("CanBindFunc", (cont, param) => new List<string>());

            var bind = container.Make("CanBindFunc");
            var bind2 = container.Make("CanBindFunc");

            Assert.AreNotEqual(null, bind);
            Assert.AreNotSame(bind, bind2);
        }

        /// <summary>
        /// 检测获取绑定
        /// </summary>
        [Test]
        public void CanGetBind()
        {
            var container = MakeContainer();
            var bind = container.Bind("CanGetBind", (cont, param) => "hello world");
            var getBind = container.GetBind("CanGetBind");
            Assert.AreSame(bind, getBind);

            var getBindNull = container.GetBind("CanGetBindNull");
            Assert.AreEqual(null, getBindNull);

            bind.Alias("AliasName");
            var aliasBind = container.GetBind("AliasName");
            Assert.AreSame(bind, aliasBind);
        }

        /// <summary>
        /// 检测非法的获取绑定
        /// </summary>
        [Test]
        public void CheckIllegalGetBind()
        {
            var container = MakeContainer();
            container.Bind("CheckIllegalGetBind", (cont, param) => "hello world");

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.GetBind(string.Empty);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.GetBind(null);
            });
        }

        /// <summary>
        /// 检测是否拥有绑定
        /// </summary>
        [Test]
        public void CanHasBind()
        {
            var container = MakeContainer();
            var bind = container.Bind("CanHasBind", (cont, param) => "hello world");
            bind.Alias("AliasName");
            Assert.IsTrue(container.HasBind("CanHasBind"));
            Assert.IsTrue(container.HasBind("AliasName"));
            Assert.IsFalse(container.HasBind(typeof(ContainerTest).ToString()));
            bind.Alias<ContainerTest>();
            Assert.IsTrue(container.HasBind(typeof(ContainerTest).ToString()));
        }

        /// <summary>
        /// 检查是否是静态的函数
        /// </summary>
        [Test]
        public void CanIsStatic()
        {
            var container = MakeContainer();
            var bind = container.Bind("CanIsStatic", (cont, param) => "hello world", true);
            container.Bind("CanIsStaticNotStatic", (cont, param) => "hello world not static");

            bind.Alias("AliasName");
            Assert.IsTrue(container.IsStatic("CanIsStatic"));
            Assert.IsTrue(container.IsStatic("AliasName"));
            Assert.IsFalse(container.IsStatic("NoAliasName"));
            Assert.IsFalse(container.IsStatic("CanIsStaticNotStatic"));
            Assert.IsTrue(container.HasBind("CanIsStaticNotStatic"));
        }
        #endregion

        #region Alias
        /// <summary>
        /// 正常的设定别名
        /// </summary>
        [Test]
        public void CheckNormalAlias()
        {
            var container = MakeContainer();
            container.Bind("CheckNormalAlias", (cont, param) => "hello world");

            container.Instance("StaticService", "hello");
            Assert.DoesNotThrow(() =>
            {
                container.Alias("AliasName1", "CheckNormalAlias");
            });
            Assert.DoesNotThrow(() =>
            {
                container.Alias("AliasName2", "StaticService");
            });
        }

        /// <summary>
        /// 检测非法的别名输入
        /// </summary>
        [Test]
        public void CheckIllegalAlias()
        {
            var container = MakeContainer();
            container.Bind("CheckIllegalAlias", (cont, param) => "hello world");
            container.Alias("AliasName", "CheckIllegalAlias");

            Assert.Throws<RuntimeException>(() =>
            {
                container.Alias("AliasName", "CheckIllegalAlias");
            });

            Assert.Throws<RuntimeException>(() =>
            {
                container.Alias("AliasNameOther", "CheckNormalAliasNotExist");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Alias(string.Empty, "CheckIllegalAlias");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Alias(null, "CheckIllegalAlias");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Alias("AliasNameOther2", string.Empty);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Alias("AliasNameOther3", null);
            });
        }
        #endregion

        #region Call
        /// <summary>
        /// 被注入的测试类
        /// </summary>
        public class CallTestClassInject
        {
            public object GetNumber()
            {
                return 2;
            }
        }
        /// <summary>
        /// 调用测试类
        /// </summary>
        public class CallTestClass
        {
            public object GetNumber(CallTestClassInject cls)
            {
                return cls != null ? cls.GetNumber() : 1;
            }
        }

        /// <summary>
        /// 调用测试类
        /// </summary>
        public class CallTestClassLoopDependency
        {
            public object GetNumber(LoopDependencyClass cls)
            {
                return 1;
            }
        }

        public class LoopDependencyClass
        {
            public LoopDependencyClass(LoopDependencyClass2 cls)
            {
        
            }
        }

        public class LoopDependencyClass2
        {
            public LoopDependencyClass2(LoopDependencyClass cls)
            {
        
            }
        }

        /// <summary>
        /// 循环依赖测试
        /// </summary>
        [Test]
        public void CheckLoopDependency()
        {
            var container = MakeContainer();
            container.Bind<LoopDependencyClass>();
            container.Bind<LoopDependencyClass2>();

            var cls = new CallTestClassLoopDependency();

            Assert.Throws<RuntimeException>(() =>
            {
                container.Call(cls, "GetNumber");
            });
        }

        /// <summary>
        /// 可以调用方法
        /// </summary>
        [Test]
        public void CanCallMethod()
        {
            var container = MakeContainer();
            container.Bind<CallTestClassInject>();
            var cls = new CallTestClass();

            var result = container.Call(cls, "GetNumber");
            Assert.AreEqual(2, result);
        }

        /// <summary>
        /// 测试无效的调用方法
        /// </summary>
        [Test]
        public void CheckIllegalCallMethod()
        {
            var container = MakeContainer();
            container.Bind<CallTestClassInject>();
            var cls = new CallTestClass();

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Call(null, "GetNumber");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Call(cls, "GetNumberIllegal");
            });
        }

        /// <summary>
        /// 测试无效的传入参数
        /// </summary>
        [Test]
        public void CheckIllegalCallMethodParam()
        {
            var container = MakeContainer();
            container.Bind<CallTestClassInject>();
            var cls = new CallTestClass();

            var result = container.Call(cls, "GetNumber" , "illegal param");
            Assert.AreEqual(2, result);

            result = container.Call(cls, "GetNumber", null);
            Assert.AreEqual(2, result);
        }
        #endregion

        /// <summary>
        /// 生成容器
        /// </summary>
        /// <returns>容器</returns>
        private CatLib.Container.Container MakeContainer()
        {
            var container = new CatLib.Container.Container();
            return container;
        }
    }
}