using CatLib.Base;
using CatLib.Contracts.Network;
using System.Collections;
using UnityEngine;

namespace CatLib.Network
{

    public class CTcpRequest : CComponent, IConnectorTcp
    {

        private bool isDisconnect = false;

        public IConnectorSocket SetHost(string ip)
        {
            return this;
        }


        public IConnectorSocket SetPort(int port)
        {

            return this;
        }

        public void Send(byte[] bytes)
        {

        }

        public IEnumerator StartServer()
        {
            while (true)
            {
                if (isDisconnect) { break; }

                yield return new WaitForEndOfFrame();
            } 
        }

        public void Disconnect()
        {
            isDisconnect = true;
        }
    }

}