using CatLib.Spl;
using NUnit.Framework;

namespace CatLib.Test.Stl
{
    /// <summary>
    /// 跳跃结点测试
    /// </summary>
    [TestFixture]
    public class SkipNodeTest
    {
        /// <summary>
        /// 新建一个跳跃结点
        /// </summary>
        [Test]
        public void NewSkipNode()
        {
            var node = new SkipNode<string, string>(32);
        }
    }
}