using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using CatLib.Contracts.Base;

namespace CatLib.Network
{

    public class CTcpConnector : IDisposable , IDestroy
    {

        public enum Status
        {
            INITIAL    = 1,
            CONNECTING = 2,
            ESTABLISH  = 3,
            CLOSED     = 4,            
        }

        protected TcpClient socket;

        protected string remoteAddress;
        
        protected int remotePort;

        protected volatile Status status = Status.INITIAL;

        protected NetworkStream networkStream;

        public CTcpConnector(string host, int port)
        {
            remoteAddress = host;
            remotePort    = port;
        }

        public void Connect()
        {
            status = Status.CONNECTING;
            Dns.BeginGetHostAddresses(remoteAddress, OnDnsGetHostAddressesComplete, null);
        }

        public void Write(byte[] bytes)
        {
            networkStream.BeginWrite(bytes, 0, bytes.Length, OnWriteCallBack, socket);
        }

        public void Dispose()
        {
            if (networkStream != null)
            {
                networkStream.Close();
            }
            if (socket != null)
            {
                socket.Close();
            }
        }

        public void OnDestroy(){

            Dispose();

        }

        protected void OnDnsGetHostAddressesComplete(IAsyncResult result)
        {
            try
            {
                var ipAddress = Dns.EndGetHostAddresses(result);
                socket = new TcpClient();
                socket.BeginConnect(ipAddress, remotePort, OnConnectComplete, socket);
            }
            catch(Exception ex){

                status = Status.CLOSED;

            }
        }

        protected void OnConnectComplete(IAsyncResult result)
        {
            try
            {

                socket.EndConnect(result);

                networkStream = socket.GetStream();
                readBuffer = new byte[socket.ReceiveBufferSize];
                networkStream.BeginRead(readBuffer, 0, readBuffer.Length, OnReadCallBack, readBuffer);

                status = Status.ESTABLISH;
                
            }
            catch(Exception ex){

                status = Status.CLOSED;

            }

        }

        protected void OnReadCallBack(IAsyncResult result)
        {
          
            int read = networkStream.EndRead(result);
            if (read <= 0)
            {
                status = Status.CLOSED;
                return;
            }

            lock (readQueueLocker)
            {
                readQueue.Enqueue(readBuffer);
                hasData = true;
            }

            readBuffer = new byte[socket.ReceiveBufferSize];
            networkStream.BeginRead(readBuffer, 0, readBuffer.Length, OnReadCallBack, readBuffer);

        }

        protected void OnWriteCallBack(IAsyncResult result)
        {

            try{
                
                networkStream.EndWrite(result);

            }catch(Exception ex){

               status = Status.CLOSED;

            }
            
        }



        //以下是废弃代码




        private volatile bool hasData;
        public bool HasData{ get{ return hasData; } }

        private readonly object statuLocker = new object();
        
        

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

        

        

        

        

    }

}