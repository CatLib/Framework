using CatLib.Contracts.NetPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Network
{

    public class CPackageMessageEventArgs : EventArgs
    {

        public IPackage Package { get; protected set; }

        public CPackageMessageEventArgs(IPackage package)
        {
            Package = package;
        }

    }

}