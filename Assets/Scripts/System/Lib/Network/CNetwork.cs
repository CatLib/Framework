using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CatLib.Network
{
    
    /// <summary>
    /// 网络服务
    /// </summary>
    public class CNetwork
    {


        /// <summary>
        /// 短链接连接器
        /// </summary>
        private Dictionary<string, IConnector> connector = new Dictionary<string, IConnector>();


        /// <summary>
        /// 创建一个网络链接
        /// </summary>
        /// <param name="aisle">通道</param>
        /// <param name="host">服务器地址</param>
        /// <param name="port">端口</param>
        /// <param name="isLong">是否是长链接</param>
        public void Connect(string aisle , string host , ushort port , bool isLong = false)
        {
            if (isLong)
            {
                //todo 长连接部分待完成
                return;
            }




        }

        /// <summary>
        /// 断开一个网络链接
        /// </summary>
        /// <param name="aisle">通道</param>
        public void Disconnect(string aisle)
        {

        }

    }

}
