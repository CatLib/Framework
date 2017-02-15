using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Base
{

    public class CErrorEventArgs : EventArgs
    {

        public Exception Error { get; protected set; }

        public CErrorEventArgs(Exception ex)
        {
            Error = ex;
        }

    }

}