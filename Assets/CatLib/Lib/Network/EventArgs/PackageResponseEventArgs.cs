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
 
using System;
using CatLib.API.Network;

namespace CatLib.Network
{

    public class PackageResponseEventArgs : EventArgs , IPackageResponse
    {

        public IPackage Response { get; protected set; }

        public PackageResponseEventArgs(IPackage package)
        {
            Response = package;
        }

    }

}