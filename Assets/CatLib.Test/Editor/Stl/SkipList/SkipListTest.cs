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
    class SkipListTest
    {
        /// <summary>
        /// 增加元素测试
        /// </summary>
        [Test]
        public void AddElementTest()
        {
            int num = 409600;
            var list = new SkipList<int, int>();
            var rand = new Random();

            for (var i = 0; i < num; i++)
            {
                var val = rand.Next();
                list.Add(val, val);
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
            int num = 409600;
            var list = new SkipList<int, int>();
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
                list.Remove(n, n);
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

        public struct MyStruct
        {
            public int A;
            public IList<MyStruct> Struc;
        }

        /// <summary>
        /// hello
        /// </summary>
        [Test]
        public void Test()
        {
            var a = new MyStruct();
            a.Struc = new MyStruct[3];
            UnityEngine.Debug.Log(a);
        }

        /// <summary>
        /// 获取随机层
        /// </summary>
        /// <returns>随机的层数</returns>
        [Test]
        public void BlockTest()
        {
            var random = new Random();
            var probability = 0.25*0xFFFF;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            int i = 0;
            while (i++ < 400000)
            {
                //var a = new MyStruct();

            }

            watch.Stop();
            string time = watch.ElapsedMilliseconds.ToString();
            UnityEngine.Debug.Log(time);
        }

        [Test]
        [MaxTime(3000)]
        public void PerformanceTesting()
        {
            
            var num = 409600;
            var list = new SkipList<int, string>(0.25 , 32);
            var dict = new Dictionary<int ,string>();
            var lst = new List<string>();
            for (var i = 0; i < num; i++)
            {
                list.Add(i, i.ToString());
                //lst.Add(i.ToString());
                //dict.Add(i , i.ToString());
            }
            //var a = list[120210];
            //UnityEngine.Debug.Log(a);
            //return;

            return;
            /*
            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (var i = 0; i < num; i++)
            {
                //var dicResult = dict[i];
                var result = list[i];
                //lst.Contains(i.ToString());
            }

            watch.Stop();
            string time = watch.ElapsedMilliseconds.ToString();
            UnityEngine.Debug.Log(time);*/
        }
    }
}
