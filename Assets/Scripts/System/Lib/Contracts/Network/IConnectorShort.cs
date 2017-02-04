using UnityEngine;
using System.Collections;

namespace CatLib.Contracts.Network
{
    /// <summary>
    /// 连接器
    /// </summary>
    public interface IConnectorShort : IConnector
    {

        /// <summary>
        /// 设定
        /// </summary>
        /// <param name="url"></param>
        void SetUrl(string url);

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="bytes"></param>
        void Send(byte[] bytes);

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bytes"></param>
        void Send(string action, byte[] bytes);

    }
}
