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

    public class HttpWebRequestEntity
    {

        private CookieContainer cookieContainer;
        public CookieContainer CookieContainer { get { return cookieContainer; } }

        private ICollection<KeyValuePair<string, string>> headers;
        public ICollection<KeyValuePair<string, string>> Headers { get { return headers; } }

        private byte[] requestBytes;
        private HttpWebRequestResponse response;
        public HttpWebRequestResponse Response { get { return response; } }

        private string uri;
        private ERestful method;
        private string contentType;
        private int timeout;
        private int readWriteTimeout;

        public HttpWebRequestEntity(string uri)
        {
            this.uri = uri;
            method = ERestful.Get;
        }

        public HttpWebRequestEntity(string uri, ERestful method)
        {
            this.uri = uri;
            this.method = method;
        }

        public HttpWebRequestEntity SetReadWriteTimeOut(int readWriteTimeout)
        {
            this.readWriteTimeout = readWriteTimeout;
            return this;
        }

        public HttpWebRequestEntity SetTimeOut(int timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public HttpWebRequestEntity SetMethod(ERestful method)
        {
            this.method = method;
            return this;
        }

        public HttpWebRequestEntity SetBody(byte[] bytes)
        {
            requestBytes = bytes;
            return this;
        }

        public HttpWebRequestEntity SetContentType(string type)
        {
            contentType = type;
            return this;
        }

        public HttpWebRequestEntity SetContainer(CookieContainer cookieContainer)
        {
            this.cookieContainer = cookieContainer;
            return this;
        }

        public HttpWebRequestEntity SetHeader(ICollection<KeyValuePair<string, string>> headers)
        {
            this.headers = headers;
            return this;
        }

        public IEnumerator Send()
        {
            return response = GetWebRequestResponse();
        }

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
                headers.Walk(webRequest.Headers.Add);
            }

            if (requestBytes != null)
            {
                if (method != ERestful.Delete)
                {
                    webRequest.ContentType = contentType;
                    webRequest.ContentLength = requestBytes.Length;
                    requestResponse.SetRequestBytes(requestBytes);
                }
            }

            if(timeout > 0)
            {
                webRequest.Timeout = timeout;
            }

            if(readWriteTimeout > 0)
            {
                webRequest.ReadWriteTimeout = readWriteTimeout;
            }

            requestResponse.Send();
            return requestResponse;

        }

    }

}