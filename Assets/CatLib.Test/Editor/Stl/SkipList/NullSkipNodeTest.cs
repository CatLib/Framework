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
using CatLib.Stl;
using NUnit.Framework;

namespace CatLib.Test.Stl
{
    /// <summary>
    /// 空的跳跃结点测试
    /// </summary>
    [TestFixture]
    public class NullSkipNodeTest
    {
        /// <summary>
        /// 新建一个跳跃结点
        /// </summary>
        [Test]
        public void NewNullSkipNode()
        {
            Assert.DoesNotThrow(() =>
            {
                new NullSkipNode<string, string>(32);
            });
        }

        /// <summary>
        /// 空结点的比较
        /// </summary>
        [Test]
        public void CheckNullSkipNodeEquals()
        {
            Assert.AreEqual(true, new NullSkipNode<string, string>(32).Equals(new NullSkipNode<string, string>(32)));
            Assert.AreEqual(false, new NullSkipNode<string, string>(32).Equals(new NullSkipNode<string, object>(32)));
        }

        /// <summary>
        /// 测试无效的空结点数据
        /// </summary>
        [Test]
        public void CheckIllegalNullSkipNodeData()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new NullSkipNode<string, string>(0);
            });
        }
    }
}
