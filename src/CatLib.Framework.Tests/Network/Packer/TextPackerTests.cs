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
using CatLib.Network.Packer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Network.Packer
{
    [TestClass]
    public class TextPackerTests
    {
        [TestMethod]
        public void InputTests()
        {
            var packer = new TextPacker();
            Exception ex;
            var length = packer.Input(Encoding.Default.GetBytes("hello\nworld"), out ex);
            Assert.AreEqual(6, length);

            length = packer.Input(Encoding.Default.GetBytes("helloworld"), out ex);
            Assert.AreEqual(0, length);
        }

        [TestMethod]
        public void DecodeTest()
        {
            var packer = new TextPacker();
            Exception ex;
            var data = packer.Decode(Encoding.Default.GetBytes("hello\n"), out ex);
            Assert.AreEqual("hello", Encoding.Default.GetString((byte[])data));
        }

        [TestMethod]
        public void EncodeTest()
        {
            var packer = new TextPacker();
            Exception ex;
            var data = packer.Encode(Encoding.Default.GetBytes("helloworld"), out ex);
            Assert.AreEqual("helloworld\n", Encoding.Default.GetString((byte[])data));
        }

        [TestMethod]
        public void MulDataInput()
        {
            var packer = new TextPacker();
            Exception ex;
            var data = packer.Input(Encoding.Default.GetBytes("hello"), out ex);
            Assert.AreEqual(0, data);
            data = packer.Input(Encoding.Default.GetBytes("helloworld\n"), out ex);
            Assert.AreEqual(11, data);
        }

        [TestMethod]
        public void TestEncodeNullData()
        {
            var packer = new TextPacker();
            Exception ex;
            var data = packer.Encode(null, out ex);

            Assert.AreEqual(null, data);
            Assert.AreNotEqual(null, ex);
        }
    }
}
