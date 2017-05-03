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

using System.IO;
using System.Net;

namespace CatLib.Network
{
    /// <summary>
    /// Http请求状态
    /// </summary>
    public sealed class HttpRequestState
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public const int BUFFER_SIZE = 1024;

        /// <summary>
        /// 读取的字节
        /// </summary>
        public byte[] BufferRead;

        /// <summary>
        /// 请求对象
        /// </summary>
        public System.Net.HttpWebRequest Request;

        /// <summary>
        /// 响应对象
        /// </summary>
        public HttpWebResponse Response;

        /// <summary>
        /// 响应流
        /// </summary>
        public Stream StreamResponse;

        /// <summary>
        /// 构建一个http请求状态
        /// </summary>
        public HttpRequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            Request = null;
            StreamResponse = null;
        }
    }
}