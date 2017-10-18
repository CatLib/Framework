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
    public class FramePackerTests
    {
        [TestMethod]
        public void InputTests()
        {
            var packer = new FramePacker();
            Exception ex;
            var length = packer.Input(Arr.Merge(BitConverter.GetBytes(14), Encoding.Default.GetBytes("helloworld")), out ex);
            Assert.AreEqual(14, length);
        }

        [TestMethod]
        public void DecodeTest()
        {
            var packer = new FramePacker();
            Exception ex;
            var data = packer.Decode(Arr.Merge(BitConverter.GetBytes(14), Encoding.Default.GetBytes("helloworld")), out ex);
            Assert.AreEqual("helloworld", Encoding.Default.GetString((byte[])data));
        }

        [TestMethod]
        public void EncodeTest()
        {
            var packer = new FramePacker();
            Exception ex;
            var data = packer.Encode(Encoding.Default.GetBytes("helloworld"), out ex);
            Assert.AreEqual("14helloworld",
                BitConverter.ToInt32(Arr.Slice(data, 0, 4), 0) + Encoding.Default.GetString(Arr.Slice(data, 4)));
        }

        [TestMethod]
        public void TestEncodeNullData()
        {
            var packer = new FramePacker();
            Exception ex;
            var data = packer.Encode(null, out ex);

            Assert.AreEqual(null, data);
            Assert.AreNotEqual(null, ex);
        }

        [TestMethod]
        public void TestInputNullData()
        {
            var packer = new FramePacker();
            Exception ex;
            var data = packer.Input(null, out ex);

            Assert.AreEqual(0, data);
        }
    }
}
