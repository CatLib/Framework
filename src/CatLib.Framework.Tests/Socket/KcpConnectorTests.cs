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
using System.Net.Sockets;
using System.Threading;
using CatLib.API.Socket;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Socket.Tests
{
    [TestClass]
    public class KcpConnectorTests
    {
        private static bool exitFlag;

        private static KcpConnector connector;

        private static object syncRoot = new object();

        [TestMethod]
        public void SimpleKcpTest()
        {
            lock (syncRoot)
            {
                connector = new KcpConnector(7758, "127.0.0.1", 7777);

                exitFlag = false;
                var thread = new Thread(Tick);
                thread.Start();

                var data = new byte[0];
                var server = new KcpTestsServer(7758, (d) =>
                {
                    data = d;
                });

                var reData = new byte[0];
                connector.On(SocketEvents.Message, (d) =>
                {
                    reData = d as byte[];
                });

                var wait = connector.Connect();

                if (!Util.Wait(wait, 3000))
                {
                    exitFlag = true;
                    server.Dispose();
                    connector.Dispose();
                    Assert.Fail("wait faild");
                }

                wait = connector.Send(System.Text.Encoding.Default.GetBytes("hello world"));

                if (!Util.Wait(wait, 3000) ||
                    !Util.Wait(ref data, 3000) ||
                    !Util.Wait(ref reData, 3000))
                {
                    exitFlag = true;
                    server.Dispose();
                    connector.Dispose();
                    Assert.Fail("wait faild");
                }

                exitFlag = true;
                server.Dispose();
                connector.Disconnect();

                Assert.AreEqual("hello world", System.Text.Encoding.Default.GetString(data));
                Assert.AreEqual("hello world", System.Text.Encoding.Default.GetString(reData));
            }
        }

        private void Tick()
        {
            while (!exitFlag)
            {
                connector.Tick();
                Thread.Sleep(10);
            }
        }

        [TestMethod]
        public void TestDoubleConnect()
        {
            var connector = new KcpConnector(100, "127.0.0.1", 7777);
            var awaits = connector.Connect();

            if (!Util.Wait(awaits, 3000))
            {
                Assert.Fail("wait faild");
            }

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
            var connector = new KcpConnector(100, "127.0.0.1", 7777);
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
        public void TestNotExistsOff()
        {
            var connector = new KcpConnector(100, "127.0.0.1", 7777);
            connector.Off("NotExists", (p) => { });
        }

        [TestMethod]
        public void TestRemoveAllOff()
        {
            var connector = new KcpConnector(100, "127.0.0.1", 7777);
            Action<object> call = (ex) =>
            {
                Assert.Fail();
            };
            connector.On(SocketEvents.Error, call);
            connector.On(SocketEvents.Error, call);
            connector.Off(SocketEvents.Error, call);
            connector.Trigger(SocketEvents.Error, new Exception());
        }

        [TestMethod]
        public void DoubleDisposeTest()
        {
            var connector = new KcpConnector(123,"127.0.0.1", 7777);
            connector.Dispose();
            connector.Dispose();
        }

        [TestMethod]
        public void TestDnsException()
        {
            var connector = new KcpConnector(123, "http://123.0.0.1", 7777);
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
