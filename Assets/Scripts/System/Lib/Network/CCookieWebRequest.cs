using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using CatLib.Contracts.Base;
using CatLib.Container;
using CatLib.Contracts.Event;
using System.Net;
using System;
using System.IO;
using CatLib.Contracts.Network;

namespace CatLib.Network
{

    public class CCookieWebRequest : IConnectorHttp
    {

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="bytes"></param>
        public void Post(byte[] bytes) { }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="fields"></param>
        public void Post(string action, Dictionary<string, string> fields) { }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bytes"></param>
        public void Post(string action, byte[] bytes) { }

        /// <summary>
        /// 以post模式发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="formData"></param>
        public void Post(string action, WWWForm formData) { }

        /// <summary>
        /// 以Put模式推送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bodyData"></param>
        public void Put(string action, byte[] bodyData) { }

        /// <summary>
        /// 以get模式发送请求
        /// </summary>
        /// <param name="action"></param>
        public void Get(string action) { }


















        public class RequestEnumerator : IEnumerator
        {
            bool done = false;
            System.Net.HttpWebRequest request;

            public RequestEnumerator(System.Net.HttpWebRequest request)
            {
                this.request = request;
            }

            public object Current
            {
                get
                {
                    byte[] r = SendWebRequest(request);
                    Debug.Log("LoG: " + System.Text.UTF8Encoding.Default.GetString(r));
                    return r;
                }
            }

            public bool MoveNext()
            {
                if (this.done)
                {
                    return false;
                }
                this.done = true;
                return true;
            }

            public void Reset()
            {
            }
        }

        private static CookieContainer cookie = new CookieContainer();

        private static System.Net.HttpWebRequest GetWebRequest(String url, byte[] bytes, string[] headers)
        {
            System.Net.HttpWebRequest request = WebRequest.Create(url) as System.Net.HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.CookieContainer = cookie;
            foreach (String header in headers)
            {
                if ("user-agent:".Equals(header.Substring(0, 11).ToLower()))
                {
                    request.UserAgent = header.Substring(11).TrimStart(' ');
                    continue;
                }
                request.Headers.Add(header);
            }
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            return request;
        }

        private static byte[] SendWebRequest(System.Net.HttpWebRequest request)
        {
            HttpWebResponse response = null;
            Stream responseStream = null;
            MemoryStream memoryStream = null;
            try
            {

                response = request.GetResponse() as HttpWebResponse;
                cookie.Add(response.Cookies);
   
                responseStream = response.GetResponseStream();
                memoryStream = new MemoryStream();

                int bufSize = 8192, count;
                byte[] buffer = new byte[bufSize];
                count = responseStream.Read(buffer, 0, bufSize);
                while (count > 0)
                {
                    memoryStream.Write(buffer, 0, count);
                    count = responseStream.Read(buffer, 0, bufSize);
                }
                return memoryStream.ToArray();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (memoryStream != null)
                {
                    memoryStream.Close();
                }
            }
        }

        private static byte[] Curl(String url, byte[] bytes, string[] headers)
        {
            return SendWebRequest(GetWebRequest(url, bytes, headers));
        }

        /// <summary>
        /// 是否断开链接
        /// </summary>
        private bool isDisconnect = false;

        /// <summary>
        /// 服务器地址
        /// </summary>

        private string url;

        /// <summary>
        /// 发送队列
        /// </summary>
        private Queue<System.Net.HttpWebRequest> queue = new Queue<System.Net.HttpWebRequest>();

        /// <summary>
        /// 设定服务器访问地址
        /// </summary>
        /// <param name="url"></param>
        public IConnectorHttp SetUrl(string url)
        {
            this.url = url.TrimEnd('/');
            return this;
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes)
        {
            SendTo(url, bytes);
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bytes"></param>
        public void Send(string action, byte[] bytes)
        {
            SendTo(url + "/" + action, bytes);
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="action"></param>
        private void SendTo(string url , byte[] bytes)
        {
            System.Net.HttpWebRequest request = GetWebRequest(url, bytes, new string[] { });
            queue.Enqueue(request);
        }

        /// <summary>
        /// 断开链接
        /// </summary>
        public void Disconnect()
        {
            isDisconnect = true;
        }

        /// <summary>
        /// 请求到服务器
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartServer()
        {
            while (true)
            {
                if (isDisconnect) { break; }
                if (queue.Count > 0)
                {
                    System.Net.HttpWebRequest request;
                    while (queue.Count > 0)
                    {
                        request = queue.Dequeue();
                        yield return new RequestEnumerator(request);

                        /*if (request.isError || request.responseCode != 200)
                        {
                            if (Dispatcher is IEvent)
                            {
                                (Dispatcher as IEvent).Event.Trigger(GetType().ToString(), this, new CWebRequestErrorEventArgs(request));
                            }
                        }
                        else
                        {
                            if (Dispatcher is IEvent)
                            {
                                (Dispatcher as IEvent).Event.Trigger(GetType().ToString(), this, new CWebRequestEventArgs(request));
                            }
                        }*/
                    }
                }
                yield return new WaitForEndOfFrame();
            }

        }

    }

}