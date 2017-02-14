using CatLib.Base;
using CatLib.Contracts.Network;
using System.Collections;
using UnityEngine;
using CatLib.Container;
using System.Collections.Generic;
using CatLib.Contracts.NetPackage;

namespace CatLib.Network
{

    public class CTcpRequest : CComponent, IConnectorTcp
    {

        [CDependency]
        public IUnpacking Unpacker { get; set; }

        private Queue<byte[]> queue = new Queue<byte[]>();

        private string host;
        private int port;

        private CTcpClient tcpClient;

        private bool isDisconnect = false;
        private bool isGiveupConnect = false;
        private int reconnNum = 3;
        private int currentReconnNum = 0;

        public IConnectorSocket SetHost(string host)
        {
            this.host = host;
            return this;
        }

        public IConnectorSocket SetPort(int port)
        {
            this.port = port;
            return this;
        }

        public void Reset()
        {
            isGiveupConnect = false;
        }

        public void Send(byte[] bytes)
        {
            queue.Enqueue(bytes);
        }

        public IEnumerator StartServer()
        {
            
            tcpClient = new CTcpClient(host, port);
            tcpClient.Connect();
            while (true)
            {
                yield return SendModel();
                yield return ReadModel();
                yield return new WaitForEndOfFrame();
            } 
        }

        private IEnumerator ReadModel()
        {
            if (tcpClient.HasData)
            {
                IPackage[] packages;
                foreach (byte[] bytes in tcpClient.ReadAllData)
                {
                    if (Unpacker.Append(bytes, out packages))
                    {
                        foreach(IPackage package in packages)
                        {
                            var args = new CTcpEventArgs(package);
                            FDispatcher.Instance.Event.Trigger(TypeGuid, this, args);
                            FDispatcher.Instance.Event.Trigger(GetType().ToString(), this, args);
                            FDispatcher.Instance.Event.Trigger(typeof(IConnectorTcp).ToString(), this, args);
                        }
                    }
                }

            }
            yield return new WaitForEndOfFrame();
        }

        private IEnumerator SendModel()
        {
            if (tcpClient.IsError && !isGiveupConnect)
            {
                do
                {
                    yield return OnErrorReconn();
                } while (tcpClient.IsError && !isGiveupConnect);
            }

            while (queue.Count > 0)
            {
                if (tcpClient.IsConnect)
                {
                    tcpClient.Write(queue.Dequeue());
                }
            }
        }

        private IEnumerator OnErrorReconn()
        {
            currentReconnNum++;
            if (tcpClient != null) { tcpClient.Dispose(); }
            Unpacker.Clear();
            queue.Clear();
            tcpClient = new CTcpClient(host, port);
            tcpClient.Connect();

            while (!tcpClient.IsError && !tcpClient.IsConnect)
            {
                yield return new WaitForEndOfFrame();
            }

            if (tcpClient.IsError)
            {
                if (currentReconnNum > reconnNum)
                {
                    isGiveupConnect = true;
                    var args = new CTcpErrorEventArgs();
                    FDispatcher.Instance.Event.Trigger(TypeGuid, this, args);
                    FDispatcher.Instance.Event.Trigger(GetType().ToString(), this, args);
                    FDispatcher.Instance.Event.Trigger(typeof(IConnectorTcp).ToString(), this, args);
                }
            }
        }

        public void Disconnect()
        {
            isDisconnect = true;
        }

    }

}