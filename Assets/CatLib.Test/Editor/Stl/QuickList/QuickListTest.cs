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
            for (var i = 0; i < 500000; i++)
            {
                lst.Add(new object());
            }
            for (var i = 0; i < 500000; i++)
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
            var num = 500000;
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
            var num = 500000;
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

        /// <summary>
        /// 在扫描到的第一个元素之后插入
        /// </summary>
        [Test]
        public void InsertAfterTest()
        {
            var lst = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                lst.Push(i);
            }

            lst.InsertAfter(1, 999);
            lst.InsertAfter(3, 999);
            lst.InsertAfter(5, 999);
            lst.InsertAfter(7, 999);
            lst.InsertAfter(9, 999);
            
            Assert.AreEqual(999 ,lst[2]);
            Assert.AreEqual(999, lst[5]);
            Assert.AreEqual(999, lst[8]);
            Assert.AreEqual(999, lst[11]);

            Assert.AreEqual(11, lst.Count);
        }
    }
}
