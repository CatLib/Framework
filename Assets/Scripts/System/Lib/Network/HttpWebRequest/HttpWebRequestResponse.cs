using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;

namespace CatLib.Network
{

    public class HttpWebRequestResponse : IEnumerator
    {

        private System.Net.HttpWebRequest webRequest;
        public System.Net.HttpWebRequest WebRequest { get { return webRequest; } }

        private volatile bool isDone = false;
        public bool IsDone { get { return isDone; } }

        private volatile bool isError = false;
        public bool IsError { get { return isError; } }

        private volatile string error;
        public string Error { get { return error; } }

        private volatile int responseCode = 0;
        public int ResponseCode { get { return responseCode; } }

        private byte[] requestBytes;

        public string Text
        {
            get
            {
                if (responseBytes != null)
                {
                    return Encoding.UTF8.GetString(responseBytes);
                }
                return string.Empty;
            }
        }

        public byte[] Bytes
        {
            get
            {
                return responseBytes;
            }
        }

        private byte[] responseBytes;
        private List<byte> responseLst = new List<byte>();

        public HttpWebRequestResponse(System.Net.HttpWebRequest request)
        {
            webRequest = request;
        }

        public void Send()
        {
            HttpRequestState requestState = new HttpRequestState();
            requestState.Request = webRequest;

            if (requestBytes != null)
            {
                webRequest.BeginGetRequestStream(RequestStreamData, requestState);
            }else
            {
                webRequest.BeginGetResponse(ReceivedData, requestState);
            }
        }

        public void SetRequestBytes(byte[] bytes)
        {
            requestBytes = bytes;
        }

        public object Current { get { return 0; } }

        public bool MoveNext() { return !IsDone; }

        public void Reset() { return; }

        private void RequestStreamData(IAsyncResult asyncResult)
        {
            try
            {
                HttpRequestState requestState = (HttpRequestState)asyncResult.AsyncState;
                System.Net.HttpWebRequest httpWebRequest = requestState.Request;

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

        private void ReceivedData(IAsyncResult asyncResult)
        {
            try
            {

                HttpRequestState requestState = (HttpRequestState)asyncResult.AsyncState;

                System.Net.HttpWebRequest httpWebRequest = requestState.Request;

                requestState.Response = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
                Stream responseStream = requestState.Response.GetResponseStream();
                requestState.StreamResponse = responseStream;

                responseStream.BeginRead(requestState.BufferRead, 0, HttpRequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);

            }catch(Exception ex)
            {
                error = ex.Message;
                isDone = true;
                isError = true;
                responseCode = 0;
            }

        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {

            try
            {
                HttpRequestState requestState = (HttpRequestState)asyncResult.AsyncState;
                Stream responseStream = requestState.StreamResponse;
                int read = responseStream.EndRead(asyncResult);

                if (read > 0)
                {
                    responseLst.AddRange(requestState.BufferRead);
                    responseStream.BeginRead(requestState.BufferRead, 0, HttpRequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);
                }
                else
                {
                    responseCode = (int)requestState.Response.StatusCode;
                    responseBytes = responseLst.ToArray();
                    responseLst.Clear();
                    responseLst = null;
                    isDone = true;
                    isError = false;
                    if (responseCode < 200 || responseCode >= 300) { isError = true; }
                    responseStream.Close();
                }

            }
            catch(Exception ex)
            {
                error = ex.Message;
                isDone = true;
                isError = true;
                responseCode = 0;
            }

        }

    }

}