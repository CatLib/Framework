using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace CatLib.Network
{


    /// <summary>
    /// 请求错误参数
    /// </summary>
    public class CWebRequestErrorEventArgs : System.EventArgs
    {

        public UnityWebRequest Request { get; set; }

        public CWebRequestErrorEventArgs(UnityWebRequest request)
        {
            Request = request;
        }
    }

}
