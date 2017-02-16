using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CatLib.Contracts.NetPackage
{
    /// <summary>
    /// 数据包
    /// </summary>
    public interface IPackage
    {

        /// <summary>
        /// 数据包
        /// </summary>
        object Package { get; }

        /// <summary>
        /// 数据包字节流
        /// </summary>
        byte[] PackageByte{ get; }
    
    }
}
