using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.Contracts.Network
{

    /// <summary>
    /// 网络服务
    /// </summary>
    public interface INetwork
    {
        T Create<T>(string aisle) where T : IConnector;

        T Create<T>(string aisle, string service) where T : IConnector;

        void Disconnect(string aisle);

        T Get<T>(string aisle) where T : IConnector;
    }
}
