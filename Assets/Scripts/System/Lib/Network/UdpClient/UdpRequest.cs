using CatLib.Contracts.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace CatLib.Network
{

    public class UdpRequest : Component, IConnectorUdp
    {

        public string Name { get; set; }

        private Queue<object[]> queue = new Queue<object[]>();

        private string host;
        private int port;

        private UdpConnector udpConnector;

        private string listenHost;
        private int listenPort;

        private IPacking packer;
        private IProtocol protocol;

        private bool stopMark = false;

        public void SetConfig(Hashtable config)
        {
            if (packer == null && config.ContainsKey("packing"))
            {
                packer = App.Make(config["packing"].ToString()) as IPacking;
            }

            if (protocol == null && config.ContainsKey("protocol"))
            {
                protocol = App.Make(config["protocol"].ToString()) as IProtocol;
            }

            if (config.ContainsKey("host"))
            {
                host = config["host"].ToString();
            }

            if (config.ContainsKey("port"))
            {
                port = Convert.ToInt32(config["port"].ToString());
            }
        }

        /// <summary>
        /// 链接
        /// </summary>
        public void Connect()
        {

            Disconnect();
            if (packer != null) { packer.Clear(); }

            udpConnector = new UdpConnector();
            udpConnector.OnConnect += OnConnect;
            udpConnector.OnClose += OnClose;
            udpConnector.OnError += OnError;
            udpConnector.OnMessage += OnMessage;

            if (string.IsNullOrEmpty(host) || port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                udpConnector.Connect();
            }
            else
            {
                udpConnector.Connect(host, port);
            }

        }

        /// <summary>
        /// 加入发送队列
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(IPackage package)
        {
            byte[] data;
            if (protocol != null)
            {
                data = protocol.Encode(package);
            }
            else
            {
                data = package.ToByte();
            }
            Send(data);
        }

        /// <summary>
        /// 加入发送队列
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes)
        {
            if (packer != null)
            {
                bytes = packer.Encode(bytes);
            }
            queue.Enqueue(new object[] { bytes });
        }

        /// <summary>
        /// 发送到目标
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Send(IPackage package, string host, int port)
        {
            byte[] data;
            if (protocol != null)
            {
                data = protocol.Encode(package);
            }
            else
            {
                data = package.ToByte();
            }
            Send(data , host , port);
        }

        /// <summary>
        /// 发送到目标
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Send(byte[] bytes , string host , int port)
        {
            if (packer != null)
            {
                bytes = packer.Encode(bytes);
            }
            queue.Enqueue(new object[] { bytes, host, port });
        }

        public IEnumerator StartServer()
        {
            while (true)
            {
                if (stopMark) { break; }
                while (queue.Count > 0)
                {
                    if (udpConnector != null && udpConnector.CurrentStatus == UdpConnector.Status.ESTABLISH)
                    {
                        object[] data = queue.Dequeue();
                        if(data.Length == 1)
                        {
                            udpConnector.Send(data[0] as byte[]);
                        }else if(data.Length == 3)
                        {
                            udpConnector.SendTo(data[0] as byte[], data[1] as string, int.Parse(data[2].ToString()));
                        }
                        
                    }
                    else { break; }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Destroy()
        {
            stopMark = true;
            Disconnect();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (udpConnector != null)
            {
                udpConnector.Dispose();
            }
            udpConnector = null;
        }

        /// <summary>
        /// 当链接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnConnect(object sender, EventArgs args)
        {
            Trigger(SocketRequestEvents.ON_CONNECT, args);
        }

        /// <summary>
        /// 当关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnClose(object sender, EventArgs args)
        {
            Trigger(SocketRequestEvents.ON_CLOSE, args);
        }

        /// <summary>
        /// 当错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnError(object sender, EventArgs args)
        {
            Trigger(SocketRequestEvents.ON_ERROR, args);
        }

        /// <summary>
        /// 当接受到消息时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMessage(object sender, EventArgs args)
        {

            if (packer == null)
            {
                Trigger(SocketRequestEvents.ON_MESSAGE, args);
                return;
            }

            try
            {
                byte[][] data = packer.Decode((args as SocketResponseEventArgs).Response);
                if (data != null)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        try
                        {

                            if (protocol == null)
                            {
                                args = new SocketResponseEventArgs(data[i]);
                            }
                            else
                            {
                                args = new PackageResponseEventArgs(protocol.Decode(data[i]));
                            }
                            Trigger(SocketRequestEvents.ON_MESSAGE, args);

                        }
                        catch (Exception ex)
                        {
                            Trigger(SocketRequestEvents.ON_ERROR, new ErrorEventArgs(ex));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trigger(SocketRequestEvents.ON_ERROR, new ErrorEventArgs(ex));
                Disconnect();
            }

        }


        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名字</param>
        /// <param name="args">参数</param>
        private void Trigger(string eventName, EventArgs args)
        {
            App.Trigger(eventName, this, args);
            App.Trigger(eventName + TypeGuid, this, args);
            App.Trigger(eventName + GetType().ToString(), this, args);
            App.Trigger(eventName + typeof(IConnectorUdp).ToString(), this, args);
            App.Trigger(eventName + typeof(IConnectorSocket).ToString(), this, args);
        }

    }
}