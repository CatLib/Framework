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
 
using UnityEngine;
using System.Collections.Generic;

namespace CatLib.API.Network
{
    /// <summary>
    /// 连接器
    /// </summary>
    public interface IConnectorHttp : IConnector
    {

        IConnectorHttp SetHeader(Dictionary<string, string> headers);

        IConnectorHttp AppendHeader(string header, string val);

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
