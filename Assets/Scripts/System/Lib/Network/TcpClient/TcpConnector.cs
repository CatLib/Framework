using System;
using System.Net;
using System.Net.Sockets;

namespace CatLib.Network
{

    public class TcpConnector : IDisposable 
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

        protected int readBufferLength = 1024;

        protected volatile Status status = Status.INITIAL;
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
            if (status != Status.INITIAL && status != Status.CLOSED) { return; }
            status = Status.CONNECTING;
            Dns.BeginGetHostAddresses(remoteAddress, OnDnsGetHostAddressesComplete, null);
        }

        public void Send(byte[] bytes)
        {
            if (status != Status.ESTABLISH) { return; }
            networkStream.BeginWrite(bytes, 0, bytes.Length, OnSendCallBack, socket);
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

                status = Status.ESTABLISH;
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
                status = Status.CLOSED;
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