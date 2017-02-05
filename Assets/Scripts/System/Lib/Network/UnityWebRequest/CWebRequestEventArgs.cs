using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace CatLib.Network.UnityWebRequest
{


    /// <summary>
    /// 请求参数
    /// </summary>
    public class CWebRequestEventArgs : System.EventArgs
    {

        public UnityEngine.Networking.UnityWebRequest Request { get; set; }

        public CWebRequestEventArgs(UnityEngine.Networking.UnityWebRequest request)
        {
            Request = request;
        }
    }

}
