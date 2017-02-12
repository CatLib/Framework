using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CatLib.Support;
using System.IO;

namespace CatLib.Network
{

    public class CHttpWebRequestResponse : IEnumerator
    {

        private HttpWebRequest webRequest;
        public HttpWebRequest WebRequest { get { return webRequest; } }

        private bool isDone = false;
        public bool IsDone { get { return isDone; } }

        private bool isError = false;
        public bool IsError { get { return isError; } }

        private string error;
        public string Error { get { return error; } }

        private int responseCode = 0;
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

        public CHttpWebRequestResponse(HttpWebRequest request)
        {
            webRequest = request;
        }

        public void Send()
        {
            CHttpRequestState requestState = new CHttpRequestState();
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
                CHttpRequestState requestState = (CHttpRequestState)asyncResult.AsyncState;
                HttpWebRequest httpWebRequest = requestState.Request;

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

                CHttpRequestState requestState = (CHttpRequestState)asyncResult.AsyncState;

                HttpWebRequest httpWebRequest = requestState.Request;

                requestState.Response = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
                Stream responseStream = requestState.Response.GetResponseStream();
                requestState.StreamResponse = responseStream;

                IAsyncResult asynchronousInputRead = responseStream.BeginRead(requestState.BufferRead, 0, CHttpRequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);

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
                CHttpRequestState requestState = (CHttpRequestState)asyncResult.AsyncState;
                Stream responseStream = requestState.StreamResponse;
                int read = responseStream.EndRead(asyncResult);

                if (read > 0)
                {
                    responseLst.AddRange(requestState.BufferRead);
                    IAsyncResult asynchronousResult = responseStream.BeginRead(requestState.BufferRead, 0, CHttpRequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);
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