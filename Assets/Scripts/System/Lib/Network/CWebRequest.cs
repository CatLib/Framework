using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using CatLib.Contracts.Base;
using CatLib.Container;
using CatLib.Contracts.Event;
using CatLib.Contracts.Network;
using CatLib.Base;

namespace CatLib.Network
{

    public class CWebRequest : CComponent, IConnectorShort
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
        private Queue<UnityWebRequest> queue = new Queue<UnityWebRequest>();

        /// <summary>
        /// 设定服务器访问地址
        /// </summary>
        /// <param name="url"></param>
        public void SetUrl(string url)
        {
            this.url = url.TrimEnd('/');
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes)
        {
            SendTo(url, bytes);
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bytes"></param>
        public void Send(string action, byte[] bytes)
        {
            SendTo(url + "/" + action, bytes);
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="action"></param>
        private void SendTo(string url , byte[] bytes)
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("data", bytes);
            UnityWebRequest request = UnityWebRequest.Post(url, form);
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
                    UnityWebRequest request;
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