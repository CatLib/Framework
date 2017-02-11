using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CatLib.Support;
using System.IO;

namespace CatLib.Network.HttpWebRequest
{

    public class CHttpWebRequestResponse : IEnumerator
    {

        private System.Net.HttpWebRequest webRequest;

        private bool isDone = false;
        public bool IsDone { get { return isDone; } }

        private bool isError = false;
        public bool IsError { get { return isError; } }

        private int responseCode = 0;
        public int ResponseCode { get { return responseCode; } }

        public string Text
        {
            get
            {
                return Encoding.UTF8.GetString(responseBytes);
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

        public CHttpWebRequestResponse(System.Net.HttpWebRequest request)
        {

            webRequest = request;

            CHttpRequestState requestState = new CHttpRequestState();
            requestState.Request = request;

            webRequest.BeginGetResponse(this.ReceivedData, requestState);

        }

        public object Current { get { return 0; } }

        public bool MoveNext() { return !IsDone; }

        public void Reset() { return; }

        private void ReceivedData(IAsyncResult asyncResult)
        {
            try
            {

                CHttpRequestState requestState = (CHttpRequestState)asyncResult.AsyncState;

                System.Net.HttpWebRequest httpWebRequest = requestState.Request;

                requestState.Response = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
                Stream responseStream = requestState.Response.GetResponseStream();
                requestState.StreamResponse = responseStream;

                IAsyncResult asynchronousInputRead = responseStream.BeginRead(requestState.BufferRead, 0, CHttpRequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);

            }catch
            {
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
                    if (responseCode != (int)HttpStatusCode.OK) { isError = true; }
                    responseStream.Close();
                }

            }
            catch(System.Exception ex)
            {
                isDone = true;
                isError = true;
                responseCode = 0;
            }

        }

    }

}