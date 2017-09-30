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
using System.IO;
using CatLib.API.Network;

namespace CatLib.Network
{
    /// <summary>
    /// 接收状态
    /// </summary>
    internal sealed class ReceiveState
    {
        /// <summary>
        /// 数据流
        /// </summary>
        private readonly MemoryStream stream;

        /// <summary>
        /// 包长
        /// </summary>
        private int packetLength;

        /// <summary>
        /// 解包器
        /// </summary>
        private readonly IPacker packer;

        /// <summary>
        /// 接收状态
        /// </summary>
        public ReceiveState(IPacker packer)
        {
            stream = new MemoryStream(1024 * 64);
            packetLength = 0;
            this.packer = packer;
        }

        /// <summary>
        /// 写入数据，通过返回值可以判断是否符合解包条件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="ex">异常</param>
        /// <returns>解包后的数据，如果返回null则代表没有解包</returns>
        public object Input(byte[] data, out Exception ex)
        {
            ex = null;

            if (stream.Position != stream.Length)
            {
                stream.Seek(stream.Length, SeekOrigin.Begin);
            }

            stream.Write(data, 0, data.Length);

            if (packetLength <= 0)
            {
                var receiveData = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(receiveData, 0, receiveData.Length);
                packetLength = Math.Max(packer.Input(receiveData, out ex), 0);
                if (ex != null)
                {
                    return null;
                }
            }

            return packetLength < stream.Length ? null : Decode(out ex);
        }

        /// <summary>
        /// 重置接收状态
        /// </summary>
        public void Reset()
        {
            stream.SetLength(0);
            packetLength = 0;
        }

        /// <summary>
        /// 尝试进行解包
        /// </summary>
        /// <param name="ex">解包发生的异常</param>
        /// <returns>解包的数据</returns>
        private object Decode(out Exception ex)
        {
            var data = new byte[packetLength];
            stream.Seek(0, SeekOrigin.Begin);
            var read = stream.Read(data, 0, data.Length);

            if (read < data.Length)
            {
                var migrateData = new byte[data.Length - read];
                stream.Read(migrateData, 0, migrateData.Length);
                Reset();
                stream.Write(migrateData, 0, migrateData.Length);
            }
            else
            {
                Reset();
            }

            var packet = packer.Decode(data, out ex);

            return ex != null ? null : packet;
        }
    }
}
