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
        public void AddElementTest([Random(2048,4096, 1)]int num)
        {
            var list = new SkipList<int, string>(0.25, 32);
            var rand = new Random();
            for (var i = 0; i < num; i++)
            {
                var val = rand.Next();
                list.Add(val, val.ToString());
            }

            var max = 0;
            foreach (var val in list)
            {
                if (max < int.Parse(val))
                {
                    max = int.Parse(val);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        struct MyStruct
        {
            /// <summary>
            /// 键
            /// </summary>
            public int Key { get; private set; }

            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; internal set; }

            /// <summary>
            /// 链接的结点
            /// </summary>
            public IList<SkipNode<int, string>> Links { get; private set; }

            /// <summary>
            /// 层跨越的结点数量
            /// </summary>
            public IList<int> Span { get; internal set; }
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
                var a = new MyStruct();

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
            Random random = new Random();
            for (var i = 0; i < num; i++)
            {
                var r = random.Next();
                list.Add( r, r.ToString());
                //dict.Add(i , i.ToString());
            }
            //var a = list[120210];
            //UnityEngine.Debug.Log(a);
            //return;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (var i = 0; i < num; i++)
            {
                //var dicResult = dict[i];
                var result = list[i];
            }

            watch.Stop();
            string time = watch.ElapsedMilliseconds.ToString();
            UnityEngine.Debug.Log(time);
        }
    }
}
