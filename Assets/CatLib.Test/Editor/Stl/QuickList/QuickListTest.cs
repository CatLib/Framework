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

using System.Collections.Generic;
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
            var lst = new List<string>();
            for (var i = 0; i < 500000; i++)
            {
                lst.Add(i.ToString());
            }
            for (var i = 0; i < 500000; i++)
            {
                var a = lst[i];
            }
        }
    }
}
