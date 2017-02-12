using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Contracts.Network
{

    /// <summary>
    /// 接口
    /// </summary>
    public interface IResponse
    {

        /// <summary>
        /// 响应字节流
        /// </summary>
        byte[] Bytes { get; }

    }

}