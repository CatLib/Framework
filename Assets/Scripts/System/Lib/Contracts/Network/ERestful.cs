using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Contracts.Network
{

    public enum ERestful
    {

        //HTTP 1.1 协议谓词
        GET, 
        POST,
        PUT,
        DELETE,
        HEAD,
        OPTIONS,
        TRACE,

        PATCH,
        COPY,
        LINK,
        UNLINK,
        PURGE,
        LOCK,
        UNLOCK,
        PROFFIND,
        VIEW

    }

}