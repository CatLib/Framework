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

using CatLib.API.Network;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System;
using CatLib.API;
using CatLib.API.Event;

namespace CatLib.Network
{
    /// <summary>
    /// HttpWeb请求
    /// </summary>
    public sealed class HttpWebRequest : IEvent, IConnectorHttp
    {
        /// <summary>
        /// 事件系统
        /// </summary>
        [Inject]
        public IEventImpl Event { get; set; }

        /// <summary>
        /// 应用程序
        /// </summary>
        [Inject]
        public IApplication App { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cookie容器
        /// </summary>
        private readonly CookieContainer cookieContainer = new CookieContainer();

        /// <summary>
        /// 停止标记
        /// </summary>
        private bool stopMark;

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
        private readonly Queue<HttpWebRequestEntity> queue = new Queue<HttpWebRequestEntity>();

        /// <summary>
        /// 事件等级
        /// </summary>
        private Hashtable triggerLevel;

        /// <summary>
        /// 头信息
        /// </summary>
        private Dictionary<string, string> headers;

        /// <summary>
        /// 头信息
        /// </summary>
        public Dictionary<string, string> Headers { get { return headers; } }

        /// <summary>
        /// 设定配置
        /// </summary>
        /// <param name="config">配置信息</param>
        public void SetConfig(Hashtable config)
        {
            if (config.ContainsKey("host"))
            {
                url = config["host"].ToString().TrimEnd('/');
            }

            if (config.ContainsKey("timeout"))
            {
                timeout = Convert.ToInt32(config["timeout"].ToString());
            }

            if (config.ContainsKey("event.level"))
            {
                if (config["event.level"] is Hashtable)
                {
                    triggerLevel = config["event.level"] as Hashtable;
                }
            }
        }

        /// <summary>
        /// 设定头
        /// </summary>
        /// <param name="headers">头信息</param>
        /// <returns>当前实例</returns>
        public IConnectorHttp SetHeader(Dictionary<string, string> headers)
        {
            this.headers = headers;
            return this;
        }

        /// <summary>
        /// 追加头
        /// </summary>
        /// <param name="header">头</param>
        /// <param name="val">值</param>
        /// <returns>当前实例</returns>
        public IConnectorHttp AppendHeader(string header, string val)
        {
            if (headers == null) { headers = new Dictionary<string, string>(); }
            headers.Remove(header);
            headers.Add(header, val);
            return this;
        }

        /// <summary>
        /// 以Restful请求
        /// </summary>
        /// <param name="method">方法类型</param>
        /// <param name="action">请求行为</param>
        public void Restful(Restfuls method, string action)
        {
            var request = new HttpWebRequestEntity(url + action, method);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Restful请求
        /// </summary>
        /// <param name="method">方法类型</param>
        /// <param name="action">请求行为</param>
        /// <param name="form">包体数据</param>
        public void Restful(Restfuls method, string action, WWWForm form)
        {
            var request = new HttpWebRequestEntity(url + action, method);
            request.SetBody(form.data).SetContentType("application/x-www-form-urlencoded");
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Restful进行请求
        /// </summary>
        /// <param name="method">方法类型</param>
        /// <param name="action">请求行为</param>
        /// <param name="body">包体数据</param>
        public void Restful(Restfuls method, string action, byte[] body)
        {
            var request = new HttpWebRequestEntity(url + action, method);
            request.SetBody(body).SetContentType("application/octet-stream");
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Get请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        public void Get(string action)
        {
            Restful(Restfuls.Get, action);
        }

        /// <summary>
        /// 以Head请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        public void Head(string action)
        {
            Restful(Restfuls.Head, action);
        }

        /// <summary>
        /// 以Post请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="form">post数据</param>
        public void Post(string action, WWWForm form)
        {
            Restful(Restfuls.Post, action, form);
        }

        /// <summary>
        /// 以Post请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="body">post数据</param>
        public void Post(string action, byte[] body)
        {
            Restful(Restfuls.Post, action, body);
        }

        /// <summary>
        /// 以Put请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="form">post数据</param>
        public void Put(string action, WWWForm form)
        {
            Restful(Restfuls.Put, action, form);
        }

        /// <summary>
        /// 以Put请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        /// <param name="body">post数据</param>
        public void Put(string action, byte[] body)
        {
            Restful(Restfuls.Put, action, body);
        }

        /// <summary>
        /// 以Delete请求数据
        /// </summary>
        /// <param name="action">请求行为</param>
        public void Delete(string action)
        {
            Restful(Restfuls.Delete, action);
        }

        /// <summary>
        /// 释放链接
        /// </summary>
        public void Destroy()
        {
            stopMark = true;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>协程</returns>
        public IEnumerator StartServer()
        {
            while (true)
            {
                if (stopMark)
                {
                    break;
                }
                if (queue.Count > 0)
                {
                    HttpWebRequestEntity request;
                    while (queue.Count > 0)
                    {
                        if (stopMark)
                        {
                            break;
                        }
                        request = queue.Dequeue();
                        request.SetContainer(cookieContainer);
                        request.SetHeader(headers);
                        request.SetTimeOut(timeout).SetReadWriteTimeOut(timeout);

                        yield return request.Send();

                        var level = EventLevel.All;
                        if (triggerLevel != null && triggerLevel.ContainsKey(HttpRequestEvents.ON_MESSAGE))
                        {
                            level = (EventLevel)triggerLevel[HttpRequestEvents.ON_MESSAGE];
                        }

                        var args = new HttpRequestEventArgs(request);

                        Event.Trigger(HttpRequestEvents.ON_MESSAGE, this, args);
                        App.TriggerGlobal(HttpRequestEvents.ON_MESSAGE, this)
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