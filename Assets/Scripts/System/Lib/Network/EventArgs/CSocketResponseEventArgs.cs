using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CatLib.Contracts.Network;

namespace CatLib.Network {

    public class CSocketResponseEventArgs : EventArgs , IResponse
    {

        public byte[] Response { get; protected set; }

        public CSocketResponseEventArgs(byte[] bytes)
        {
            Response = bytes;
        }

    }

}