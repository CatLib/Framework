using CatLib.Base;
using CatLib.Contracts.Network;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace CatLib.Network
{

    public class CHttpWebRequest : CComponent , IConnectorHttp
    {

        /// <summary>
        /// Cookie容器
        /// </summary>
        private CookieContainer cookieContainer = new CookieContainer();

        /// <summary>
        /// 是否断开链接
        /// </summary>
        private bool isDisconnect = false;

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
        private Queue<CHttpWebRequestEntity> queue = new Queue<CHttpWebRequestEntity>();

        private Dictionary<string, string> headers;
        public Dictionary<string, string> Headers { get { return headers; } }

        public IConnectorHttp SetUrl(string url)
        {
            this.url = url.TrimEnd('/');
            return this;
        }

        public IConnectorHttp SetTimeOut(int timeout)
        {
            this.timeout = timeout;
            return this;
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
            CHttpWebRequestEntity request = new CHttpWebRequestEntity(url + action , method);
            queue.Enqueue(request);
        }

        public void Restful(ERestful method, string action , WWWForm form)
        {
            CHttpWebRequestEntity request = new CHttpWebRequestEntity(url + action, method);
            request.SetBody(form.data).SetContentType("application/x-www-form-urlencoded");
            queue.Enqueue(request);
        }

        public void Restful(ERestful method , string action , byte[] body)
        {
            CHttpWebRequestEntity request = new CHttpWebRequestEntity(url + action, method);
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
                    CHttpWebRequestEntity request;
                    while (queue.Count > 0)
                    {
                        request = queue.Dequeue();
                        request.SetContainer(cookieContainer);
                        request.SetHeader(headers);
                        request.SetTimeOut(timeout).SetReadWriteTimeOut(timeout);

                        yield return request.Send();

                        var args = new CHttpRequestEventArgs(request);
                        FDispatcher.Instance.Event.Trigger(TypeGuid, this, args);
                        FDispatcher.Instance.Event.Trigger(GetType().ToString(), this, args);
                        FDispatcher.Instance.Event.Trigger(typeof(IConnectorHttp).ToString(), this, args);
                        
                    }
                }
                yield return new WaitForEndOfFrame();
            }

        }

    }

}