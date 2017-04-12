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
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#elif UNITY_5_2 || UNITY_5_3
using UnityEngine.Experimental.Networking;
#endif
using CatLib.API.Network;
using CatLib.API;
using CatLib.API.Event;

namespace CatLib.Network
{

    public class WebRequest : IEvent, IConnectorHttp
    {

        [Dependency]
        public IEventAchieve Event { get; set; }

        [Dependency]
        public IApplication App { get; set; }

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

        public void SetConfig(Hashtable config)
        {

            if (config.ContainsKey("host"))
            {
                url = config["host"].ToString().TrimEnd('/');
            }

            if (config.ContainsKey("timeout"))
            {
                throw new Exception(this.GetType().ToString() + " is not support [timeout] config");
            }

            if (config.ContainsKey("event.level"))
            {
                if (config["event.level"] is Hashtable)
                {
                    triggerLevel = config["event.level"] as Hashtable;
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

        public IConnectorHttp AppendHeader(string header, string val)
        {
            if (headers == null) { headers = new Dictionary<string, string>(); }
            headers.Remove(header);
            headers.Add(header, val);
            return this;
        }


        public void Restful(ERestful method, string action)
        {
            UnityWebRequest request = new UnityWebRequest(url + action, method.ToString());
            queue.Enqueue(request);
        }

        public void Restful(ERestful method, string action, WWWForm form)
        {
            if (method == ERestful.Post)
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
                            foreach (var kv in headers)
                            {
                                request.SetRequestHeader(kv.Key, kv.Value);
                            }
                        }
                        yield return request.Send();

                        EventLevel level = EventLevel.All;
                        if (triggerLevel != null && triggerLevel.ContainsKey(HttpRequestEvents.ON_MESSAGE))
                        {
                            level = (EventLevel)triggerLevel[HttpRequestEvents.ON_MESSAGE];
                        }

                        var args = new WebRequestEventArgs(request);

                        Event.Trigger(HttpRequestEvents.ON_MESSAGE, this, args);
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