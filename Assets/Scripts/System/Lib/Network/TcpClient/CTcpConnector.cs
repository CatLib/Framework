using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using CatLib.Contracts.Base;
using CatLib.Base;

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

        private byte[] readBuffer;
        private Queue<byte[]> readQueue = new Queue<byte[]>();
        private readonly object readQueueLocker = new object();

        public EventHandler OnConnect;
        public EventHandler OnClose;
        public EventHandler OnError;
        public EventHandler OnMessage;

        public CTcpConnector(string host, int port)
        {
            remoteAddress = host;
            remotePort    = port;
        }

        public void Connect()
        {
            if (status != Status.INITIAL && status != Status.CLOSED) { return; }
            status = Status.CONNECTING;
            Dns.BeginGetHostAddresses(remoteAddress, OnDnsGetHostAddressesComplete, null);
        }

        public void Write(byte[] bytes)
        {
            if (status != Status.ESTABLISH) { return; }
            networkStream.BeginWrite(bytes, 0, bytes.Length, OnWriteCallBack, socket);
        }

        public void Dispose()
        {
            if (status == Status.CLOSED) { return; }
            if (networkStream != null)
            {
                networkStream.Close();
            }
            if (socket != null)
            {
                socket.Close();
            }
            status = Status.CLOSED;
            OnClose(this, EventArgs.Empty);
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

                OnError(this ,new CErrorEventArgs(ex));
                Dispose();

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
                OnConnect(this, EventArgs.Empty);

            }
            catch(Exception ex){

                OnError(this, new CErrorEventArgs(ex));
                Dispose();

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

            var args = new CSocketMessageEventArgs(readBuffer);
            OnMessage(this, args);

            readBuffer = new byte[socket.ReceiveBufferSize];
            networkStream.BeginRead(readBuffer, 0, readBuffer.Length, OnReadCallBack, readBuffer);

        }

        protected void OnWriteCallBack(IAsyncResult result)
        {

            try{
                
                networkStream.EndWrite(result);

            }catch(Exception ex){

                OnError(this, new CErrorEventArgs(ex));
                Dispose();

            }
            
        }

    }

}