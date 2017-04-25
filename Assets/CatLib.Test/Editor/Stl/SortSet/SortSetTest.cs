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
    /// 跳跃链表测试
    /// </summary>
    [TestFixture]
    class SortSetTest
    {
        [Test]
        public void TestSortedList()
        {
            var list = new SortedList<int , int>();
            int num = 500000;
            var rand = new Random();
            for (var i = 0; i < num; i++)
            {
                var val = rand.Next();
                if (!list.ContainsKey(val))
                {
                    list.Add(val, val);
                }
                //list.Add(i,i);
            }
            var max = 0;
            foreach (var val in list)
            {
                if (max <= val.Key)
                {
                    max = val.Key;
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// 增加元素测试
        /// </summary>
        [Test]
        public void AddElementTest()
        {
            //var list2 = new SortSet<object, int>();
            int num = 500000;
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

                if ((rank - 1) != val)
                {
                    Assert.Fail();
                }
            }
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
                if (list.GetRank(n) != n + 1)
                {
                    Assert.Fail();
                }
            }
        }
    }
}
