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

using CatLib.API.Buffer;
using System;

namespace CatLib.Buffer
{
    /// <summary>
    /// Buffer构建器
    /// </summary>
    public class BufferBuilder : IBufferBuilder
    {
        /// <summary>
        /// buffer数据
        /// </summary>
        private byte[] buffer = new byte[0];

        /// <summary>
        /// 将buffer构建器转为byte
        /// </summary>
        /// <param name="buffer"></param>
        public static implicit operator byte[] (BufferBuilder buffer)
        {
            return buffer.buffer;
        }

        /// <summary>
        /// 将byte转为buffer构建器
        /// </summary>
        /// <param name="buffer">字节序</param>
        public static implicit operator BufferBuilder(byte[] buffer)
        {
            var obj = new BufferBuilder();
            obj.Push(buffer);
            return obj;
        }

        /// <summary>
        /// 获取或者设定构建器内的byte[]
        /// </summary>
        public byte[] Byte
        {
            get
            {
                return buffer;
            }
            set
            {
                if (value == null)
                {
                    buffer = new byte[0];
                    return;
                }
                buffer = value;
            }
        }

        /// <summary>
        /// 返回某个指定的字节序值在构建器中首次出现的位置
        /// </summary>
        /// <param name="data">指定的字节序</param>
        /// <returns>如果要检索的字节序值没有出现，则该方法返回 -1</returns>
        public int IndexOf(params byte[] data)
        {
            if (data.Length <= 0) { return -1; }

            bool isFinded;
            for (int i = 0, n; i < buffer.Length; i++)
            {
                if (buffer[i] != data[0]) { continue; }
                isFinded = true;
                for (n = 0; n < data.Length; n++)
                {
                    if (buffer[i + n] == data[n]) { continue; }
                    isFinded = false;
                    break;
                }
                if (isFinded) { return i; }
            }

            return -1;
        }

        /// <summary>
        /// 在构建器尾部推入数据
        /// </summary>
        /// <param name="data">推入的字节序</param>
        public void Push(byte[] data)
        {
            var newBuffer = new byte[buffer.Length + data.Length];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
            System.Buffer.BlockCopy(data, 0, newBuffer, buffer.Length, data.Length);
            buffer = newBuffer;
        }

        /// <summary>
        /// 在字节序尾部弹出数据
        /// </summary>
        /// <param name="count">弹出字节的长度</param>
        /// <returns>弹出的字节序</returns>
        public byte[] Pop(int count = 1)
        {
            count = Math.Max(1, count);
            if (count > buffer.Length) { throw new ArgumentOutOfRangeException("count"); }

            var returnBuffer = new byte[count];
            System.Buffer.BlockCopy(buffer, buffer.Length - returnBuffer.Length, returnBuffer, 0, returnBuffer.Length);

            var newBuffer = new byte[buffer.Length - returnBuffer.Length];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, newBuffer.Length);

            return returnBuffer;
        }

        /// <summary>
        /// 在构建器头部推入数据
        /// </summary>
        /// <param name="data">推入的字节序</param>
        public void Unshift(byte[] data)
        {
            var newBuffer = new byte[buffer.Length + data.Length];
            System.Buffer.BlockCopy(data, 0, newBuffer, 0, data.Length);
            System.Buffer.BlockCopy(buffer, 0, newBuffer, data.Length, buffer.Length);
            buffer = newBuffer;
        }

        /// <summary>
        /// 在构建器头部弹出数据
        /// </summary>
        /// <param name="count">弹出字节的长度</param>
        /// <returns>弹出的字节序</returns>
        public byte[] Shift(int count = 1)
        {
            count = Math.Max(1, count);
            var returnBuffer = Peek(count);
            var newBuffer = new byte[buffer.Length - count];
            System.Buffer.BlockCopy(buffer, count, newBuffer, 0, newBuffer.Length);
            buffer = newBuffer;
            return returnBuffer;
        }

        /// <summary>
        /// 获取构建器头部的数据但是不推出它
        /// </summary>
        /// <param name="count">字节的长度</param>
        /// <returns>字节序</returns>
        public byte[] Peek(int count = 1)
        {
            count = Math.Max(1, count);
            if (count > buffer.Length) { throw new ArgumentOutOfRangeException("count"); }

            var newBuffer = new byte[count];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, newBuffer.Length);
            return newBuffer;
        }

        /// <summary>
        /// 清空构建器
        /// </summary>
        public void Clear()
        {
            buffer = new byte[0];
        }

        /// <summary>
        /// 构建器的长度
        /// </summary>
        public int Length
        {
            get
            {
                return buffer.Length;
            }
        }
    }
}