using System;
using System.Collections.Generic;
using System.Diagnostics;
using CatLib.Stl;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace CatLib.Test.Stl
{
    /// <summary>
    /// SortSet测试
    /// </summary>
    [TestFixture]
    class SortSetTest
    {
        /// <summary>
        /// 对象插入测试
        /// </summary>
        [Test]
        public void ObjectInsertTest()
        {
            var list = new SortSet<object, int>();
            var objs = new List<object>();
            for (var i = 0; i < 10; i++)
            {
                var o = new object();
                objs.Add(o);
                list.Add(o, i);
            }

            var n = 0;
            foreach (var o in list)
            {
                Assert.AreSame(objs[n++], o);
            }
        }

        /// <summary>
        /// 根据排名区间获取元素
        /// </summary>
        [Test]
        public void GetElementRangeByRank()
        {
            var list = new SortSet<int, int>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(i, i);
            }

            var elements = list.GetElementRangeByRank(3, 8);
            var n = 3;
            foreach (var e in elements)
            {
                Assert.AreEqual(n++, e);
            }

            var list2 = new SortSet<int, int>();
            Assert.AreEqual(0, list2.GetElementRangeByRank(3, 8).Length);
        }

        /// <summary>
        /// 根据分数区间获取元素
        /// </summary>
        [Test]
        public void GetElementRangeByScore()
        {
            var list = new SortSet<int, int>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(i, i);
            }

            var elements = list.GetElementRangeByScore(3, 8);
            var n = 3;
            foreach (var e in elements)
            {
                Assert.AreEqual(n++ , e);
            }
            var list2 = new SortSet<int, int>();
            Assert.AreEqual(0, list2.GetElementRangeByScore(3, 8).Length);
        }

        /// <summary>
        /// 根据分数区间移除元素
        /// </summary>
        [Test]
        [Repeat(100)]
        public void RemoveRangeByScore()
        {
            var list = new SortSet<int, int>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(i, i);
            }
            list.RemoveRangeByScore(3, 8);
            Assert.AreEqual(0, list.GetElementByRank(0));
            Assert.AreEqual(1, list.GetElementByRank(1));
            Assert.AreEqual(2, list.GetElementByRank(2));
            Assert.AreEqual(9, list.GetElementByRank(3));
            for (var i = 3; i < 9; i++)
            {
                list.Add(i, i);
            }
            list.Add(33, 3);
            list.RemoveRangeByScore(3, 3);
            Assert.AreEqual(0, list.GetElementByRank(0));
            Assert.AreEqual(1, list.GetElementByRank(1));
            Assert.AreEqual(2, list.GetElementByRank(2));
            Assert.AreEqual(4, list.GetElementByRank(3));
        }

        /// <summary>
        /// 根据排名区间移除元素
        /// </summary>
        [Test]
        [Repeat(100)]
        public void RemoveRangeByRank()
        {
            var list = new SortSet<int, int>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(i, i);
            }

            list.RemoveRangeByRank(3, 8);
            Assert.AreEqual(0, list.GetElementByRank(0));
            Assert.AreEqual(1, list.GetElementByRank(1));
            Assert.AreEqual(2, list.GetElementByRank(2));
            Assert.AreEqual(9, list.GetElementByRank(3));
            for (var i = 3; i < 9; i++)
            {
                list.Add(i, i);
            }
            list.Add(33, 3);
            list.RemoveRangeByRank(3, 3);
            Assert.AreEqual(0, list.GetElementByRank(0));
            Assert.AreEqual(1, list.GetElementByRank(1));
            Assert.AreEqual(2, list.GetElementByRank(2));
            var r = list.GetElementByRank(3);
            Assert.AreEqual(true, r == 3 || r == 33);
        }

        /// <summary>
        /// 根据排名获取元素 (有序集成员按照Score从大到小排序)
        /// </summary>
        [Test]
        [Repeat(100)]
        public void GetElementByRevRank()
        {
            var list = new SortSet<int, int>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(i, i);
            }
            Assert.AreEqual(6, list.GetElementByRevRank(3));
            Assert.AreEqual(9, list.GetElementByRevRank(0));
            Assert.AreEqual(0, list.GetElementByRevRank(9));
        }

        /// <summary>
        /// 反向迭代遍历
        /// </summary>
        [Test]
        public void ReversEnumerator()
        {
            int num = 50000;
            var list = new SortSet<int, int>();
            for (int i = 0; i < num; i++)
            {
                list.Add(i, i);
            }

            int n = 0;
            var e = list.ReversEnumerator();
            while (e.MoveNext())
            {
                var v = e.Current;
                Assert.AreEqual(num - ++n, v);
            }
        }

        /// <summary>
        /// 边界值测试
        /// </summary>
        [Test]
        public void BoundTestScoreRangeCount()
        {
            var list = new SortSet<int, int>();
            list.Add(6, 6);
            Assert.AreEqual(1, list.GetRangeCount(0, 100));
            Assert.AreEqual(0, list.GetRangeCount(7, 100));
            Assert.AreEqual(0, list.GetRangeCount(0, 5));
            Assert.AreEqual(1, list.GetRangeCount(6, 100));

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Assert.AreEqual(0, list.GetRangeCount(800, 100));
            });
        }

        /// <summary>
        /// 空的列表执行分数区间内的元素数量查询
        /// </summary>
        [Test]
        public void EmptyListScoreRangeCount()
        {
            var list = new SortSet<int, int>();
            Assert.AreEqual(0, list.GetRangeCount(0, 100));
        }

        /// <summary>
        /// 分数区间内的元素数量
        /// </summary>
        [Test]
        public void ScoreRangeCount()
        {
            var list = new SortSet<int, int>();
            var lst = new List<int>();
            int num = 50000;
            var rand = new Random();
            for (var i = 0; i < num; i++)
            {
                var s = rand.Next(0, 1000);
                list.Add(i, s);
                if (s <= 100)
                {
                    lst.Add(i);
                }
            }
            Assert.AreEqual(lst.Count, list.GetRangeCount(0, 100));
        }

        /// <summary>
        /// 增加元素测试
        /// </summary>
        [Test]
        public void AddElementTest()
        {
            //var list2 = new SortSet<object, int>();
            int num = 50000;
            var list = new SortSet<int, int>();
            var rand = new Random();

            for (var i = 0; i < num; i++)
            {
                var val = rand.Next();
                list.Add(val, val);
                //list.Add(i,i);
            }

            var max = 0;
            foreach (var val in list)
            {
                if (max <= val)
                {
                    max = val;
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// 删除测试
        /// </summary>
        [Test]
        public void RemoveTest()
        {
            int num = 50000;
            var list = new SortSet<int, int>();
            var lst = new List<int>();
            var rand = new Random();

            for (var i = 0; i < num; i++)
            {
                var val = rand.Next();
                lst.Add(val);
                list.Add(val, val);
            }

            foreach (int n in lst)
            {
                list.Remove(n);
            }

            foreach (var val in list)
            {
                Assert.Fail();
            }

            if (list.Count == 0)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// 根据排名获取元素
        /// </summary>
        [Test]
        public void GetElementByRank()
        {
            var num = 50000;
            var list = new SortSet<int, int>();

            for (var i = 0; i < num; i++)
            {
                list.Add(i, i);
            }
            var rand = new Random();
            for (var i = 0; i < Math.Min(num, 100); i++)
            {
                var rank = rand.Next(0, num);
                var val = list.GetElementByRank(rank);

                if (rank != val)
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// 获取排名 , 有序集成员按照Score从大到小排序
        /// </summary>
        [Test]
        public void GetRevRank()
        {
            var list = new SortSet<int, int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(i, i);
            }
            Assert.AreEqual(6, list.GetRevRank(3));
        }

        /// <summary>
        /// 获取排名位置
        /// </summary>
        [Test]
        [Repeat(100)]
        public void GetRankTest()
        {
            var num = 100;
            var list = new SortSet<int, int>();
            var lst = new List<int>();
            var rand = new Random();

            for (var i = 0; i < num; i++)
            {
                if (rand.NextDouble() < 0.1)
                {
                    lst.Add(i);
                }
                list.Add(i, i);
            }

            foreach (var n in lst)
            {
                if (list.GetRank(n) != n)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual(-1, list.GetRank(-1));
        }

        /// <summary>
        /// 顺序插入测试
        /// </summary>
        [Test]
        public void SequentialAddTest()
        {
            var list = new SortSet<int, int>();
            for (var i = 0; i < 500000; i++)
            {
                list.Add(i, i);
            }

            int n = 0;
            foreach (var v in list)
            {
                Assert.AreEqual(n++, v);
            }
        }

        [Test]
        public void PerformanceTesting()
        {
            var lst = new List<int>();
            var random = new Random();
            for (var i = 0; i < 50000; i++)
            {
                lst.Add(random.Next());
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var CatLibSortSet = new SortSet<int, int>();
            foreach (var v in lst)
            {
                CatLibSortSet.Add(v, v);
            }

            stopwatch.Stop();

            UnityEngine.Debug.Log("CatLibSortSet :" + stopwatch.Elapsed.TotalMilliseconds);

            stopwatch.Reset();

            stopwatch.Start();
            var SystemSortList = new SortedList<int ,int>();
            foreach (var v in lst)
            {
                if (!SystemSortList.ContainsKey(v))
                {
                    SystemSortList.Add(v, v);
                }
            }

            stopwatch.Stop();

            UnityEngine.Debug.Log("SystemSortList :" + stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
