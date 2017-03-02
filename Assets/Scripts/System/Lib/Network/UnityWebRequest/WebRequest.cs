using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CatLib.API.Network;
using CatLib.API;

namespace CatLib.Network
{

    public class WebRequest : Component, IConnectorHttp
    {

        public string Name { get; set; }

        /// <summary>
        /// 终止标记
        /// </summary>
        private bool stopMark = false;

        /// <summary>
        /// 服务器地址
        /// </summary>

        private string url;

        /// <summary>
        /// 发送队列
        /// </summary>
        private Queue<UnityWebRequest> queue = new Queue<UnityWebRequest>();

        private Hashtable triggerLevel;

        private Dictionary<string, string> headers;
        public Dictionary<string, string> Headers { get { return headers; } }

        public void SetConfig(Hashtable config){

            if(config.ContainsKey("host")){
                url = config["host"].ToString().TrimEnd('/');
            }

            if(config.ContainsKey("timeout")){
                throw new Exception(this.GetType().ToString() + " is not support [timeout] config");
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

        public IConnectorHttp SetTimeOut(int timeout)
        {
            throw new Exception("this component is not support this features");
        }

        public IConnectorHttp AppendHeader(string header , string val)
        {
            if (headers == null) { headers = new Dictionary<string, string>(); }
            headers.Remove(header);
            headers.Add(header , val);
            return this;
        }


        public void Restful(ERestful method, string action)
        {
            UnityWebRequest request = new UnityWebRequest(url + action, method.ToString());
            queue.Enqueue(request);
        }

        public void Restful(ERestful method, string action, WWWForm form)
        {
            if(method == ERestful.Post)
            {
                UnityWebRequest request = UnityWebRequest.Post(url + action, form);
                queue.Enqueue(request);
            }
            else
            {
                Restful(method, action, form.data);
            }
        }

        public void Restful(ERestful method, string action, byte[] body)
        {
            UnityWebRequest request = null;
            switch (method)
            {
                case ERestful.Get: request = UnityWebRequest.Get(url + action); break;
                case ERestful.Put: request = UnityWebRequest.Put(url + action, body); break;
                case ERestful.Delete: request = UnityWebRequest.Delete(url + action); break;
                case ERestful.Head: request = UnityWebRequest.Head(url + action); break;
                default: throw new Exception("this component is not support [" + method.ToString() + "] restful");
            }
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
            Restful(ERestful.Post, action, form);
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
                    UnityWebRequest request;
                    while (queue.Count > 0)
                    {
                        if (stopMark) { break; }
                        request = queue.Dequeue();
                        if (headers != null)
                        {
                            headers.Walk((statu, val) => request.SetRequestHeader(statu.ToString(), val));
                        }
                        yield return request.Send();
  
                        EventLevel level = EventLevel.All;
                        if (triggerLevel != null && triggerLevel.ContainsKey(HttpRequestEvents.ON_MESSAGE))
                        {
                            level = (EventLevel)int.Parse(triggerLevel[HttpRequestEvents.ON_MESSAGE].ToString());
                        }

                        var args = new WebRequestEventArgs(request);

                        Event.Trigger(eventName);
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