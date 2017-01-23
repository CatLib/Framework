using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace CatLib.Network
{


    /// <summary>
    /// 请求参数
    /// </summary>
    public class CWebRequestEventArgs : System.EventArgs
    {

        public UnityWebRequest Request { get; set; }

        public CWebRequestEventArgs(UnityWebRequest request)
        {
            Request = request;
        }
    }

}
