using CatLib.Contracts.NetPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Network
{

    /// <summary>
    /// tcp 事件
    /// </summary>
    public class CTcpEventArgs : EventArgs
    {

        public IPackage Package { get; protected set; }

        public CTcpEventArgs(IPackage package)
        {
            Package = package;
        }

    }
}