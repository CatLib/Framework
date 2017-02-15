using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CatLib.Network {

    public class CSocketMessageEventArgs : EventArgs
    {

        public byte[] Message { get; protected set; }

        public CSocketMessageEventArgs(byte[] bytes)
        {
            Message = bytes;
        }

    }

}