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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CatLib.Socket.Tests
{
    /// <summary>
    /// Tcp测试用的服务器
    /// </summary>
    internal class TcpTestsServer : IDisposable
    {
        class ClientThread
        {
            private readonly System.Net.Sockets.Socket client;

            private readonly Action<byte[]> callback;

            public ClientThread(System.Net.Sockets.Socket c, Action<byte[]> callback)
            {
                client = c;
                this.callback = callback;
            }

            public void ClientService()
            {
                var bytes = new byte[4096];
                try
                {
                    var i = 0;
                    while ((i = client.Receive(bytes)) != 0)
                    {
                        if (i < 0)
                        {
                            break;
                        }
                        var callbackBuff = new byte[i];
                        Buffer.BlockCopy(bytes, 0, callbackBuff, 0, i);
                        client.Send(callbackBuff);
                        callback.Invoke(callbackBuff);
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.ToString());
                }
                client.Close();
            }
        }

        private readonly Action<byte[]> callback;

        private readonly System.Net.Sockets.Socket server;

        /// <summary>
        /// Tcp测试用的服务器
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="port"></param>
        public TcpTestsServer(Action<byte[]> callback, int port = 7777)
        {
            server = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var iep = new IPEndPoint(IPAddress.Any, port);
            server.Bind(iep);
            server.Listen(20);
            this.callback = callback;
            var tcpThread = new Thread(TcpListen);
            tcpThread.Start();
        }

        /// <summary>
        /// 退出标记
        /// </summary>
        private bool exitFlag;

        public void Dispose()
        {
            exitFlag = true;
            server.Close();
        }

        private void TcpListen()
        {
            while (!exitFlag)
            {
                try
                {
                    var client = server.Accept();
                    var newClient = new ClientThread(client, callback);
                    var newThread = new Thread(newClient.ClientService);
                    newThread.Start();
                }
                catch { }
            }
        }
    }
}
