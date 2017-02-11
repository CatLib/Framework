using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Network.HttpWebRequest
{


    /// <summary>
    /// 请求参数
    /// </summary>
    public class CHttpRequestEventArgs : System.EventArgs
    {

        public CHttpWebRequestEntity Request { get; set; }

        public CHttpRequestEventArgs(CHttpWebRequestEntity request)
        {
            Request = request;
        }
    }

}
