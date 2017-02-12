using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System.Net;
using CatLib.Network;

namespace CatLib.Contracts.Network
{
    /// <summary>
    /// 连接器
    /// </summary>
    public interface IConnectorHttp : IConnector
    {

        IConnectorHttp SetUrl(string url);

        IConnectorHttp SetHeader(Dictionary<string, string> headers);

        IConnectorHttp AppendHeader(string header, string val);

        IConnectorHttp SetTimeOut(int timeout);

        void Restful(ERestful method, string action);

        void Restful(ERestful method, string action, WWWForm form);

        void Restful(ERestful method, string action, byte[] body);

        void Get(string action);

        void Head(string action);

        void Post(string action, WWWForm form);

        void Post(string action, byte[] body);

        void Put(string action, WWWForm form);

        void Put(string action, byte[] body);

        void Delete(string action);

    }
}
