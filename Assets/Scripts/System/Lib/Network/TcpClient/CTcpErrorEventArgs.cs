using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Network
{

    /// <summary>
    /// tcp 错误事件
    /// </summary>
    public class CTcpErrorEventArgs : EventArgs
    {

        public byte[][] FaildSendData { get; protected set; }

        public CTcpErrorEventArgs(byte[][] faildData)
        {
            FaildSendData = faildData;
        }

    }
}