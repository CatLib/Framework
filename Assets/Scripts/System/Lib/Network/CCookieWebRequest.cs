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

    public class RequestEnumerator : IEnumerator
    {
        bool done = false;
        HttpWebRequest request;

        public RequestEnumerator(HttpWebRequest request)
        {
            this.request = request;
        }

        public object Current
        {
            get
            {
                byte[] r = CCookieWebRequest.sendWebRequest(request);
                Debug.Log( "LoG: " + System.Text.UTF8Encoding.Default.GetString(r) );
                return (object)r; //  System.Text.UTF8Encoding.Default.GetString(r);
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

    public class CCookieWebRequest : IConnectorShort
    {
        static CookieContainer cookie = new CookieContainer();

        static HttpWebRequest getWebRequest(String url, byte[] bytes, string[] headers)
        {
            // String url = "http://echo-system.co:8080/test/index?a=1";

            // var bytes = System.Text.UTF8Encoding.Default.GetBytes("abc");
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
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

        public static byte[] sendWebRequest(HttpWebRequest request)
        {
            HttpWebResponse response = null;
            Stream responseStream = null;
            MemoryStream memoryStream = null;
            try
            {
                // send request
                response = request.GetResponse() as HttpWebResponse;
                cookie.Add(response.Cookies);
                // Console.WriteLine("cookie.Count = {0}", cookie.Count);

                responseStream = response.GetResponseStream();
                memoryStream = new MemoryStream();
                // responseStream.CopyTo(memoryStream);

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

        static byte[] curl(String url, byte[] bytes, string[] headers)
        {
            return sendWebRequest(getWebRequest(url, bytes, headers));
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
        private Queue<HttpWebRequest> queue = new Queue<HttpWebRequest>();

        /// <summary>
        /// 设定服务器访问地址
        /// </summary>
        /// <param name="url"></param>
        public void SetUrl(string url)
        {
            this.url = url.TrimEnd('/');
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
            HttpWebRequest request = getWebRequest(url, bytes, new string[] { });
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
                    HttpWebRequest request;
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