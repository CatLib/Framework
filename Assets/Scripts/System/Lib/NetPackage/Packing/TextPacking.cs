using System.Collections.Generic;
using CatLib.Support;
using CatLib.Contracts.Network;


namespace CatLib.NetPackage
{

    /// <summary>
    /// CatLib 文本协议拆包器
    /// 协议格式为 数据包+换行符(\r\n)，即在每个数据包末尾加上一个换行符表示包的结束
    /// </summary>
    public class TextPacking : IPacking
    {

        /// <summary>
        /// 缓冲区
        /// </summary>
        private BufferBuilder buffer;

        /// <summary>
        /// 换行符
        /// </summary>
        private byte[] lineFeed = new byte[] { 13, 10 };

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public byte[][] Decode(byte[] bytes)
        {

            buffer.Push(bytes);

            int indexOf = 0;
            byte[] bodyBuffer;
            List<byte[]> package = null;

            while (true)
            {
                indexOf = buffer.IndexOf(lineFeed);
                if (indexOf < 0) { break; }
                if(indexOf == 0) { buffer.Shift(2); continue; }

                bodyBuffer = buffer.Shift(indexOf);

                if (package == null) { package = new List<byte[]>(); }
                package.Add(bodyBuffer);
            }

            if (package == null) { return null; }

            return package.ToArray();

        }

        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public byte[] Encode(byte[] bytes)
        {

            BufferBuilder newBuffer = bytes;
            newBuffer.Push(lineFeed);
            return newBuffer;

        }

        /// <summary>
        /// 清空缓存区
        /// </summary>
        public void Clear()
        {
            buffer = new BufferBuilder();
        }

    }
}
