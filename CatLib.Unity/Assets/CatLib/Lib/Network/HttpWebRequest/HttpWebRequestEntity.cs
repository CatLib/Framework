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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using CatLib.API.Network;

namespace CatLib.Network
{
    /// <summary>
    /// HttpWebRequest实体
    /// </summary>
    public sealed class HttpWebRequestEntity
    {
        /// <summary>
        /// cookie容器
        /// </summary>
        private CookieContainer cookieContainer;

        /// <summary>
        /// cookie容器
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return cookieContainer; }
        }

        /// <summary>
        /// 请求头
        /// </summary>
        private ICollection<KeyValuePair<string, string>> headers;

        /// <summary>
        /// 请求头
        /// </summary>
        public ICollection<KeyValuePair<string, string>> Headers
        {
            get { return headers; }
        }

        /// <summary>
        /// 请求字节
        /// </summary>
        private byte[] requestBytes;

        /// <summary>
        /// 响应
        /// </summary>
        private HttpWebRequestResponse response;

        /// <summary>
        /// 响应
        /// </summary>
        public HttpWebRequestResponse Response
        {
            get { return response; }
        }

        /// <summary>
        /// 请求uri
        /// </summary>
        private string uri;

        /// <summary>
        /// 请求方法
        /// </summary>
        private Restfuls method;

        /// <summary>
        /// Content Type
        /// </summary>
        private string contentType;

        /// <summary>
        /// 超时
        /// </summary>
        private int timeout;

        /// <summary>
        /// 读写超时
        /// </summary>
        private int readWriteTimeout;

        /// <summary>
        /// 构建一个请求实体
        /// </summary>
        /// <param name="uri">uri</param>
        public HttpWebRequestEntity(string uri)
        {
            this.uri = uri;
            method = Restfuls.Get;
        }

        /// <summary>
        /// 构建一个请求实体
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="method">方法</param>
        public HttpWebRequestEntity(string uri, Restfuls method)
        {
            this.uri = uri;
            this.method = method;
        }

        /// <summary>
        /// 设定读写超时时间
        /// </summary>
        /// <param name="readWriteTimeout">超时时间</param>
        /// <returns>当前实例</returns>
        public HttpWebRequestEntity SetReadWriteTimeOut(int readWriteTimeout)
        {
            this.readWriteTimeout = readWriteTimeout;
            return this;
        }

        /// <summary>
        /// 设定超时时间
        /// </summary>
        /// <param name="timeout">超时时间</param>
        /// <returns>当前实例</returns>
        public HttpWebRequestEntity SetTimeOut(int timeout)
        {
            this.timeout = timeout;
            return this;
        }

        /// <summary>
        /// 设定请求方法
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns>当前实例</returns>
        public HttpWebRequestEntity SetMethod(Restfuls method)
        {
            this.method = method;
            return this;
        }

        /// <summary>
        /// 设定包体
        /// </summary>
        /// <param name="bytes">包体数据</param>
        /// <returns>当前实例</returns>
        public HttpWebRequestEntity SetBody(byte[] bytes)
        {
            requestBytes = bytes;
            return this;
        }

        /// <summary>
        /// 设定Content Type
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>当前实例</returns>
        public HttpWebRequestEntity SetContentType(string type)
        {
            contentType = type;
            return this;
        }

        /// <summary>
        /// 设定cookie容器
        /// </summary>
        /// <param name="cookieContainer">cookie容器</param>
        /// <returns>当前实例</returns>
        public HttpWebRequestEntity SetContainer(CookieContainer cookieContainer)
        {
            this.cookieContainer = cookieContainer;
            return this;
        }

        /// <summary>
        /// 设定请求头
        /// </summary>
        /// <param name="headers">请求头</param>
        /// <returns>当前实例</returns>
        public HttpWebRequestEntity SetHeader(ICollection<KeyValuePair<string, string>> headers)
        {
            this.headers = headers;
            return this;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns>协程</returns>
        public IEnumerator Send()
        {
            return response = GetWebRequestResponse();
        }

        /// <summary>
        /// 获取响应数据
        /// </summary>
        /// <returns>响应数据</returns>
        private HttpWebRequestResponse GetWebRequestResponse()
        {
            var webRequest = new System.Net.HttpWebRequest(new Uri(uri));
            var requestResponse = new HttpWebRequestResponse(webRequest);
            webRequest.Method = method.ToString();

            webRequest.KeepAlive = false;

            if (cookieContainer != null)
            {
                webRequest.CookieContainer = cookieContainer;
            }

            if (headers != null)
            {
                foreach (var kv in headers)
                {
                    webRequest.Headers.Add(kv.Key, kv.Value);
                }
            }

            if (requestBytes != null)
            {
                if (method != Restfuls.Delete)
                {
                    webRequest.ContentType = contentType;
                    webRequest.ContentLength = requestBytes.Length;
                    requestResponse.SetRequestBytes(requestBytes);
                }
            }

            if (timeout > 0)
            {
                webRequest.Timeout = timeout;
            }

            if (readWriteTimeout > 0)
            {
                webRequest.ReadWriteTimeout = readWriteTimeout;
            }

            requestResponse.Send();
            return requestResponse;
        }
    }
}