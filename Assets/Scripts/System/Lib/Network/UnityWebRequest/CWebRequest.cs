using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using CatLib.Contracts.Base;
using CatLib.Container;
using CatLib.Contracts.Event;
using CatLib.Contracts.Network;
using CatLib.Base;
using CatLib.Exception;

namespace CatLib.Network.UnityWebRequest
{

    public class CWebRequest : CComponent, IConnectorHttp
    {

        /// <summary>
        /// 调度器
        /// </summary>
        [CDependency]
        public IDispatcher Dispatcher { get; set; }

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
        private Queue<UnityEngine.Networking.UnityWebRequest> queue = new Queue<UnityEngine.Networking.UnityWebRequest>();

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
        public void Post(byte[] bytes)
        {
            throw new CException("this component is not support this function");
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bytes"></param>
        public void Post(string action, byte[] bytes)
        {
            throw new CException("this component is not support this function");
        }

        public void Post(string action, Dictionary<string, string> fields)
        {
            UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Post(url + action, fields);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Post模式获取数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="form"></param>
        public void Post(string action, WWWForm form)
        {
            UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Post(url + action, form);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Get模式获取数据
        /// </summary>
        /// <param name="action"></param>
        public void Get(string action)
        {
            UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(url + action);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Put模式发送数据
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        public void Put(string action, byte[] bodyData)
        {
            UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Put(url + action, bodyData);
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
                    UnityEngine.Networking.UnityWebRequest request;
                    while (queue.Count > 0)
                    {
                        request = queue.Dequeue();
                        yield return request.Send();
                        if (request.isError || request.responseCode != 200)
                        {
                            if (Dispatcher is IEvent)
                            {
                                (Dispatcher as IEvent).Event.Trigger(TypeGuid, this, new CWebRequestErrorEventArgs(request));
                                (Dispatcher as IEvent).Event.Trigger(GetType().ToString(), this, new CWebRequestErrorEventArgs(request));
                            }
                        }
                        else
                        {
                            if (Dispatcher is IEvent)
                            {
                                (Dispatcher as IEvent).Event.Trigger(TypeGuid, this, new CWebRequestEventArgs(request));
                                (Dispatcher as IEvent).Event.Trigger(GetType().ToString(), this, new CWebRequestEventArgs(request));
                            }
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }

        }

    }

}