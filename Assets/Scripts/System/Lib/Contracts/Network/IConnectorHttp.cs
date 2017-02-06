using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;

namespace CatLib.Contracts.Network
{
    [LuaCallCSharp]
    /// <summary>
    /// 连接器
    /// </summary>
    public interface IConnectorHttp : IConnector
    {

        /// <summary>
        /// 设定
        /// </summary>
        /// <param name="url"></param>
        IConnectorHttp SetUrl(string url);

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="bytes"></param>
        void Post(byte[] bytes);

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="fields"></param>
        void Post(string action, Dictionary<string, string> fields);

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bytes"></param>
        void Post(string action, byte[] bytes);

        /// <summary>
        /// 以post模式发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="formData"></param>
        void Post(string action, WWWForm formData);

        /// <summary>
        /// 以Put模式推送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bodyData"></param>
        void Put(string action, byte[] bodyData);

        /// <summary>
        /// 以get模式发送请求
        /// </summary>
        /// <param name="action"></param>
        void Get(string action);

    }
}
