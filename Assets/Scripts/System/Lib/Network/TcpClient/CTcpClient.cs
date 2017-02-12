using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace CatLib.Network
{

    public class CTcpClient : IDisposable
    {

        private TcpClient tcpClient;

        private string host;
        private int port;

        private volatile bool isConnect = false;
        public bool IsConnect { get { return isConnect; } }

        private volatile bool isError = false;
        public bool IsError { get { return isError; } }

        private volatile string error;
        public string Error { get { return error; } }

        private NetworkStream networkStream;

        private Queue<byte[]> readQueue = new Queue<byte[]>();
        private readonly object readQueueLocker = new object();

        private Queue<byte[]> writeQueue = new Queue<byte[]>();
        private readonly object writeQueueLocker = new object();

        public byte[][] UnwriteData
        {
            get
            {
                lock (writeQueueLocker)
                {
                    return writeQueue.ToArray();
                }
            }
        }

        public byte[][] ReadAll
        {
            get
            {
                lock (readQueueLocker)
                {
                    var bytes = readQueue.ToArray();
                    readQueue.Clear();
                    return bytes;
                }
            }
        }

        public CTcpClient(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Connect()
        {
            Dns.BeginGetHostAddresses(host, OnDnsGetHostAddressesComplete, null);
        }

        public void Write(byte[] bytes)
        {
            if (networkStream.CanWrite)
            {
                networkStream.BeginWrite(bytes, 0, bytes.Length, OnWriteCallBack, tcpClient);
            }else
            {
                lock (writeQueueLocker)
                {
                    writeQueue.Enqueue(bytes);
                }
            }
        }

        public void Dispose()
        {
            if (networkStream != null)
            {
                networkStream.Close();
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }

        private void OnDnsGetHostAddressesComplete(IAsyncResult result)
        {
            try
            {
                var ipAddress = Dns.EndGetHostAddresses(result);
                tcpClient = new TcpClient();
                tcpClient.BeginConnect(ipAddress, port, OnConnectComplete, tcpClient);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                isConnect = false;
                isError = true;
            }
        }

        private void OnConnectComplete(IAsyncResult result)
        {
            try
            {
                tcpClient.EndConnect(result);

                networkStream = tcpClient.GetStream();
                byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                networkStream.BeginRead(buffer, 0, buffer.Length, OnReadCallBack, buffer);

                isConnect = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                isConnect = false;
                isError = true;
            }

        }

        private void OnReadCallBack(IAsyncResult result)
        {
            int read = networkStream.EndRead(result);
            if (read <= 0)
            {
                isConnect = false;
                return;
            }

            byte[] buffer = result.AsyncState as byte[];

            Queue myCollection = new Queue();
            lock (readQueueLocker)
            {
                readQueue.Enqueue(buffer);
            }
            networkStream.BeginRead(buffer, 0, buffer.Length, OnReadCallBack, buffer);

        }

        private void OnWriteCallBack(IAsyncResult result)
        {
            //todo: 看下can write 什么时候发生变化
            Debug.Log("on end write left: " + networkStream.CanWrite);
            networkStream.EndWrite(result);
            Debug.Log("on end write right: " + networkStream.CanWrite);
            lock (writeQueueLocker)
            {
                if (writeQueue.Count > 0)
                {
                    byte[] writeBytes;
                    writeBytes = writeQueue.Dequeue();
                    networkStream.BeginWrite(writeBytes, 0, writeBytes.Length, OnWriteCallBack, tcpClient);
                }
            }
        }
    }

}