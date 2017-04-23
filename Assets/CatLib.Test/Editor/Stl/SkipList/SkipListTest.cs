
using System;
using System.Collections.Generic;
using CatLib.Stl;
using NUnit.Framework;

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
            var list = new SkipList<int, string>(num);
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

        [Test]
        [MaxTime(3000)]
        public void PerformanceTesting()
        {
            var num = 40960;
            var list = new SkipList<int, string>(0.5 , 32);
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
            for (var i = 0; i < num; i++)
            {
                //var dicResult = dict[i];
                var result = list[i];
            }
        }
    }
}
