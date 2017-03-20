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
using CatLib.API.Network;

namespace CatLib.Network
{

    /// <summary>
    /// 请求参数
    /// </summary>
    public class HttpRequestEventArgs : EventArgs, IHttpResponse
    {

        public HttpWebRequestEntity WebRequest { get; protected set; }

        public object Request { get { return WebRequest; } }

        public byte[] Bytes { get { return WebRequest.Response.Bytes; } }

        public string Text { get { return WebRequest.Response.Text; } }

        public bool IsError { get { return WebRequest.Response.IsError; } }

        public string Error { get { return WebRequest.Response.Error; } }

        public long ResponseCode { get { return WebRequest.Response.ResponseCode; } }

        public ERestful Restful { get { return (ERestful)Enum.Parse(typeof(ERestful), WebRequest.Response.WebRequest.Method); } }

        public HttpRequestEventArgs(HttpWebRequestEntity request)
        {
            WebRequest = request;
        }

    }
}