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

using CatLib.API.Socket;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Sockets;

namespace CatLib.Socket.Tests
{
    [TestClass]
    public class TcpConnectorTests
    {
        /// <summary>
        /// 基础连接测试
        /// </summary>
        [TestMethod]
        public void SimpleTcpTest()
        {
            var connector = new TcpConnector("127.0.0.1", 7777);

            var data = new byte[0];
            var server = new TcpTestsServer(b =>
            {
                data = b;
            });

            var reData = new byte[0];
            connector.On(SocketEvents.Message, (d) =>
            {
                reData = d as byte[];
            });

            var wait = connector.Connect();

            if (!Util.Wait(wait, 3000))
            {
                server.Dispose();
                connector.Dispose();
                Assert.Fail("wait faild");
            }

            wait = connector.Send(System.Text.Encoding.Default.GetBytes("hello world"));

            if (!Util.Wait(wait, 3000) ||
                !Util.Wait(ref data, 3000) ||
                !Util.Wait(ref reData, 3000))
            {
                server.Dispose();
                connector.Dispose();
                Assert.Fail("wait faild");
            }

            server.Dispose();
            connector.Disconnect();

            Assert.AreEqual("hello world", System.Text.Encoding.Default.GetString(data));
            Assert.AreEqual("hello world", System.Text.Encoding.Default.GetString(reData));
        }

        [TestMethod]
        public void TestDoubleConnect()
        {
            var connector = new TcpConnector("127.0.0.1", 7777);
            connector.Connect();

            var isThrow = false;
            try
            {
                connector.Connect();
            }
            catch (RuntimeException)
            {
                isThrow = true;
            }
            Assert.AreEqual(true, isThrow);
        }

        [TestMethod]
        public void TestOffEvent()
        {
            var connector = new TcpConnector("127.0.0.1", 7777);
            Action<object> call = (ex) =>
            {
                Assert.Fail();
            };
            connector.On(SocketEvents.Error, call);
            connector.On(SocketEvents.Error, call);

            connector.On(SocketEvents.Error, (ex) =>
            {
            });

            connector.Off(SocketEvents.Error, call);
            connector.Trigger(SocketEvents.Error, new Exception());
        }

        [TestMethod]
        public void DoubleDisposeTest()
        {
            var connector = new TcpConnector("127.0.0.1", 7777);
            connector.Dispose();
            connector.Dispose();
        }

        [TestMethod]
        public void TestDnsException()
        {
            var connector = new TcpConnector("http://123.0.0.1", 7777);
            Exception e = null;
            connector.On(SocketEvents.Error, (ex) =>
            {
                e = ex as Exception;
            });

            var wait = connector.Connect();

            if (!Util.Wait(wait, 10000))
            {
                connector.Dispose();
                Assert.Fail("wait faild");
            }

            Assert.AreEqual(typeof(SocketException), (wait.Result as Exception).GetType());
        }
    }
}
