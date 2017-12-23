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
using System.Threading;
using CatLib.API.Network;
using CatLib.API.Socket;
using CatLib.Network;
using CatLib.Socket;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Network
{
    [TestClass]
    public class NetworkProviderTests
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private object syncRoot = new object();

        /// <summary>
        /// 生成环境
        /// </summary>
        /// <returns></returns>
        private IApplication MakeEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new NetworkProvider());
            app.Register(new SocketProvider());
            app.Init();
            return app;
        }

        [TestMethod]
        public void SimpleTextTests()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();

                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("text.tcp://127.0.0.1:7777");
                    var backMessage = string.Empty;
                    channel.OnMessage += (c, d) =>
                    {
                        backMessage = Encoding.Default.GetString((byte[])d);
                    };

                    var count = 0;
                    var wait = channel.Connect();

                    try
                    {
                        while (!wait.IsDone && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }

                        channel.Send(Encoding.Default.GetBytes("helloworld"));

                        count = 0;
                        while (backMessage == string.Empty && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }

                        Assert.AreEqual("helloworld", backMessage);
                        Assert.AreEqual("helloworld", Encoding.Default.GetString(Arr.Slice(fromClient, 0, -1)));
                    }
                    finally
                    {
                        channel.Disconnect();
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleFrameTest()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();

                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("frame.tcp://127.0.0.1:7777");
                    var backMessage = string.Empty;
                    channel.OnMessage += (c, d) =>
                    {
                        backMessage = Encoding.Default.GetString((byte[])d);
                    };

                    var count = 0;
                    var wait = channel.Connect();
                    try
                    {
                        while (!wait.IsDone && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }

                        channel.Send(Encoding.Default.GetBytes("helloworld"));

                        count = 0;
                        while (backMessage == string.Empty && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }

                        Assert.AreEqual("helloworld", backMessage);
                        Assert.AreEqual("helloworld", Encoding.Default.GetString(Arr.Slice(fromClient, 4)));
                    }
                    finally
                    {
                        channel.Disconnect();
                    }
                }
            }
        }

        [TestMethod]
        public void HeatBeatTest()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();
                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("frame.tcp://127.0.0.1:7777");
                    channel.SetHeartBeat(500);
                    channel.Connect();

                    try
                    {
                        var missCount = 0;
                        channel.OnMissHeartBeat += (c, num) =>
                        {
                            missCount = num;
                        };

                        var missCountGlobal = 0;
                        manager.OnMissHeartBeat += (c, num) =>
                        {
                            missCountGlobal = num;
                        };

                        var count = 0;
                        while (missCount != 2 && count++ < 6000)
                        {
                            (manager as NetworkManager).Tick(1);
                            Thread.Sleep(1);
                        }

                        Assert.AreEqual(2, missCount);
                        Assert.AreEqual(2, missCountGlobal);
                        channel.Send(Encoding.Default.GetBytes("helloworld"));
                        Thread.Sleep(600);
                        (manager as NetworkManager).Tick(600);
                        Assert.AreEqual(1, missCount);
                    }
                    finally
                    {
                        channel.Disconnect();
                    }
                }
            }
        }

        [TestMethod]
        public void HeatBeatDisconnectTests()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();
                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("frame.tcp://127.0.0.1:7777");
                    channel.SetHeartBeat(500);
                    channel.Connect();

                    var disconnect = false;
                    channel.OnDisconnect += (c) =>
                    {
                        disconnect = true;
                    };

                    var closed = false;
                    channel.OnClosed += (c, e) =>
                    {
                        closed = true;
                    };

                    try
                    {
                        var missCount = 0;
                        channel.OnMissHeartBeat += (c, num) =>
                        {
                            missCount++;
                            channel.Disconnect();
                        };

                        while (missCount < 1)
                        {
                            (manager as NetworkManager).Tick(1);
                            Thread.Sleep(1);
                        }

                        while (!disconnect)
                        {
                            (manager as NetworkManager).Tick(1);
                            Thread.Sleep(1);
                        }

                        while (!closed)
                        {
                            (manager as NetworkManager).Tick(1);
                            Thread.Sleep(1);
                        }

                        Assert.AreEqual(1, missCount);
                        Assert.AreEqual(true, disconnect);
                        Assert.AreEqual(true, closed);
                    }
                    finally
                    {
                        channel.Disconnect();
                    }
                }
            }
        }

        [TestMethod]
        public void SendNullDataTest()
        {
            var app = MakeEnv();
            var manager = app.Make<INetworkManager>();
            var channel = manager.Make("frame.tcp://127.0.0.1:7777");
            var wait = channel.Send(new byte[] { });

            Assert.AreEqual(true, wait.IsDone);
            Assert.AreEqual(true, wait.Result);
        }

        [TestMethod]
        public void TestRelease()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();
                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("frame.tcp://127.0.0.1:7777", "test");
                    channel.Connect();

                    var disconnect = false;
                    channel.OnDisconnect += (c) =>
                    {
                        disconnect = true;
                    };

                    var closed = false;
                    channel.OnClosed += (c, e) =>
                    {
                        closed = true;
                    };

                    manager.Release("test");

                    Assert.AreEqual(false, channel.Socket.Connected);
                    Assert.AreEqual(true, disconnect);
                    Assert.AreEqual(true, closed);

                    var socket = app.Make<ISocketManager>().Make("tcp://127.0.0.1:7777", "test");
                    Assert.AreNotEqual(socket, channel.Socket);
                }
            }
        }

        /// <summary>
        /// 测试全局事件
        /// </summary>
        [TestMethod]
        public void TestGlobalEvent()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();
                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("frame.tcp://127.0.0.1:7777");
                    var backMessage = string.Empty;
                    manager.OnMessage += (c, d) =>
                    {
                        backMessage = Encoding.Default.GetString((byte[])d);
                    };

                    try
                    {
                        var wait = channel.Connect();
                        var count = 0;
                        while (!wait.IsDone && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }

                        channel.Send(Encoding.Default.GetBytes("helloworld"));

                        count = 0;
                        while (backMessage == string.Empty && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }

                        Assert.AreEqual("helloworld", backMessage);
                        Assert.AreEqual("helloworld", Encoding.Default.GetString(Arr.Slice(fromClient, 4)));
                    }
                    finally
                    {
                        channel.Disconnect();
                    }
                }
            }
        }

        /// <summary>
        /// 测试由异常引发的断开链接
        /// </summary>
        [TestMethod]
        public void TestExceptionDisconnect()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();
                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("frame.tcp://127.0.0.1:7777");
                    Exception err = null;
                    manager.OnClosed += (c, e) =>
                    {
                        err = e;
                    };

                    try
                    {
                        var wait = channel.Connect();
                        var count = 0;
                        while (!wait.IsDone && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }
                        channel.Disconnect(new RuntimeException("test exception"));

                        Assert.AreEqual("CatLib.RuntimeException: test exception", err.ToString());
                    }
                    finally
                    {
                        channel.Disconnect();
                    }
                }
            }
        }

        [TestMethod]
        public void TestAllEventRegister()
        {
            lock (syncRoot)
            {
                var app = MakeEnv();
                var manager = app.Make<INetworkManager>();
                var fromClient = new byte[] { };
                using (var server = new TcpServer((data) =>
                {
                    fromClient = Arr.Merge(fromClient, data);
                }))
                {
                    var channel = manager.Make("frame.tcp://127.0.0.1:7777");

                    Exception err = null;
                    manager.OnError += (c, e) =>
                    {
                        err = e;
                    };

                    Exception closeErr = new Exception();
                    manager.OnClosed += (c, e) =>
                    {
                        closeErr = e;
                    };

                    INetworkChannel connected = null;
                    manager.OnConnected += (c) =>
                    {
                        connected = c;
                    };

                    INetworkChannel disconnected = null;
                    manager.OnDisconnect += (c) =>
                    {
                        disconnected = c;
                    };

                    INetworkChannel sent = null;
                    manager.OnSent += (c, b) =>
                    {
                        sent = c;
                    };

                    INetworkChannel released = null;
                    manager.OnReleased += (c) =>
                    {
                        released = c;
                    };

                    try
                    {
                        var wait = channel.Connect();
                        var count = 0;
                        while (!wait.IsDone && count++ < 3000)
                        {
                            Thread.Sleep(1);
                        }

                        channel.Send(Encoding.Default.GetBytes("helloworld"));
                    }
                    finally
                    {
                        manager.Release();
                    }

                    Assert.AreEqual(null, err);
                    Assert.AreEqual(null, closeErr);
                    Assert.AreEqual(channel, connected);
                    Assert.AreEqual(channel, disconnected);
                    Assert.AreEqual(channel, sent);
                    Assert.AreEqual(channel, released);
                }
            }
        }
    }
}
