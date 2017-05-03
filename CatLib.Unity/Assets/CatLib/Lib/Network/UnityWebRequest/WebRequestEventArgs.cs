/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#elif UNITY_5_2 || UNITY_5_3
using UnityEngine.Experimental.Networking;
#endif
using CatLib.API.Network;

namespace CatLib.Network
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class WebRequestEventArgs : EventArgs, IHttpResponse
    {

        public UnityWebRequest WebRequest { get; set; }

        public object Request { get { return WebRequest; } }

        public byte[] Bytes { get { return WebRequest.downloadHandler == null ? null : WebRequest.downloadHandler.data; } }

        public string Text { get { return WebRequest.downloadHandler == null ? string.Empty : WebRequest.downloadHandler.text; } }

        public bool IsError { get { return WebRequest.isError; } }

        public string Error { get { return WebRequest.error; } }

        public long ResponseCode { get { return WebRequest.responseCode; } }

        public Restfuls Restful { get { return (Restfuls)Enum.Parse(typeof(Restfuls), WebRequest.method); } }

        public WebRequestEventArgs(UnityWebRequest request)
        {
            WebRequest = request;
        }
    }

}
