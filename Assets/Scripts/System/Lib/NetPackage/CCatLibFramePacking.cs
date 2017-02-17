using CatLib.Contracts.NetPackage;
using System;
using System.Collections.Generic;
using CatLib.Support;
using CatLib.Base;

namespace CatLib.NetPackage
{
    
    /// <summary>
    /// CatLib Frame协议拆包器
    /// 协议格式为 总包长+包体，其中包长为4字节网络字节序的整数，包体可以是普通文本或者二进制数据。
    /// </summary>
    public class CCatLibFramePacking : IPacking
    {

        /// <summary>
        /// 缓冲区
        /// </summary>
        private CBuffer buffer;

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public byte[][] Decode(byte[] bytes) {

            buffer.Push(bytes);
            if(buffer.Length < 4){ return null; }

            List<byte[]> package = null;
            int totalSize;
            byte[] bodyBuffer;
            while(buffer.Length >= 4){

                totalSize = BitConverter.ToInt32(buffer.Peek(4) , 0);
                if(totalSize < buffer.Length){ break; }

                buffer.Shift(4);
                bodyBuffer = buffer.Shift(totalSize - 4);

                if(package == null){ package = new List<byte[]>(); }
                package.Add(bodyBuffer);

            }

            if(package == null){ return null; }

            return package.ToArray();

        }

        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public byte[] Encode(byte[] bytes){

            CBuffer newBuffer = bytes;
            newBuffer.Unshift(newBuffer.Length.ToString().ToByte());
            return newBuffer;

        }

        /// <summary>
        /// 清空缓存区
        /// </summary>
        public void Clear()
        {
            buffer = new CBuffer();
        }

    }
}
