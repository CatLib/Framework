using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using CatLib.Contracts.Network;
using System;

namespace CatLib.Network
{

    /// <summary>
    /// 请求参数
    /// </summary>
    public class CHttpRequestEventArgs : EventArgs, IHttpResponse
    {

        public CHttpWebRequestEntity WebRequest { get; protected set; }

        public object Request { get { return WebRequest; } }

        public byte[] Bytes { get { return WebRequest.Response.Bytes; } }

        public string Text { get { return WebRequest.Response.Text; } }

        public bool IsError { get { return WebRequest.Response.IsError; } }

        public string Error { get { return WebRequest.Response.Error; } }

        public long ResponseCode { get { return WebRequest.Response.ResponseCode; } }

        public ERestful Restful { get { return (ERestful)Enum.Parse(typeof(ERestful), WebRequest.Response.WebRequest.Method); } }

        public CHttpRequestEventArgs(CHttpWebRequestEntity request)
        {
            WebRequest = request;
        }

    }
}