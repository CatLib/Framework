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
using System.Net;
using System.Text;

namespace CatLib.Network
{
    /// <summary>
    /// 请求响应数据
    /// </summary>
    public sealed class HttpWebRequestResponse : IEnumerator
    {
        /// <summary>
        /// 请求
        /// </summary>
        private readonly System.Net.HttpWebRequest webRequest;

        /// <summary>
        /// 请求
        /// </summary>
        public System.Net.HttpWebRequest WebRequest
        {
            get { return webRequest; }
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        private volatile bool isDone;

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsDone
        {
            get { return isDone; }
        }

        /// <summary>
        /// 是否是错误的
        /// </summary>
        private volatile bool isError;

        /// <summary>
        /// 是否发生了错误
        /// </summary>
        public bool IsError
        {
            get { return isError; }
        }

        /// <summary>
        /// 错误描述字符串
        /// </summary>
        private volatile string error;

        /// <summary>
        /// 错误描述字符串
        /// </summary>
        public string Error
        {
            get { return error; }
        }

        /// <summary>
        /// 响应代码
        /// </summary>
        private volatile int responseCode;

        /// <summary>
        /// 响应代码
        /// </summary>
        public int ResponseCode
        {
            get { return responseCode; }
        }

        /// <summary>
        /// 请求字节
        /// </summary>
        private byte[] requestBytes;

        /// <summary>
        /// 响应的字节
        /// </summary>
        private byte[] responseBytes;

        /// <summary>
        /// 响应的字符串内容
        /// </summary>
        public string Text
        {
            get { return responseBytes != null ? Encoding.UTF8.GetString(responseBytes) : string.Empty; }
        }

        /// <summary>
        /// 响应的字节
        /// </summary>
        public byte[] Bytes
        {
            get { return responseBytes; }
        }

        /// <summary>
        /// 响应数据
        /// </summary>
        private List<byte> responseLst = new List<byte>();

        /// <summary>
        /// 构建一个响应实例
        /// </summary>
        /// <param name="request">请求</param>
        public HttpWebRequestResponse(System.Net.HttpWebRequest request)
        {
            webRequest = request;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void Send()
        {
            var requestState = new HttpRequestState { Request = webRequest };

            if (requestBytes != null)
            {
                webRequest.BeginGetRequestStream(RequestStreamData, requestState);
            }
            else
            {
                webRequest.BeginGetResponse(ReceivedData, requestState);
            }
        }

        /// <summary>
        /// 设定请求字节数据
        /// </summary>
        /// <param name="bytes">请求字节</param>
        public void SetRequestBytes(byte[] bytes)
        {
            requestBytes = bytes;
        }

        /// <summary>
        /// 用于协程
        /// </summary>
        public object Current
        {
            get { return 0; }
        }

        /// <summary>
        /// 用于协程
        /// </summary>
        public bool MoveNext()
        {
            return !IsDone;
        }

        /// <summary>
        /// 用于协程
        /// </summary>
        public void Reset() { }

        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="asyncResult">异步结果</param>
        private void RequestStreamData(IAsyncResult asyncResult)
        {
            try
            {
                var requestState = (HttpRequestState)asyncResult.AsyncState;
                var httpWebRequest = requestState.Request;

                var stream = httpWebRequest.EndGetRequestStream(asyncResult);
                stream.Write(requestBytes, 0, requestBytes.Length);
                stream.Close();

                webRequest.BeginGetResponse(ReceivedData, requestState);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                isDone = true;
                isError = true;
                responseCode = 0;
            }
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="asyncResult">异步结果</param>
        private void ReceivedData(IAsyncResult asyncResult)
        {
            try
            {
                var requestState = (HttpRequestState)asyncResult.AsyncState;
                var httpWebRequest = requestState.Request;

                requestState.Response = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
                var responseStream = requestState.Response.GetResponseStream();
                requestState.StreamResponse = responseStream;

                responseStream.BeginRead(requestState.BufferRead, 0, HttpRequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                isDone = true;
                isError = true;
                responseCode = 0;
            }
        }

        /// <summary>
        /// 读取回调
        /// </summary>
        /// <param name="asyncResult">异步结果</param>
        private void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                var requestState = (HttpRequestState)asyncResult.AsyncState;
                var responseStream = requestState.StreamResponse;
                var read = responseStream.EndRead(asyncResult);

                if (read > 0)
                {
                    responseLst.AddRange(requestState.BufferRead);
                    responseStream.BeginRead(requestState.BufferRead, 0, HttpRequestState.BUFFER_SIZE, ReadCallBack, requestState);
                }
                else
                {
                    responseCode = (int)requestState.Response.StatusCode;
                    responseBytes = responseLst.ToArray();
                    responseLst.Clear();
                    responseLst = null;
                    isDone = true;
                    isError = (responseCode < 200 || responseCode >= 300);
                    responseStream.Close();
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                isDone = true;
                isError = true;
                responseCode = 0;
            }
        }
    }
}