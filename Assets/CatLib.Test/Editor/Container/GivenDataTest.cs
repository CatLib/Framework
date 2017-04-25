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

namespace CatLib.Test.Container
{
    /// <summary>
    /// 给与数据测试用例
    /// </summary>
    [TestFixture]
    public class GivenDataTest
    {
        /// <summary>
        /// 可以给与数据
        /// </summary>
        [Test]
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
        [Test]
        public void CheckGivenIllegalValue()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CanGiven", (app, param) => "hello world", false);
            var givenData = new CatLib.Container.GivenData(bindData);
            givenData.Needs("needs");

            Assert.Throws<ArgumentNullException>(() =>
            {
                givenData.Given(null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                givenData.Given(string.Empty);
            });
        }
    }
}