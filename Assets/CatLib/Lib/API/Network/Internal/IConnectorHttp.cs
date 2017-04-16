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
 
using UnityEngine;
using System.Collections.Generic;

namespace CatLib.API.Network
{
    /// <summary>
    /// 连接器
    /// </summary>
    public interface IConnectorHttp : IConnector
    {
        /// <summary>
        /// 设定头
        /// </summary>
        /// <param name="headers">头信息</param>
        /// <returns>当前实例</returns>
        IConnectorHttp SetHeader(Dictionary<string, string> headers);

        /// <summary>
        /// 追加头
        /// </summary>
        /// <param name="header">头</param>
        /// <param name="val">值</param>
        /// <returns>当前实例</returns>
        IConnectorHttp AppendHeader(string header, string val);

        /// <summary>
        /// 以Restful请求
        /// </summary>
        /// <param name="method">方法类型</param>
        /// <param name="action">请求行为</param>
        void Restful(Restfuls method, string action);

        /// <summary>
        /// 以Restful请求
        /// </summary>
        /// <param name="method">方法类型</param>
        /// <param name="action">请求行为</param>
        /// <param name="form">包体数据</param>
        void Restful(Restfuls method, string action, WWWForm form);

        /// <summary>
        /// 以Restful进行请求
        /// </summary>
        /// <param name="method">方法类型</param>
        /// <param name="action">请求行为</param>
        /// <param name="body">包体数据</param>
        void Restful(Restfuls method, string action, byte[] body);

        /// <summary>
        /// 以Get请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        void Get(string action);

        /// <summary>
        /// 以Head请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        void Head(string action);

        /// <summary>
        /// 以Post请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="form">post数据</param>
        void Post(string action, WWWForm form);

        /// <summary>
        /// 以Post请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="body">post数据</param>
        void Post(string action, byte[] body);

        /// <summary>
        /// 以Put请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="form">post数据</param>
        void Put(string action, WWWForm form);

        /// <summary>
        /// 以Put请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="body">post数据</param>
        void Put(string action, byte[] body);

        /// <summary>
        /// 以Delete请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        void Delete(string action);
    }
}
