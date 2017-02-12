using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization;

namespace CatLib.Base
{

    public class CNullReferenceException : NullReferenceException
    {

        public CNullReferenceException() { }
        public CNullReferenceException(string message) : base(message) { }
        public CNullReferenceException(string message, System.Exception inner) : base(message, inner) { }
        protected CNullReferenceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public CErrorCode ErrorCode { get { return CErrorCode.SYSTEM_EXCEPTION; } }

    }

}