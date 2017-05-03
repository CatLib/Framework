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
        /// <summary>
        /// Web请求
        /// </summary>
        public HttpWebRequestEntity WebRequest { get; protected set; }

        /// <summary>
        /// 请求对象
        /// </summary>
        public object Request
        {
            get { return WebRequest; }
        }

        /// <summary>
        /// 响应数据
        /// </summary>
        public byte[] Bytes
        {
            get { return WebRequest.Response.Bytes; }
        }

        /// <summary>
        /// 响应字符串
        /// </summary>
        public string Text
        {
            get { return WebRequest.Response.Text; }
        }

        /// <summary>
        /// 是否出现错误
        /// </summary>
        public bool IsError
        {
            get { return WebRequest.Response.IsError; }
        }

        /// <summary>
        /// 错误字符串
        /// </summary>
        public string Error
        {
            get { return WebRequest.Response.Error; }
        }

        /// <summary>
        /// 响应代码
        /// </summary>
        public long ResponseCode
        {
            get { return WebRequest.Response.ResponseCode; }
        }

        /// <summary>
        /// Restful类型
        /// </summary>
        public Restfuls Restful
        {
            get { return (Restfuls)Enum.Parse(typeof(Restfuls), WebRequest.Response.WebRequest.Method); }
        }

        /// <summary>
        /// 构建一个请求参数
        /// </summary>
        /// <param name="request">请求</param>
        public HttpRequestEventArgs(HttpWebRequestEntity request)
        {
            WebRequest = request;
        }
    }
}