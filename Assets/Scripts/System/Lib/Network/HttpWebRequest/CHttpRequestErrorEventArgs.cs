using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace CatLib.Network.HttpWebRequest
{

    /// <summary>
    /// 错误请求参数
    /// </summary>
    public class CHttpRequestErrorEventArgs : System.EventArgs
    {

        public CHttpWebRequestEntity Request { get; set; }

        public CHttpRequestErrorEventArgs(CHttpWebRequestEntity request)
        {
            Request = request;
        }
    }

}
