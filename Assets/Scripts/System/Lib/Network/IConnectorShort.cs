using UnityEngine;
using System.Collections;

namespace CatLib.Network
{
    /// <summary>
    /// 连接器
    /// </summary>
    public interface IConnectorShort : IConnector
    {

        void Send(string action, byte[] bytes , System.Action<byte[]> callback);

    }
}
