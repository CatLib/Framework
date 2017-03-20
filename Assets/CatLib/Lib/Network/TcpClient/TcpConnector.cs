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
using CatLib.API;

namespace CatLib.Network
{

    public class TcpConnector : IDisposable 
    {

        public enum Status
        {
            Initial    = 1,
            Connecting = 2,
            Establish = 3,
            Closed     = 4,            
        }

        protected TcpClient socket;

        protected string remoteAddress;
        protected int remotePort;

        protected int readBufferLength = 1024;

        protected volatile Status status = Status.Initial;
        public Status CurrentStatus{ get { return status; } }

        protected NetworkStream networkStream;

        private byte[] readBuffer;

        public EventHandler OnConnect;
        public EventHandler OnClose;
        public EventHandler OnError;
        public EventHandler OnMessage;

        public TcpConnector(string host, int port)
        {
            remoteAddress = host;
            remotePort    = port;
        }

        public void Connect()
        {
            if (status != Status.Initial && status != Status.Closed) { return; }
            status = Status.Connecting;
            Dns.BeginGetHostAddresses(remoteAddress, OnDnsGetHostAddressesComplete, null);
        }

        public void Send(byte[] bytes)
        {
            if (status != Status.Establish) { return; }
            networkStream.BeginWrite(bytes, 0, bytes.Length, OnSendCallBack, socket);
        }

        public void Dispose()
        {
            if (status == Status.Closed) { return; }
            if (networkStream != null)
            {
                networkStream.Close();
            }
            if (socket != null)
            {
                socket.Close();
            }
            status = Status.Closed;
            OnClose(this, EventArgs.Empty);
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

                OnError(this ,new ErrorEventArgs(ex));
                Dispose();

            }
        }

        protected void OnConnectComplete(IAsyncResult result)
        {
            try
            {

                socket.EndConnect(result);

                networkStream = socket.GetStream();
                readBuffer = new byte[readBufferLength];
                networkStream.BeginRead(readBuffer, 0, readBuffer.Length, OnReadCallBack, readBuffer);

                status = Status.Establish;
                OnConnect(this, EventArgs.Empty);

            }
            catch(Exception ex){

                OnError(this, new ErrorEventArgs(ex));
                Dispose();

            }

        }

        protected void OnReadCallBack(IAsyncResult result)
        {
          
            int read = networkStream.EndRead(result);
            if (read <= 0)
            {
                status = Status.Closed;
                return;
            }

            var callbackBuff = new byte[read];
            System.Buffer.BlockCopy(readBuffer, 0, callbackBuff, 0, read);
            var args = new SocketResponseEventArgs(callbackBuff);
            OnMessage(this, args);

            readBuffer = new byte[readBufferLength];
            networkStream.BeginRead(readBuffer, 0, readBuffer.Length, OnReadCallBack, readBuffer);

        }

        protected void OnSendCallBack(IAsyncResult result)
        {

            try{
                
                networkStream.EndWrite(result);

            }catch(Exception ex){
                
                OnError(this, new ErrorEventArgs(ex));
                Dispose();

            }
            
        }

    }

}