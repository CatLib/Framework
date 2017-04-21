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
using CatLib.API.Network;

namespace CatLib.Network
{
    /// <summary>
    /// Socket响应参数
    /// </summary>
    public class SocketResponseEventArgs : EventArgs, IResponse
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        public byte[] Response { get; protected set; }

        /// <summary>
        /// 构建一个Socket响应参数
        /// </summary>
        /// <param name="bytes">字节数据</param>
        public SocketResponseEventArgs(byte[] bytes)
        {
            Response = bytes;
        }
    }
}