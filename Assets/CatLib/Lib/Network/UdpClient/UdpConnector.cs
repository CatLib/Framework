using System;
using System.Net;
using System.Net.Sockets;
using CatLib.API;

namespace CatLib.Network
{

    public class UdpConnector : IDisposable
    {

        public enum Status
        {
            Initial    = 1,
            Connecting = 2,
            Establish = 3,
            Closed     = 4, 
        }

        protected volatile Status status = Status.Initial;
        public Status CurrentStatus { get { return status; } }

        protected UdpClient socket;

        protected IPEndPoint listenEndPoint;

        protected string remoteAddress;
        protected int remotePort;
      
        public EventHandler OnConnect;
        public EventHandler OnClose;
        public EventHandler OnError;
        public EventHandler OnMessage;

        public UdpConnector()
        {

        }

        public void Connect()
        {
            if (status != Status.Initial && status != Status.Closed) { return; }

            status = Status.Connecting;

            socket = new UdpClient();

            status = Status.Establish;

            OnConnect(this, EventArgs.Empty);

            socket.BeginReceive(OnReadCallBack, null);

        }

        public void Connect(string host , int port)
        {
            if (status != Status.Initial && status != Status.Closed) { return; }
            remoteAddress = host;
            remotePort = port;
            status = Status.Connecting;

            socket = new UdpClient();
            socket.Connect(host , port);

            status = Status.Establish;
            OnConnect(this, EventArgs.Empty);

            socket.BeginReceive(OnReadCallBack, null);
        }

        public void SendTo(byte[] bytes , string host , int port)
        {
            if (status != Status.Establish) { return; }
            socket.BeginSend(bytes, bytes.Length , host, port, OnSendCallBack, null);
        }

        public void Send(byte[] bytes)
        {
            if (status != Status.Establish) { return; }
            socket.BeginSend(bytes, bytes.Length, OnSendCallBack , null);
        }

        public void Dispose()
        {
            if (status == Status.Closed) { return; }
            if (socket != null)
            {
                socket.Close();
            }
            status = Status.Closed;
            OnClose(this, EventArgs.Empty);
        }

        protected void OnReadCallBack(IAsyncResult result)
        {
            try
            {
                byte[] receiveBytes = socket.EndReceive(result, ref listenEndPoint);

                var args = new SocketResponseEventArgs(receiveBytes);
                OnMessage(this, args);

                socket.BeginReceive(OnReadCallBack, null);

            }catch(Exception ex)
            {
                OnError(this, new ErrorEventArgs(ex));
                Dispose();
            }

        }

        protected void OnSendCallBack(IAsyncResult result)
        {

            try
            {
                socket.EndSend(result);
            }
            catch (Exception ex)
            {

                OnError(this, new ErrorEventArgs(ex));
                Dispose();

            }

        }

        

    }

}