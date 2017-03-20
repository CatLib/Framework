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

namespace CatLib.Network {

    public class SocketResponseEventArgs : EventArgs , IResponse
    {

        public byte[] Response { get; protected set; }

        public SocketResponseEventArgs(byte[] bytes)
        {
            Response = bytes;
        }

    }

}