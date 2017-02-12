using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using CatLib.Contracts.Network;
using CatLib.Base;
using CatLib.Support;
using System.Net;

namespace CatLib.Network
{

    public class CWebRequest : CComponent, IConnectorHttp
    {

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
        private Queue<UnityWebRequest> queue = new Queue<UnityWebRequest>();

        private Dictionary<string, string> headers;
        public Dictionary<string, string> Headers { get { return headers; } }

        public IConnectorHttp SetUrl(string url)
        {
            this.url = url.TrimEnd('/');
            return this;
        }

        public IConnectorHttp SetHeader(Dictionary<string, string> headers)
        {
            this.headers = headers;
            return this;
        }

        public IConnectorHttp SetTimeOut(int timeout)
        {
            throw new CException("this component is not support this features");
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
                default: throw new CException("this component is not support [" + method.ToString() + "] restful");
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
                    UnityWebRequest request;
                    while (queue.Count > 0)
                    {
                        request = queue.Dequeue();
                        if (headers != null)
                        {
                            headers.Walk((statu, val) => request.SetRequestHeader(statu.ToString(), val));
                        }
                        yield return request.Send();
                        var args = new CWebRequestEventArgs(request);

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