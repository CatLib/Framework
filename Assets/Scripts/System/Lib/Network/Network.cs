using System.Collections;
using System.Collections.Generic;
using CatLib.API;
using CatLib.API.Network;

namespace CatLib.Network
{
    
    /// <summary>
    /// 网络服务
    /// </summary>
    public class Network : Component , IDestroy , INetwork
    {

        [Dependency]
        public Configs Config { get; set; }

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<string, IConnector> connector = new Dictionary<string, IConnector>();

        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="name">通道</param>
        /// <param name="service">服务名</param>
        public T Create<T>(string name , string service) where T : IConnector
        {
            if (this.connector.ContainsKey(name)) { return (T)this.connector[name]; }
            IConnector connector = (T)App.Make(service);
            this.connector.Add(name, connector);
            InitConnector(connector, name);
            App.StartCoroutine(connector.StartServer());
            return (T)connector;
        }

        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="name">通道</param>
        public T Create<T>(string name) where T : IConnector
        {
            if (this.connector.ContainsKey(name)) { return (T)this.connector[name]; }
            IConnector connector = App.Make<T>();
            this.connector.Add(name, connector);
            InitConnector(connector, name);
            App.StartCoroutine(connector.StartServer());
            return (T)connector;

        }

        /// <summary>
        /// 断开一个网络链接
        /// </summary>
        /// <param name="name">通道</param>
        public void Destroy(string name)
        {
            if (connector.ContainsKey(name))
            {
                connector[name].Destroy();
            }
            connector.Remove(name);
        }

        /// <summary>
        /// 获取一个链接器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">通道</param>
        /// <returns></returns>
        public T Get<T>(string name) where T : IConnector
        {
            if (connector.ContainsKey(name))
            {
                return (T)connector[name];
            }
            return default(T);
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            foreach(KeyValuePair<string , IConnector> conn in connector)
            {
                conn.Value.Destroy();
            }
            connector.Clear();
        }

        private void InitConnector(IConnector connector, string name)
        {
            if (Config.IsExists(name))
            {
                connector.SetConfig(Config.Get<Hashtable>(name));
            }   
        }

    }

}
