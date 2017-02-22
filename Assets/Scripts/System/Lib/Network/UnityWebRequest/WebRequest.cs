using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CatLib.API.Network;
using CatLib.Base;

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
            if(method == ERestful.POST)
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
                case ERestful.GET: request = UnityWebRequest.Get(url + action); break;
                case ERestful.PUT: request = UnityWebRequest.Put(url + action, body); break;
                case ERestful.DELETE: request = UnityWebRequest.Delete(url + action); break;
                case ERestful.HEAD: request = UnityWebRequest.Head(url + action); break;
                default: throw new Exception("this component is not support [" + method.ToString() + "] restful");
            }
            queue.Enqueue(request);
        }

        public void Get(string action)
        {
            Restful(ERestful.GET, action);
        }

        public void Head(string action)
        {
            Restful(ERestful.HEAD, action);
        }

        public void Post(string action, WWWForm form)
        {
            Restful(ERestful.POST, action, form);
        }

        public void Post(string action, byte[] body)
        {
            Restful(ERestful.POST, action, body);
        }

        public void Put(string action, WWWForm form)
        {
            Restful(ERestful.PUT, action, form);
        }

        public void Put(string action, byte[] body)
        {
            Restful(ERestful.PUT, action, body);
        }

        public void Delete(string action)
        {
            Restful(ERestful.DELETE, action);
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
  
                        TriggerLevel level = TriggerLevel.ALL;
                        if (triggerLevel != null && triggerLevel.ContainsKey(HttpRequestEvents.ON_MESSAGE))
                        {
                            level = (TriggerLevel)int.Parse(triggerLevel[HttpRequestEvents.ON_MESSAGE].ToString());
                        }

                        var args = new WebRequestEventArgs(request);

                        if ((level & TriggerLevel.SELF) > 0)
                        {
                            Event.Trigger(HttpRequestEvents.ON_MESSAGE, this, args);
                            App.Trigger(HttpRequestEvents.ON_MESSAGE + TypeGuid, this, args);
                        }

                        if ((level & TriggerLevel.TYPE) > 0)
                        {
                            App.Trigger(HttpRequestEvents.ON_MESSAGE + GetType().ToString(), this, args);
                            App.Trigger(HttpRequestEvents.ON_MESSAGE + typeof(IConnectorHttp).ToString(), this, args);
                        }

                        if ((level & TriggerLevel.GLOBAL) > 0)
                        {
                            App.Trigger(HttpRequestEvents.ON_MESSAGE, this, args);
                        }

                    }
                }
                yield return new WaitForEndOfFrame();
            }

        }

    }

}