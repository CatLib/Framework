using CatLib.Contracts.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Network
{

    public class CPackageResponseEventArgs : EventArgs
    {

        public IPackage Response { get; protected set; }

        public CPackageResponseEventArgs(IPackage package)
        {
            Response = package;
        }

    }

}