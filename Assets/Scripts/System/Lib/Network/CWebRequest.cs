using UnityEngine;
using System.Collections;

namespace CatLib.Network
{

    public class CWebRequest : IConnectorShort
    {
        /// <summary>
        /// 服务器地址
        /// </summary>

        private string host;

        /// <summary>
        /// 端口
        /// </summary>
        private ushort port;

        public CWebRequest(string host , ushort port)
        {
            this.host = host;
            this.port = port;
        }

        public void Send(string action , byte[] bytes , System.Action<byte[]> callback)
        {
            //string 
            //WWW www = new WWW(host);

        }

    }

}