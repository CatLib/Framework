﻿/*
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
#if UNITY_EDITOR
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Container
{
    /// <summary>
    /// 给与数据测试用例
    /// </summary>
    [TestClass]
    public class GivenDataTest
    {
        /// <summary>
        /// 可以给与数据
        /// </summary>
        [TestMethod]
        public void CanGiven()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CanGiven", (app, param) => "hello world", false);
            var givenData = new CatLib.Container.GivenData(bindData);
            givenData.Needs("needs1");
            givenData.Given("hello");
            Assert.AreEqual("hello", bindData.GetContextual("needs1"));

            givenData = new CatLib.Container.GivenData(bindData);
            givenData.Needs("needs2");
            givenData.Given<GivenDataTest>();
            Assert.AreEqual(typeof(GivenDataTest).ToString(), bindData.GetContextual("needs2"));
        }

        /// <summary>
        /// 检查给与的无效值
        /// </summary>
        [TestMethod]
        public void CheckGivenIllegalValue()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CanGiven", (app, param) => "hello world", false);
            var givenData = new CatLib.Container.GivenData(bindData);
            givenData.Needs("needs");

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                givenData.Given(null);
            });
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                givenData.Given(string.Empty);
            });
        }
    }
}