using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using CatLib.Support;
using CatLib.Contracts.Network;

namespace CatLib.Network
{

    public class CHttpWebRequestEntity
    {

        private CookieContainer cookieContainer;
        public CookieContainer CookieContainer { get { return cookieContainer; } }

        private ICollection<KeyValuePair<string, string>> headers;
        public ICollection<KeyValuePair<string, string>> Headers { get { return headers; } }

        private byte[] requestBytes;
        private CHttpWebRequestResponse response;
        public CHttpWebRequestResponse Response { get { return response; } }

        private string uri;
        private ERestful method;
        private string contentType;
        private int timeout;
        private int readWriteTimeout;

        public CHttpWebRequestEntity(string uri)
        {
            this.uri = uri;
            method = ERestful.GET;
        }

        public CHttpWebRequestEntity(string uri, ERestful method)
        {
            this.uri = uri;
            this.method = method;
        }

        public CHttpWebRequestEntity SetReadWriteTimeOut(int readWriteTimeout)
        {
            this.readWriteTimeout = readWriteTimeout;
            return this;
        }

        public CHttpWebRequestEntity SetTimeOut(int timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public CHttpWebRequestEntity SetMethod(ERestful method)
        {
            this.method = method;
            return this;
        }

        public CHttpWebRequestEntity SetBody(byte[] bytes)
        {
            requestBytes = bytes;
            return this;
        }

        public CHttpWebRequestEntity SetContentType(string type)
        {
            contentType = type;
            return this;
        }

        public CHttpWebRequestEntity SetContainer(CookieContainer cookieContainer)
        {
            this.cookieContainer = cookieContainer;
            return this;
        }

        public CHttpWebRequestEntity SetHeader(ICollection<KeyValuePair<string, string>> headers)
        {
            this.headers = headers;
            return this;
        }

        public IEnumerator Send()
        {
            return response = GetWebRequestResponse();
        }

        private CHttpWebRequestResponse GetWebRequestResponse()
        {
            var webRequest = new HttpWebRequest(new Uri(uri));
            var requestResponse = new CHttpWebRequestResponse(webRequest);
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
                if (method != ERestful.DELETE)
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