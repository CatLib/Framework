using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.Contracts.Network
{
    /// <summary>
    /// 连接器
    /// </summary>
    public interface IConnectorSocket : IConnector
    {

        /// <summary>
        /// 设定连接地址
        /// </summary>
        /// <param name="ip"></param>
        IConnectorSocket SetHost(string ip);

        /// <summary>
        /// 设定端口
        /// </summary>
        /// <param name="port"></param>
        IConnectorSocket SetPort(int port);

        /// <summary>
        /// 发送内容
        /// </summary>
        /// <param name="data"></param>
        void Send(byte[] data);

    }
}
