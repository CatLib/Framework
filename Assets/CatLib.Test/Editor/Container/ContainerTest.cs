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
                container.Tag("TestTag", null);
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


        /// <summary>
        /// 静态绑定方法
        /// </summary>
        [Test]
        public void BindFuncStatic()
        {
            var container = MakeContainer();
            container.Bind("BindFuncStatic", (cont, param) => "HelloWorld", true);

            var bind = container.GetBind("BindFuncStatic");
            var hasBind = container.HasBind("BindFuncStatic");
            var obj = container.Make("BindFuncStatic");

            Assert.AreNotEqual(null, bind);
            Assert.AreEqual(true, hasBind);
            Assert.AreEqual(true, bind.IsStatic);
            Assert.AreSame("HelloWorld", obj);
        }

        /// <summary>
        /// 非静态绑定
        /// </summary>
        [Test]
        public void BindFunc()
        {
            var container = MakeContainer();
            container.Bind("BindFunc", (cont, param) => new List<string>());

            var bind = container.Make("BindFunc");
            var bind2 = container.Make("BindFunc");

            Assert.AreNotEqual(null, bind);
            Assert.AreNotSame(bind, bind2);
        }

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