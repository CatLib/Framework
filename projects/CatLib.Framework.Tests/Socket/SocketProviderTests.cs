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

using System.Threading;
using CatLib.API.Socket;
using CatLib.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Socket.Tests
{
    [TestClass]
    public class SocketProviderTests
    {
        private IApplication MakeEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new EventsProvider());
            app.Register(new SocketProvider());
            app.Init();
            return app;
        }

        /// <summary>
        /// 基础连接测试
        /// </summary>
        [TestMethod]
        public void TestGetSocket()
        {
            var app = MakeEnv();
            var factory = app.Make<ISocketManager>();

            var tcp = factory.Make("tcp://127.0.0.1:7777", "tcp");
            var kcp = factory.Make("kcp://8787@127.0.0.1:7777", "kcp");

            Assert.AreNotEqual(null, tcp);
            Assert.AreNotEqual(null, kcp);

            Assert.AreEqual(tcp, factory["tcp"]);
            Assert.AreEqual(kcp, factory["kcp"]);
        }

        [TestMethod]
        public void TestUndefiendProtocol()
        {
            var app = MakeEnv();
            var factory = app.Make<ISocketManager>();

            var isThrow = false;
            try
            {
                factory.Make("undefiend://127.0.0.1:7777", "undefiend");
            }
            catch(RuntimeException)
            {
                isThrow = true;
            }

            Assert.AreEqual(true, isThrow);
        }

        [TestMethod]
        public void TestNotSetConv()
        {
            var app = MakeEnv();
            var factory = app.Make<ISocketManager>();

            var isThrow = false;
            try
            {
                factory.Make("kcp://127.0.0.1:7777", "kcp");
            }
            catch (RuntimeException)
            {
                isThrow = true;
            }

            Assert.AreEqual(true, isThrow);
        }

        [TestMethod]
        public void TestRelease()
        {
            var app = MakeEnv();
            var factory = app.Make<ISocketManager>();

            var kcp = factory.Make("kcp://213@127.0.0.1:999", "kcp");
            factory.Release("kcp");
            var newKcp = factory.Make("kcp://213@127.0.0.1:999", "kcp");

            Assert.AreNotEqual(kcp, newKcp);
        }

        [TestMethod]
        public void TestReleaseSocketFactory()
        {
            var app = MakeEnv();
            var factory = app.Make<ISocketManager>();
            app.Release<ISocketManager>();
            var newFactory = app.Make<ISocketManager>();

            Assert.AreNotEqual(factory, newFactory);
        }
    }
}
