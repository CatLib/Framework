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