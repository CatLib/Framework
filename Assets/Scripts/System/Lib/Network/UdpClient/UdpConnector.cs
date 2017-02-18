using System;
using System.Net;
using System.Net.Sockets;

namespace CatLib.Network
{

    public class UdpConnector : IDisposable
    {

        public enum Status
        {
            INITIAL = 1,
            CONNECTING = 2,
            ESTABLISH = 3,
            CLOSED = 4,
        }

        protected volatile Status status = Status.INITIAL;
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
            if (status != Status.INITIAL && status != Status.CLOSED) { return; }

            status = Status.CONNECTING;

            socket = new UdpClient();

            status = Status.ESTABLISH;

            OnConnect(this, EventArgs.Empty);

            socket.BeginReceive(OnReadCallBack, null);

        }

        public void Connect(string host , int port)
        {
            if (status != Status.INITIAL && status != Status.CLOSED) { return; }
            remoteAddress = host;
            remotePort = port;
            status = Status.CONNECTING;
            Dns.BeginGetHostAddresses(host, OnDnsGetHostAddressesComplete, null);
        }

        public void SendTo(byte[] bytes , string host , int port)
        {
            if (status != Status.ESTABLISH) { return; }
            socket.BeginSend(bytes, bytes.Length , host, port, OnSendCallBack, null);
        }

        public void Send(byte[] bytes)
        {
            if (status != Status.ESTABLISH) { return; }
            socket.BeginSend(bytes, bytes.Length, OnSendCallBack , null);
        }

        public void Dispose()
        {
            if (status == Status.CLOSED) { return; }
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
                if (ipAddress != null && ipAddress.Length > 0)
                {
                    socket = new UdpClient();
                    socket.Connect(new IPEndPoint(ipAddress[0], remotePort));

                    status = Status.ESTABLISH;
                    OnConnect(this, EventArgs.Empty);

                    socket.BeginReceive(OnReadCallBack, null);

                }
                else
                {
                    throw new Exception("can not connect to :" + remoteAddress);
                }
            }
            catch (Exception ex)
            {

                OnError(this, new ErrorEventArgs(ex));
                Dispose();

            }
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