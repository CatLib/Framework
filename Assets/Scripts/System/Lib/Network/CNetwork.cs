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

        [CDependency]
        public CConfig Config { get; set; }

        private const string CONNECTOR_CONFIG_HEADER = "connector.";

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<string, IConnector> connector = new Dictionary<string, IConnector>();

        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="aisle">通道</param>
        /// <param name="service">服务名</param>
        public T Create<T>(string aisle , string service) where T : IConnector
        {
            if (this.connector.ContainsKey(aisle)) { return (T)this.connector[aisle]; }
            IConnector connector = (T)App.Make(service);
            this.connector.Add(aisle, connector);
            InitConnector(connector, aisle);
            App.StartCoroutine(connector.StartServer());
            return (T)connector;
        }

        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="aisle">通道</param>
        public T Create<T>(string aisle) where T : IConnector
        {
            if (this.connector.ContainsKey(aisle)) { return (T)this.connector[aisle]; }
            IConnector connector = App.Make<T>();
            this.connector.Add(aisle, connector);
            InitConnector(connector, aisle);
            App.StartCoroutine(connector.StartServer());
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

        private void InitConnector(IConnector connector, string aisle)
        {

            if (Config.IsExists(CONNECTOR_CONFIG_HEADER + aisle))
            {
                string config;
                if (connector is IConnectorSocket)
                {
                    config = Config.Get<string>(CONNECTOR_CONFIG_HEADER + aisle);
                    var hostPort = config.Split(':');
                    (connector as IConnectorTcp).SetHost(hostPort[0]).SetPort(int.Parse(hostPort[1]));
                }
                else if (connector is IConnectorHttp)
                {
                    (connector as IConnectorHttp).SetUrl(Config.Get<string>(CONNECTOR_CONFIG_HEADER + aisle));
                }
            }
           
        }

    }

}
