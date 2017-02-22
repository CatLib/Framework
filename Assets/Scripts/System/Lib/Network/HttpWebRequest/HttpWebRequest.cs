using CatLib.API.Network;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System;
using CatLib.Base;

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
            Restful(ERestful.GET, action);
        }

        public void Head(string action)
        {
            Restful(ERestful.HEAD, action);
        }

        public void Post(string action, WWWForm form)
        {
            Restful(ERestful.POST , action, form);
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
                    HttpWebRequestEntity request;
                    while (queue.Count > 0)
                    {
                        if (stopMark) { break; }
                        request = queue.Dequeue();
                        request.SetContainer(cookieContainer);
                        request.SetHeader(headers);
                        request.SetTimeOut(timeout).SetReadWriteTimeOut(timeout);

                        yield return request.Send();

                        TriggerLevel level = TriggerLevel.ALL;
                        if (triggerLevel != null && triggerLevel.ContainsKey(HttpRequestEvents.ON_MESSAGE))
                        {
                            level = (TriggerLevel)int.Parse(triggerLevel[HttpRequestEvents.ON_MESSAGE].ToString());
                        }

                        var args = new HttpRequestEventArgs(request);

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