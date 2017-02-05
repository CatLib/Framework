using UnityEngine;
using System.Collections;
using System;

namespace CatLib.Network.HttpWebRequest
{

    public class CHttpWebRequest
    {

        private System.Net.HttpWebRequest webRequest;

        private byte[] requestData;

        public CHttpWebRequest(string url)
        {
            webRequest = new System.Net.HttpWebRequest(new Uri(url));
            webRequest.Method = "GET";
        }

        public CHttpWebRequest(string url, string method)
        {
            webRequest = new System.Net.HttpWebRequest(new Uri(url));
            webRequest.Method = method;
        }

        public static CHttpWebRequest Get(string uri)
        {
            var obj = new CHttpWebRequest(uri, "GET");
            return obj;
        }

        public static CHttpWebRequest Post(string uri, WWWForm formData)
        {
            var obj = new CHttpWebRequest(uri, "POST");
            return obj;
        }

        public static CHttpWebRequest Post(string uri , byte[] bytes)
        {
            var obj = new CHttpWebRequest(uri, "POST");
            obj.requestData = bytes;
            return null;
        }

        public AsyncOperation Send()
        {
            return null;
        }

    }

}