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
    public sealed class Network : IDestroy, INetworkFactory
    {
        /// <summary>
        /// 应用程序
        /// </summary>
        [Inject]
        public IApplication App { get; set; }

        /// <summary>
        /// 建立出的连接器
        /// </summary>
        private readonly Dictionary<string, IConnector> connector = new Dictionary<string, IConnector>();

        /// <summary>
        /// 配置查询器
        /// </summary>
        private Func<string, Hashtable> configSearch;

        /// <summary>
        /// 设定配置查询器
        /// </summary>
        /// <param name="search">查询配置</param>
        public void SetQuery(Func<string, Hashtable> search)
        {
            configSearch = search;
        }

        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="name">连接名</param>
        public T Create<T>(string name) where T : IConnector
        {
            if (this.connector.ContainsKey(name))
            {
                return (T)this.connector[name];
            }
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
            if (connector == null)
            {
                connector = App.Make<T>();
            }
            this.connector.Add(name, connector);
            InitConnector(connector, name, table);
            App.StartCoroutine(connector.StartServer());
            return (T)connector;
        }

        /// <summary>
        /// 释放一个网络链接
        /// </summary>
        /// <param name="name">连接名</param>
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
        /// <typeparam name="T">连接类型</typeparam>
        /// <param name="name">连接名</param>
        /// <returns>连接器</returns>
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
            foreach (var conn in connector)
            {
                conn.Value.Destroy();
            }
            connector.Clear();
        }

        /// <summary>
        /// 初始化连接器
        /// </summary>
        /// <param name="connector">连接器</param>
        /// <param name="name">连接名</param>
        /// <param name="table">配置表</param>
        private void InitConnector(IConnector connector, string name, Hashtable table)
        {
            if (table == null)
            {
                return;
            }
            connector.SetConfig(table);
        }
    }
}
