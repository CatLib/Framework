using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Base;
using CatLib.Contracts.Network;

namespace CatLib.Network
{
    
    /// <summary>
    /// 网络服务
    /// </summary>
    public class CNetwork : CComponent , IDestroy , INetwork
    {


        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<string, IConnector> connector = new Dictionary<string, IConnector>();

        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="aisle">通道</param>
        public T Create<T>(string aisle) where T : IConnector
        {
            if (this.connector.ContainsKey(aisle)) { return (T)this.connector[aisle]; }
            IConnector connector = Application.Make<T>();
            this.connector.Add(aisle, connector);
            Application.StartCoroutine(connector.StartServer());
            return (T)connector;

        }

        /// <summary>
        /// 断开一个网络链接
        /// </summary>
        /// <param name="aisle">通道</param>
        public void Disconnect(string aisle)
        {
            if (connector.ContainsKey(aisle))
            {
                connector[aisle].Disconnect();
            }
            connector.Remove(aisle);
        }

        /// <summary>
        /// 获取一个链接器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aisle">通道</param>
        /// <returns></returns>
        public T Get<T>(string aisle) where T : IConnector
        {
            if (connector.ContainsKey(aisle))
            {
                return (T)connector[aisle];
            }
            return default(T);
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            foreach(KeyValuePair<string , IConnector> connector in this.connector)
            {
                connector.Value.Disconnect();
            }
            this.connector.Clear();
        }

    }

}
