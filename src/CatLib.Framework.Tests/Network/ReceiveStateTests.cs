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
using System.Text;
using CatLib.Network;
using CatLib.Network.Packer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Network
{
    [TestClass]
    public class ReceiveStateTests
    {
        [TestMethod]
        public void MulReceiveTests()
        {
            var state = new ReceiveState(new TextPacker());
            Exception ex;
            var result = state.Input(Encoding.Default.GetBytes("hello"), out ex);
            Assert.AreEqual(null, result);
            result = state.Input(Encoding.Default.GetBytes("world\n"), out ex);
            Assert.AreEqual("helloworld", Encoding.Default.GetString((byte[])result[0]));
            result = state.Input(Encoding.Default.GetBytes("hello"), out ex);
            Assert.AreEqual(null, result);
            result = state.Input(Encoding.Default.GetBytes("shanghai\n"), out ex);
            Assert.AreEqual("helloshanghai", Encoding.Default.GetString((byte[])result[0]));

            result = state.Input(Encoding.Default.GetBytes("shanghai\nchina\n"), out ex);
            Assert.AreEqual("shanghai", Encoding.Default.GetString((byte[])result[0]));
            Assert.AreEqual("china", Encoding.Default.GetString((byte[])result[1]));
        }
    }
}
