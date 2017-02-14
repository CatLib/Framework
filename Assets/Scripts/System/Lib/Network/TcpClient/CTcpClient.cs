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

        private volatile bool hasData;
        public bool HasData{ get{ return hasData; } }

        private readonly object statuLocker = new object();
        
        private NetworkStream networkStream;

        private Queue<byte[]> readQueue = new Queue<byte[]>();
        private readonly object readQueueLocker = new object();

        private Queue<byte[]> writeQueue = new Queue<byte[]>();
        private readonly object writeQueueLocker = new object();

        private byte[] readBuffer;

        public byte[][] ReadAllData
        {
            get
            {
                lock (readQueueLocker)
                {
                    var bytes = readQueue.ToArray();
                    readQueue.Clear();
                    hasData = false;
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
            catch(Exception ex){

                OnException(ex);

            }
        }

        private void OnConnectComplete(IAsyncResult result)
        {
            try
            {
                tcpClient.EndConnect(result);

                networkStream = tcpClient.GetStream();
                readBuffer = new byte[tcpClient.ReceiveBufferSize];
                networkStream.BeginRead(readBuffer, 0, readBuffer.Length, OnReadCallBack, readBuffer);

                lock(statuLocker){
                    
                    isConnect = true;
                
                }
            }
            catch(Exception ex){

                OnException(ex);

            }

        }

        private void OnReadCallBack(IAsyncResult result)
        {
          
            int read = networkStream.EndRead(result);
            if (read <= 0)
            {
                lock(statuLocker){
                    
                    isConnect = false;
                    isError = true;
                    
                }
                return;
            }

            lock (readQueueLocker)
            {
                readQueue.Enqueue(readBuffer);
                hasData = true;
            }

            readBuffer = new byte[tcpClient.ReceiveBufferSize];
            networkStream.BeginRead(readBuffer, 0, readBuffer.Length, OnReadCallBack, readBuffer);

        }

        private void OnWriteCallBack(IAsyncResult result)
        {

            byte[] writeBytes;

            try{
                
                networkStream.EndWrite(result);

                lock (writeQueueLocker)
                {
                    if (writeQueue.Count > 0)
                    {
                        writeBytes = writeQueue.Dequeue();
                        networkStream.BeginWrite(writeBytes, 0, writeBytes.Length, OnWriteCallBack, tcpClient);
                    }
                }

            }catch(Exception ex){

                OnException(ex);

            }
            
        }

        private void OnException(Exception ex){

            lock(statuLocker){

                error = ex.Message;
                isConnect = false;
                isError = true;

            }

        }
        
    }

}