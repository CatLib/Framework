using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using CatLib.Exception;
using CatLib.Support;
using System.IO;

namespace CatLib.Network.HttpWebRequest
{

    public class CHttpWebRequestEntity
    {

        private CookieContainer cookieContainer;
        public CookieContainer CookieContainer { get { return cookieContainer; } }

        private string[] headers;
        public string[] Headers { get { return headers; } }

        private byte[] requestBytes;
        private CHttpWebRequestResponse response;
        public CHttpWebRequestResponse Response { get { return response; } }

        private string uri;
        private string method;

        public CHttpWebRequestEntity(string uri)
        {
            this.uri = uri;
            method = "GET";
        }

        public CHttpWebRequestEntity(string uri, string method)
        {
            this.uri = uri;
            this.method = method;
        }

        public CHttpWebRequestEntity SetContainer(CookieContainer cookieContainer)
        {
            this.cookieContainer = cookieContainer;
            return this;
        }

        public CHttpWebRequestEntity SetHeaders(string[] headers)
        {
            this.headers = headers;
            return this;
        }

        public static CHttpWebRequestEntity Get(string uri)
        {
            var obj = new CHttpWebRequestEntity(uri, "GET");
            return obj;
        }

        public static CHttpWebRequestEntity Post(string uri, Dictionary<string, string> fields)
        {
            var obj = new CHttpWebRequestEntity(uri, "POST");
            WWWForm form = new WWWForm();
            fields.Walk(form.AddField);
            return Post(uri , form);
        }

        public static CHttpWebRequestEntity Post(string uri, WWWForm formData)
        {
            var obj = new CHttpWebRequestEntity(uri, "POST");
            obj.requestBytes = formData.data;
            return obj;
        }

        public static CHttpWebRequestEntity Post(string uri , byte[] bytes)
        {
            var obj = new CHttpWebRequestEntity(uri, "POST");
            obj.requestBytes = bytes;
            return obj;
        }

        public static CHttpWebRequestEntity Put(string uri, byte[] bytes)
        {
            var obj = new CHttpWebRequestEntity(uri, "PUT");
            obj.requestBytes = bytes;
            return obj;
        }

        public IEnumerator Send()
        {
            return response = new CHttpWebRequestResponse(GetWebRequest()); ;
        }

        private System.Net.HttpWebRequest GetWebRequest()
        {
            var webRequest = new System.Net.HttpWebRequest(new Uri(uri));
            webRequest.Method = method;

            webRequest.KeepAlive = true;

            if (cookieContainer != null)
            {
                webRequest.CookieContainer = cookieContainer;
            }

            if (headers != null)
            {
                foreach (string header in headers)
                {
                    if ("user-agent:".Equals(header.Substring(0, 11).ToLower()))
                    {
                        webRequest.UserAgent = header.Substring(11).TrimStart(' ');
                        continue;
                    }
                    webRequest.Headers.Add(header);
                }
            }

            if (requestBytes != null)
            {
                var stream = webRequest.GetRequestStream();
                stream.Write(requestBytes, 0, requestBytes.Length);
                stream.Close();
            }

            return webRequest;

        }

    }

}