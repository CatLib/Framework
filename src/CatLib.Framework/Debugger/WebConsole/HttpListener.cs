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
using System.Net;
using Listener = System.Net.HttpListener;

namespace CatLib.Debugger.WebConsole
{
    /// <summary>
    /// Http监听器
    /// </summary>
    internal sealed class HttpListener : IDisposable
    {
        /// <summary>
        /// 监听器
        /// </summary>
        private Listener listener;

        /// <summary>
        /// 当请求时
        /// </summary>
        public event Action<HttpListenerContext> OnRequest;

        /// <summary>
        /// 当关闭时
        /// </summary>
        public event Action OnClosed;

        /// <summary>
        /// Http监听器
        /// </summary>
        /// <param name="host">监听host</param>
        /// <param name="port">监听端口</param>
        public HttpListener(string host = "*", int port = 9478)
        {
            listener = new Listener();
            listener.Prefixes.Add("http://" + host + ":" + port + "/");
            listener.Start();
            listener.BeginGetContext(ListenedRequest, null);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }
        }

        /// <summary>
        /// 监听到请求时
        /// </summary>
        private void ListenedRequest(IAsyncResult result)
        {
            try
            {
                var context = listener.EndGetContext(result);
                if (OnRequest != null)
                {
                    try
                    {
                        OnRequest.Invoke(context);
                    }
                    catch { }
                }
                listener.BeginGetContext(ListenedRequest, null);
            }
            catch (Exception)
            {
                if(OnClosed != null)
                {
                    OnClosed.Invoke();
                }
            }
        }
    }
}
