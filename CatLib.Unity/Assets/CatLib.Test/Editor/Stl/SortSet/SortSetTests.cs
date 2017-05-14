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
using CatLib.Stl;
using Random = System.Random;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Stl
{
    /// <summary>
    /// SortSet测试
    /// </summary>
    [TestClass]
    public class SortSetTests
    {
        /// <summary>
        /// 对象插入测试
        /// </summary>
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
                Assert.AreEqual(n++, e);
            }
            var list2 = new SortSet<int, int>();
            Assert.AreEqual(0, list2.GetElementRangeByScore(3, 8).Length);
        }

        /// <summary>
        /// 根据分数区间移除元素
        /// </summary>
        [TestMethod]
        public void RemoveRangeByScore()
        {
            for (var _ = 0; _ < 100; _++)
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
        }

        /// <summary>
        /// 根据排名区间移除元素
        /// </summary>
        [TestMethod]
        public void RemoveRangeByRank()
        {
            for (var _ = 0; _ < 100; _++)
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
        }

        /// <summary>
        /// 根据排名获取元素 (有序集成员按照Score从大到小排序)
        /// </summary>
        [TestMethod]
        public void GetElementByRevRank()
        {
            for (var _ = 0; _ < 100; _++)
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
        }

        /// <summary>
        /// 反向迭代遍历
        /// </summary>
        [TestMethod]
        public void ReversEnumerator()
        {
            int num = 50000;
            var list = new SortSet<int, int>();
            for (int i = 0; i < num; i++)
            {
                list.Add(i, i);
            }

            int n = 0;
            list.ReverseForeach();
            foreach (var v in list)
            {
                Assert.AreEqual(num - ++n, v);
            }
        }

        /// <summary>
        /// 边界值测试
        /// </summary>
        [TestMethod]
        public void BoundTestScoreRangeCount()
        {
            var list = new SortSet<int, int>();
            list.Add(6, 6);
            Assert.AreEqual(1, list.GetRangeCount(0, 100));
            Assert.AreEqual(0, list.GetRangeCount(7, 100));
            Assert.AreEqual(0, list.GetRangeCount(0, 5));
            Assert.AreEqual(1, list.GetRangeCount(6, 100));

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Assert.AreEqual(0, list.GetRangeCount(800, 100));
            });
        }

        /// <summary>
        /// 空的列表执行分数区间内的元素数量查询
        /// </summary>
        [TestMethod]
        public void EmptyListScoreRangeCount()
        {
            var list = new SortSet<int, int>();
            Assert.AreEqual(0, list.GetRangeCount(0, 100));
        }

        /// <summary>
        /// 分数区间内的元素数量
        /// </summary>
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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

            if (list.Count != 0)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// 根据排名获取元素
        /// </summary>
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
        public void GetRankTest()
        {
            for (var _ = 0; _ < 100; _++)
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
        }

        /// <summary>
        /// 顺序插入测试
        /// </summary>
        [TestMethod]
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

        /// <summary>
        /// 空列表遍历
        /// </summary>
        [TestMethod]
        public void EmptyListForeach()
        {
            var master = new SortSet<int, int>();
            foreach (var v in master)
            {
                Assert.Fail();
            }

            master.ReverseForeach();
            foreach (var v in master)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// 删除已经存在的元素
        /// </summary>
        [TestMethod]
        public void OverrideElement()
        {
            var master = new SortSet<int, int>();
            master.Add(10, 100);
            master.Add(10, 200);
            Assert.AreEqual(200, master.GetScore(10));
        }

        /// <summary>
        /// 是否包含元素
        /// </summary>
        [TestMethod]
        public void ContainsTest()
        {
            var master = new SortSet<int, int>();
            master.Add(10, 100);

            Assert.AreEqual(true, master.Contains(10));
            Assert.AreEqual(false, master.Contains(11));
        }

        /// <summary>
        /// 获取分数测试
        /// </summary>
        [TestMethod]
        public void GetScoreTest()
        {
            var master = new SortSet<int, int>();
            master.Add(10, 100);
            Assert.AreEqual(100, master.GetScore(10));
        }

        /// <summary>
        /// 获取同步
        /// </summary>
        [TestMethod]
        public void GetSyncRootTest()
        {
            var master1 = new SortSet<int, int>();
            var master2 = new SortSet<int, int>();

            Assert.AreNotSame(master1, master2);
        }

        /// <summary>
        /// GetElementByRank 溢出测试
        /// </summary>
        [TestMethod]
        public void GetElementByRankOverflowTest()
        {
            var master = new SortSet<int, int>();
            master.Add(10, 10);

            Assert.AreEqual(0, master.GetElementByRank(1000));
        }

        /// <summary>
        /// GetElementByRank 空内容测试
        /// </summary>
        [TestMethod]
        public void GetElementByRankEmptyTest()
        {
            var master = new SortSet<int, int>();
            Assert.AreEqual(0, master.GetElementByRank(100));
        }

        /// <summary>
        /// 获取排名溢出测试
        /// </summary>
        [TestMethod]
        public void GetRankOverflowTest()
        {
            var master = new SortSet<int, int>();
            master.Add(10, 100);

            Assert.AreEqual(-1, master.GetRank(100));
        }

        /// <summary>
        /// 获取排名反转溢出测试
        /// </summary>
        [TestMethod]
        public void GetRevRankOverflowTest()
        {
            var master = new SortSet<int, int>();
            master.Add(10, 100);

            Assert.AreEqual(-1, master.GetRevRank(100));
            Assert.AreEqual(0, master.GetRevRank(10));
        }

        /// <summary>
        /// 最大等级约束测试
        /// </summary>
        [TestMethod]
        public void MaxLevelLimitTest()
        {
            var master = new SortSet<int, int>(0.5, 3);
            for (var i = 0; i < 65536; i++)
            {
                master.Add(i, i);
            }
        }

        /// <summary>
        /// 清空测试
        /// </summary>
        [TestMethod]
        public void ClearTest()
        {
            var master = new SortSet<int, int>(0.25, 32);
            for (var i = 0; i < 65536; i++)
            {
                master.Add(i, i);
            }
            master.Clear();
            for (var i = 0; i < 65536; i++)
            {
                master.Add(i, i);
            }

            for (var i = 0; i < 65536; i++)
            {
                Assert.AreEqual(i, master.GetRank(i));
            }

            Assert.AreEqual(65536, master.Count);
        }
    }
}
