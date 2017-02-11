using CatLib.Base;
using CatLib.Contracts.Network;
using CatLib.Exception;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace CatLib.Network.HttpWebRequest
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
        /// 发送队列
        /// </summary>
        private Queue<CHttpWebRequestEntity> queue = new Queue<CHttpWebRequestEntity>();

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
            CHttpWebRequestEntity request = CHttpWebRequestEntity.Post(url, bytes);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bytes"></param>
        public void Post(string action, byte[] bytes)
        {
            CHttpWebRequestEntity request = CHttpWebRequestEntity.Post(url + action, bytes);
            queue.Enqueue(request);
        }

        public void Post(string action, Dictionary<string, string> fields)
        {
            CHttpWebRequestEntity request = CHttpWebRequestEntity.Post(url + action, fields);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Post模式获取数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="form"></param>
        public void Post(string action, WWWForm form)
        {
            CHttpWebRequestEntity request = CHttpWebRequestEntity.Post(url + action, form);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Get模式获取数据
        /// </summary>
        /// <param name="action"></param>
        public void Get(string action)
        {
            CHttpWebRequestEntity request = CHttpWebRequestEntity.Get(url + action);
            queue.Enqueue(request);
        }

        /// <summary>
        /// 以Put模式发送数据
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        public void Put(string action, byte[] bodyData)
        {
            CHttpWebRequestEntity request = CHttpWebRequestEntity.Put(url + action , bodyData);
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
                    CHttpWebRequestEntity request;
                    while (queue.Count > 0)
                    {
                        request = queue.Dequeue();
                        request.SetContainer(cookieContainer);
                        yield return request.Send();
                        if (request.Response.IsError || request.Response.ResponseCode != (int)HttpStatusCode.OK)
                        {
                            FDispatcher.Instance.Event.Trigger(TypeGuid, this, new CHttpRequestErrorEventArgs(request));
                            FDispatcher.Instance.Event.Trigger(GetType().ToString(), this, new CHttpRequestErrorEventArgs(request));

                        }
                        else
                        {
                            FDispatcher.Instance.Event.Trigger(TypeGuid, this, new CHttpRequestEventArgs(request));
                            FDispatcher.Instance.Event.Trigger(GetType().ToString(), this, new CHttpRequestEventArgs(request));
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }

        }

    }

}