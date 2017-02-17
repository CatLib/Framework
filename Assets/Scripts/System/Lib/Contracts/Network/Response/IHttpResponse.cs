using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Contracts.Network
{
    public interface IHttpResponse
    {

        /// <summary>
        /// 请求对象
        /// </summary>
        object Request { get; }

        /// <summary>
        /// 响应字节流的字符串形式
        /// </summary>
        string Text { get; }

        /// <summary>
        /// 是否错误
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// 错误内容
        /// </summary>
        string Error { get; }

        /// <summary>
        /// 响应代码
        /// </summary>
        long ResponseCode { get; }

        /// <summary>
        /// 谓词
        /// </summary>
        ERestful Restful { get; }

    }

}
