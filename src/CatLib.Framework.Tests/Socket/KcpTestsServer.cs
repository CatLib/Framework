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
    public class KcpTestsServer
    {
        private readonly System.Net.Sockets.Socket server;

        private readonly Action<byte[]> callback;

        private bool exitFlag;

        private KCP kcp;

        /// <summary>
        /// 开关队列
        /// </summary>
        private readonly SwitchQueue<byte[]> receiveQueue = new SwitchQueue<byte[]>(128);

        private EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

        /// <summary>
        /// 下一次刷新时间
        /// </summary>
        private uint nextUpdateTime;

        /// <summary>
        /// 时钟
        /// </summary>
        /// <returns>时钟</returns>
        private static uint Clock()
        {
            return (uint)(Convert.ToInt64(DateTime.UtcNow.Subtract(SystemTime.UtcTime).TotalMilliseconds) & 0xffffffff);
        }


        public KcpTestsServer(uint conv, Action<byte[]> callback, int port = 7777)
        {
            server = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            InitKcp(conv);
            var iep = new IPEndPoint(IPAddress.Any, port);
            server.Bind(iep);

            this.callback = callback;
            var tcpThread = new Thread(KcpListen);
            tcpThread.Start();
            var tickThread = new Thread(TickThread);
            tickThread.Start();
        }

        /// <summary>
        /// 初始化Kcp
        /// </summary>
        /// <param name="conv">会话</param>
        private void InitKcp(uint conv)
        {
            kcp = new KCP(conv, (buf, size) =>
            {
                server.SendTo(buf, 0, size, SocketFlags.None, remote);
            });

            kcp.NoDelay(1, 10, 2, 1);
            kcp.WndSize(128, 128);
        }

        public void KcpListen()
        {
            while (!exitFlag)
            {
                var data = new byte[1024];
                var recv = server.ReceiveFrom(data, ref remote);

                var callbackBuff = new byte[recv];
                Buffer.BlockCopy(data, 0, callbackBuff, 0, recv);
                receiveQueue.Push(callbackBuff);
            }
        }

        public void Update(uint current)
        {
            if (kcp == null)
            {
                return;
            }

            if (!ProcessReceiveQueue() && current < nextUpdateTime)
            {
                return;
            }

            kcp.Update(current);
            nextUpdateTime = kcp.Check(current);
        }

        /// <summary>
        /// 执行接收队列
        /// </summary>
        /// <returns>是否需要更新</returns>
        private bool ProcessReceiveQueue()
        {
            receiveQueue.Switch();
            var needUpdate = false;
            while (!receiveQueue.Empty())
            {
                kcp.Input(receiveQueue.Pop());
                needUpdate = true;

                for (var size = kcp.PeekSize(); size > 0; size = kcp.PeekSize())
                {
                    var buffer = new byte[size];
                    if (kcp.Recv(buffer) > 0)
                    {
                        callback.Invoke(buffer);
                        kcp.Send(buffer);
                    }
                }
            }
            return needUpdate;
        }

        public void Dispose()
        {
            exitFlag = true;
            server.Close();
        }

        private void TickThread()
        {
            while (!exitFlag)
            {
                Update(Clock());
                Thread.Sleep(10);
            }
        }
    }
}
