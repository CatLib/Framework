using CatLib.Base;
using CatLib.Contracts.Network;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CatLib.Contracts.NetPackage;
using System;
using CatLib.Contracts.Base;

namespace CatLib.Network
{

    public class CTcpRequest : CComponent, IConnectorTcp
    {

        public string Name { get; set; }

        private Queue<byte[]> queue = new Queue<byte[]>();

        private string host;
        private int port;

        private CTcpConnector tcpConnector;
        private IPacking packer;

        private bool stopMark = false;
        
        /// <summary>
        /// 设定配置
        /// </summary>
        /// <param name="config"></param>
        public void SetConfig(Hashtable config){

            if(packer == null && config.ContainsKey("packing.alias")){
                packer = App.Make(config["packing.alias"].ToString()) as IPacking;
            }
            if(packer == null && config.ContainsKey("packing.type")){
                packer = App.Make(config["packing.type"].ToString()) as IPacking;
            }
            
            if(config.ContainsKey("host")){
                host = config["host"].ToString();
            }

            if(config.ContainsKey("port")){
                port = Convert.ToInt32(config["port"].ToString());
            }

        }

        /// <summary>
        /// 当连接时
        /// </summary>
        public void Connect()
        {

            if(string.IsNullOrEmpty(host)){

                OnError(this, new CErrorEventArgs(new CException(GetType().ToString() + ", Name:" + Name + " , host is invalid")));
                return;

            }

            if(port <= 0){

                OnError(this, new CErrorEventArgs(new CException(GetType().ToString() + ", Name:" + Name + " , port is invalid")));
                return;

            }

            Disconnect();
            if (packer != null){ packer.Clear(); }

            tcpConnector = new CTcpConnector(host, port);
            tcpConnector.OnConnect += OnConnect;
            tcpConnector.OnClose   += OnClose;
            tcpConnector.OnError   += OnError;
            tcpConnector.OnMessage += OnMessage;
            tcpConnector.Connect();

        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="package"></param>
        public void Send(IPackage package)
        {
            if(packer != null)
            {
                Send(packer.Encode(package));
            }else
            {
                Send(package.PackageByte);
            }
        }

        /// <summary>
        /// 加入发送队列
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes)
        {
            queue.Enqueue(bytes);
        }

        public IEnumerator StartServer()
        {
            while (true)
            {
                if (stopMark) { break; }
                while (queue.Count > 0)
                {
                    if (tcpConnector != null && tcpConnector.CurrentStatus == CTcpConnector.Status.ESTABLISH)
                    {
                        tcpConnector.Write(queue.Dequeue());
                    }
                    else { break; }
                }
                yield return new WaitForEndOfFrame();
            } 
        }

        /// <summary>
        /// 当链接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnConnect(object sender , EventArgs args)
        {
            Trigger(CTcpRequestEvents.ON_CONNECT, args);
        }

        /// <summary>
        /// 当关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnClose(object sender, EventArgs args)
        {
            Trigger(CTcpRequestEvents.ON_CLOSE, args);
        }

        /// <summary>
        /// 当错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnError(object sender, EventArgs args)
        {
            Trigger(CTcpRequestEvents.ON_ERROR, args);
        }

        /// <summary>
        /// 当接受到消息时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMessage(object sender , EventArgs args)
        {

            if(packer == null)
            {
                Trigger(CTcpRequestEvents.ON_MESSAGE, args);
                return;
            }

            IPackage[] package = packer.Decode((args as CSocketMessageEventArgs).Message);
            if(package != null)
            {
                for(int i = 0; i < package.Length; i++)
                {
                    args = new CPackageMessageEventArgs(package[i]);
                    Trigger(CTcpRequestEvents.ON_MESSAGE, args);
                }
            }

        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (tcpConnector != null)
            {
                tcpConnector.Dispose();
            }
            tcpConnector = null;
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
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名字</param>
        /// <param name="args">参数</param>
        private void Trigger(string eventName , EventArgs args)
        {
            App.Trigger(eventName, this, args);
            App.Trigger(eventName + TypeGuid, this, args);
            App.Trigger(eventName + GetType().ToString(), this, args);
            App.Trigger(eventName + typeof(IConnectorTcp).ToString(), this, args);
        }

    }

}