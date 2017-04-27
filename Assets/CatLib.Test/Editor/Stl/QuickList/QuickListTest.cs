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
using System.Collections.Generic;
using System.Diagnostics;
using CatLib.Stl;
using NUnit.Framework;

namespace CatLib.Test.Stl
{
    /// <summary>
    /// 快速列表测试
    /// </summary>
    [TestFixture]
    class QuickListTest
    {
        /// <summary>
        /// 系统的List测试
        /// </summary>
        [Test]
        public void TestSystemList()
        {
            var lst = new List<object>();
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 50000; i++)
            {
                lst.Add(new object());
            }
            for (var i = 0; i < 50000; i++)
            {
                lst.RemoveAt(0);
            }
            sw.Stop();
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 推入数据到尾部测试
        /// </summary>
        [Test]
        public void PushShiftData()
        {
            var num = 50000;
            var lst = new QuickList<int>();
            var rand = new Random();
            var lst2 = new List<int>();
            for (var i = 0; i < num; i++)
            {
                var v = rand.Next();
                lst.Push(v);
                lst2.Add(v);
            }
            foreach (var v in lst2)
            {
                Assert.AreEqual(v, lst.Shift());
            }
            Assert.AreEqual(0, lst.Count);
            Assert.AreEqual(0, lst.Length);
        }

        /// <summary>
        /// 头部推入和尾部推出测试
        /// </summary>
        [Test]
        public void UnShiftPopTest()
        {
            var num = 50000;
            var lst = new QuickList<int>();
            var rand = new Random();
            var lst2 = new List<int>();
            for (var i = 0; i < num; i++)
            {
                var v = rand.Next();
                lst.Unshift(v);
                lst2.Add(v);
            }
            foreach (var v in lst2)
            {
                Assert.AreEqual(v, lst.Pop());
            }
            Assert.AreEqual(0 , lst.Count);
            Assert.AreEqual(0, lst.Length);
        }
    }
}
