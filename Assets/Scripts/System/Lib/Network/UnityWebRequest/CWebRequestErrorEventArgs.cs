using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace CatLib.Network.UnityWebRequest
{

    /// <summary>
    /// 错误请求参数
    /// </summary>
    public class CWebRequestErrorEventArgs : System.EventArgs
    {

        public UnityEngine.Networking.UnityWebRequest Request { get; set; }

        public CWebRequestErrorEventArgs(UnityEngine.Networking.UnityWebRequest request)
        {
            Request = request;
        }
    }

}
