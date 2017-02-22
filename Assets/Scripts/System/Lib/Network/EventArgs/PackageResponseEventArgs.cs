using System;
using CatLib.API.Network;

namespace CatLib.Network
{

    public class PackageResponseEventArgs : EventArgs
    {

        public IPackage Response { get; protected set; }

        public PackageResponseEventArgs(IPackage package)
        {
            Response = package;
        }

    }

}