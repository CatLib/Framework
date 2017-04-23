
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
        public void AddElementTest()
        {
            var list = new SkipList<int, string>(0.5, 32);
            list.Add(5, "this is 5");
            list.Add(10, "this is 10");
            list.Add(20, "this is 20");
            list.Add(15, "this is 15");
            list.Add(30, "this is 30");
            list.Add(25, "this is 25");
            list.Add(0, "this is 0");

            list.Remove(25);
            list.Remove(12);
            list.Remove(10);
            foreach (var val in list)
            {
                UnityEngine.Debug.Log(val);
            }
        }
    }
}
