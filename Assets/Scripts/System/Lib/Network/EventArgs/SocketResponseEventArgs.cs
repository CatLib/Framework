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