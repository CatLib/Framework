using CatLib.Contracts.NetPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatLib.NetPackge
{
    
    /// <summary>
    /// CatLib 网络帧协议拆包器
    /// </summary>
    public class CCatLibFramePacking : IPacking
    {

        /// <summary>
        /// 缓冲区
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public IPackage[] Decode(byte[] bytes) {

            if (buffer == null) {
                buffer = bytes;
            }else {
                var newBuffer = new byte[buffer.Length + bytes.Length];
                Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
                Buffer.BlockCopy(bytes, 0, newBuffer, buffer.Length, bytes.Length);
                buffer = newBuffer;
            }
            List<IPackage> package = null;
            while (true) {

                int bodySize = 0;
                if (buffer == null) { break; }
                if (buffer.Length > 4)
                {
                    bodySize = BitConverter.ToInt32(buffer, 0);
                    if (buffer.Length - 4 < bodySize) { break; }
                }

                if (bodySize <= 0)
                {
                    if (buffer.Length - 4 <= 0) { break; }
                    byte[] newBuffer = new byte[buffer.Length - 4];
                    Buffer.BlockCopy(buffer, 4, newBuffer, 0, newBuffer.Length);
                    buffer = newBuffer;
                    continue;
                }

                byte[] bodyBuffer = new byte[bodySize];
                Buffer.BlockCopy(buffer, 4, bodyBuffer, 0, bodySize);

                if (buffer.Length - 4 - bodySize <= 0)
                {
                    buffer = null;
                }
                else
                {
                    byte[] newBuffer = new byte[buffer.Length - 4 - bodySize];
                    Buffer.BlockCopy(buffer, 4 + bodySize, newBuffer, 0, newBuffer.Length);
                    buffer = newBuffer;
                }

                //todo:: bodyBuffer is body byte
                UnityEngine.Debug.Log(Encoding.Default.GetString(bodyBuffer));

            }

            if(package == null)
            {
                return null;
            }

            return package.ToArray();

        }

        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public byte[] Encode(IPackage bytes){

            return null;

        }

        /// <summary>
        /// 清空缓存区
        /// </summary>
        public void Clear()
        {
            buffer = null;
        }

    }
}
