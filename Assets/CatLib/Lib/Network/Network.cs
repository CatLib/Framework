/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using CatLib.API;
using CatLib.API.Network;

namespace CatLib.Network
{
    
    /// <summary>
    /// 网络服务
    /// </summary>
    public class Network :  IDestroy , INetworkFactory
    {

        [Dependency]
        public IApplication App { get; set; }

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<string, IConnector> connector = new Dictionary<string, IConnector>();

        private Func<string , Hashtable> configSearch;

        public void SetQuery(Func<string , Hashtable> search){

            configSearch = search;

        }


        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="name">通道</param>
        public T Create<T>(string name) where T : IConnector
        {
            if (this.connector.ContainsKey(name)) { return (T)this.connector[name]; }
            IConnector connector = null;
            Hashtable table = null;
            if (configSearch != null)
            {
                table = configSearch(name);
                if (table != null && table.ContainsKey("driver"))
                {
                    connector = App.Make<T>(table["driver"].ToString());
                }
            }
            if(connector == null)
            {
                connector = App.Make<T>();
            }
            this.connector.Add(name, connector);
            InitConnector(connector, name , table);
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

        private void InitConnector(IConnector connector, string name , Hashtable table)
        {
            if(table == null){ return; }
            connector.SetConfig(table); 
        }

    }

}
