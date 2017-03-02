using CatLib.API.Network;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System;
using CatLib.API;

namespace CatLib.Network
{

    public class HttpWebRequest : Component , IConnectorHttp
    {


        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cookie容器
        /// </summary>
        private CookieContainer cookieContainer = new CookieContainer();

        /// <summary>
        /// 停止标记
        /// </summary>
        private bool stopMark = false;

        /// <summary>
        /// 服务器地址
        /// </summary>
        private string url;

        /// <summary>
        /// 超时
        /// </summary>
        private int timeout;

        /// <summary>
        /// 发送队列
        /// </summary>
        private Queue<HttpWebRequestEntity> queue = new Queue<HttpWebRequestEntity>();

        private Hashtable triggerLevel;

        private Dictionary<string, string> headers;
        public Dictionary<string, string> Headers { get { return headers; } }

        public void SetConfig(Hashtable config){

            if(config.ContainsKey("host")){
                url = config["host"].ToString().TrimEnd('/');
            }

            if(config.ContainsKey("timeout")){
                timeout = Convert.ToInt32(config["timeout"].ToString());
            }

            if (config.ContainsKey("trigger"))
            {
                if (config["trigger"] is Hashtable)
                {
                    triggerLevel = config["trigger"] as Hashtable;
                }
            }
        }

        public IConnectorHttp SetHeader(Dictionary<string, string> headers)
        {
            this.headers = headers;
            return this;
        }

        public IConnectorHttp AppendHeader(string header, string val)
        {
            if (headers == null) { headers = new Dictionary<string, string>(); }
            headers.Remove(header);
            headers.Add(header, val);
            return this;
        }

        public void Restful(ERestful method, string action)
        {
            HttpWebRequestEntity request = new HttpWebRequestEntity(url + action , method);
            queue.Enqueue(request);
        }

        public void Restful(ERestful method, string action , WWWForm form)
        {
            HttpWebRequestEntity request = new HttpWebRequestEntity(url + action, method);
            request.SetBody(form.data).SetContentType("application/x-www-form-urlencoded");
            queue.Enqueue(request);
        }

        public void Restful(ERestful method , string action , byte[] body)
        {
            HttpWebRequestEntity request = new HttpWebRequestEntity(url + action, method);
            request.SetBody(body).SetContentType("application/octet-stream");
            queue.Enqueue(request);
        }

        public void Get(string action)
        {
            Restful(ERestful.Get, action);
        }

        public void Head(string action)
        {
            Restful(ERestful.Head, action);
        }

        public void Post(string action, WWWForm form)
        {
            Restful(ERestful.Post , action, form);
        }

        public void Post(string action, byte[] body)
        {
            Restful(ERestful.Post, action, body);
        }

        public void Put(string action, WWWForm form)
        {
            Restful(ERestful.Put, action, form);
        }

        public void Put(string action, byte[] body)
        {
            Restful(ERestful.Put, action, body);
        }

        public void Delete(string action)
        {
            Restful(ERestful.Delete, action);
        }

        /// <summary>
        /// 释放链接
        /// </summary>
        public void Destroy()
        {
            stopMark = true;
        }

        /// <summary>
        /// 请求到服务器
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartServer()
        {
            while (true)
            {
                if (stopMark) { break; }
                if (queue.Count > 0)
                {
                    HttpWebRequestEntity request;
                    while (queue.Count > 0)
                    {
                        if (stopMark) { break; }
                        request = queue.Dequeue();
                        request.SetContainer(cookieContainer);
                        request.SetHeader(headers);
                        request.SetTimeOut(timeout).SetReadWriteTimeOut(timeout);

                        yield return request.Send();

                        EventLevel level = EventLevel.All;
                        if (triggerLevel != null && triggerLevel.ContainsKey(HttpRequestEvents.ON_MESSAGE))
                        {
                            level = (EventLevel)int.Parse(triggerLevel[HttpRequestEvents.ON_MESSAGE].ToString());
                        }

                        var args = new HttpRequestEventArgs(request);

                        Event.Trigger(HttpRequestEvents.ON_MESSAGE);
                        App.Trigger(this)
                           .SetEventName(HttpRequestEvents.ON_MESSAGE)
                           .SetEventLevel(level)
                           .AppendInterface<IConnectorHttp>()
                           .Trigger(args);
                                        
                    }
                }
                yield return new WaitForEndOfFrame();
            }

        }

    }

}