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
    /// <summary>
    /// 频道测试
    /// </summary>
    [TestClass]
    public class NetworkChannelTests
    {
        public class PackerError : IPacker
        {
            [Flags]
            public enum ErrorType
            {
                Input = 1 << 0,
                Encode = 1 << 1,
                Decode = 1 << 2,
            }

            private readonly ErrorType errorType;

            public PackerError(ErrorType errorType)
            {
                this.errorType = errorType;
            }

            /// <summary>
            /// <para>检查包的完整性</para>
            /// <para>如果能够得到包长，则返回包的在buffer中的长度(包含包头)，否则返回0继续等待数据</para>
            /// <para>如果协议有问题，则填入ex参数，当前连接会因此断开</para>
            /// </summary>
            /// <param name="source"></param>
            /// <param name="ex"></param>
            /// <returns></returns>
            public int Input(byte[] source, out Exception ex)
            {
                ex = (errorType & ErrorType.Input) > 0 ? new RuntimeException("exception input") : null;

                if(source.Length >= 4)
                {
                    return BitConverter.ToInt32(source, 0);
                }

                return 0;
            }

            /// <summary>
            /// 序列化消息包。
            /// <para>如果协议有问题，则填入ex参数，会直接触发异常</para>
            /// </summary>
            /// <param name="packet">要序列化的消息包。</param>
            /// <param name="ex">用户自定义异常</param>
            /// <returns>序列化后的消息包字节流。如果需要抛弃数据包则返回null</returns>
            public byte[] Encode(object packet, out Exception ex)
            {
                ex = (errorType & ErrorType.Encode) > 0 ? new RuntimeException("exception encode") : null;

                var data = packet as byte[];
                if (data != null && data.Length > 0)
                {
                    data = Arr.Merge(BitConverter.GetBytes(data.Length + 4), data);
                }
                return data.Length <= 0 ? null : data;
            }

            /// <summary>
            /// 反序列化消息包(包体)。
            /// <para>如果协议有问题，则填入ex参数，当前连接会因此断开</para>
            /// </summary>
            /// <param name="source">反序列化的数据。</param>
            /// <param name="ex">用户自定义错误数据。</param>
            /// <returns>反序列化后的消息包。如果需要抛弃数据包则返回null</returns>
            public object Decode(byte[] source, out Exception ex)
            {
                ex = (errorType & ErrorType.Decode) > 0 ? new RuntimeException("exception decode") : null;
                return Arr.Slice(source, 4);
            }
        }

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
        public void TestChannelPackerErrorDecode()
        {
            var fromClient = new byte[] { };
            using (var server = new TcpServer((data) =>
            {
                fromClient = Arr.Merge(fromClient, data);
            }))
            {
                var socket = MakeEnv().Make<ISocketManager>().Make("tcp://127.0.0.1", "TestChannelPackerErrorDecode");
                var channel = new NetworkChannel(socket, new PackerError(PackerError.ErrorType.Decode), "TestChannelPackerErrorDecode");

                var wait = channel.Connect("127.0.0.1", 7777);
                var count = 0;
                while (!wait.IsDone && count++ < 3000)
                {
                    Thread.Sleep(1);
                }
                try
                {
                    string err = string.Empty;
                    channel.OnError += (c, ex) =>
                    {
                        if (err == string.Empty)
                        {
                            err = ex.ToString();
                        }
                    };

                    channel.Send(Encoding.Default.GetBytes("helloworld"));
                    count = 0;
                    while (channel.Socket.Connected && count++ < 3000)
                    {
                        Thread.Sleep(1);
                    }

                    Assert.AreEqual(false, channel.Socket.Connected);
                    Assert.AreEqual("CatLib.RuntimeException: exception decode", err);
                }
                finally
                {
                    channel.Disconnect();
                }
            }
        }

        [TestMethod]
        public void TestChannelPackerErrorEncode()
        {
            var fromClient = new byte[] { };
            using (var server = new TcpServer((data) =>
            {
                fromClient = Arr.Merge(fromClient, data);
            }))
            {
                var socket = MakeEnv().Make<ISocketManager>().Make("tcp://127.0.0.1", "TestChannelPackerErrorEncode");
                var channel = new NetworkChannel(socket, new PackerError(PackerError.ErrorType.Encode), "TestChannelPackerErrorEncode");

                var wait = channel.Connect("127.0.0.1", 7777);
                var count = 0;
                while (!wait.IsDone && count++ < 3000)
                {
                    Thread.Sleep(1);
                }
                try
                {
                    var isThrow = false;
                    try
                    {
                        channel.Send(Encoding.Default.GetBytes("helloworld"));
                    }
                    catch (RuntimeException)
                    {
                        isThrow = true;
                    }
                    Assert.AreEqual(true, isThrow);
                }
                finally
                {
                    channel.Disconnect();
                }
            }
        }

        [TestMethod]
        public void TestChannelPackerErrorInput()
        {
            var fromClient = new byte[] { };
            using (var server = new TcpServer((data) =>
            {
                fromClient = Arr.Merge(fromClient, data);
            }))
            {
                var socket = MakeEnv().Make<ISocketManager>().Make("tcp://127.0.0.1", "TestChannelPackerErrorDecode");
                var channel = new NetworkChannel(socket, new PackerError(PackerError.ErrorType.Input), "TestChannelPackerErrorDecode");

                var wait = channel.Connect("127.0.0.1", 7777);
                var count = 0;
                while (!wait.IsDone && count++ < 3000)
                {
                    Thread.Sleep(1);
                }
                try
                {
                    var err = new Exception();
                    channel.OnError += (c, ex) =>
                    {
                        err = ex;
                    };

                    channel.Send(Encoding.Default.GetBytes("helloworld"));
                    count = 0;
                    while (channel.Socket.Connected && count++ < 3000)
                    {
                        Thread.Sleep(1);
                    }

                    Assert.AreEqual(false, channel.Socket.Connected);
                    Assert.AreEqual("CatLib.RuntimeException: exception input", err.ToString());
                }
                finally
                {
                    channel.Disconnect();
                }
            }
        }
    }
}
