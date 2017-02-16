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
        T Create<T>(string name) where T : IConnector;

        T Create<T>(string name, string service) where T : IConnector;

        void Destroy(string name);

        T Get<T>(string name) where T : IConnector;
    }
}
