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
        /// 发送内容
        /// </summary>
        /// <param name="data"></param>
        void Send(byte[] data);

    }
}
