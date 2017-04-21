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
 
namespace CatLib.API.Buffer
{
    /// <summary>
    /// Buffer构建器
    /// </summary>
    public interface IBufferBuilder
    {
        /// <summary>
        /// 获取或者设定构建器内的byte[]
        /// </summary>
        byte[] Byte { get; set; }

        /// <summary>
        /// 返回某个指定的字节序值在构建器中首次出现的位置
        /// </summary>
        /// <param name="data">指定的字节序</param>
        /// <returns>如果要检索的字节序值没有出现，则该方法返回 -1</returns>
        int IndexOf(params byte[] data);

        /// <summary>
        /// 在构建器尾部推入数据
        /// </summary>
        /// <param name="data">推入的字节序</param>
        void Push(byte[] data);

        /// <summary>
        /// 在字节序尾部弹出数据
        /// </summary>
        /// <param name="count">弹出字节的长度</param>
        /// <returns>弹出的字节序</returns>
        byte[] Pop(int count = 1);

        /// <summary>
        /// 在构建器头部推入数据
        /// </summary>
        /// <param name="data">推入的字节序</param>
        void Unshift(byte[] data);

        /// <summary>
        /// 在构建器头部弹出数据
        /// </summary>
        /// <param name="count">弹出字节的长度</param>
        /// <returns>弹出的字节序</returns>
        byte[] Shift(int count = 1);

        /// <summary>
        /// 获取构建器头部的数据但是不推出它
        /// </summary>
        /// <param name="count">字节的长度</param>
        /// <returns>字节序</returns>
        byte[] Peek(int count = 1);

        /// <summary>
        /// 清空构建器
        /// </summary>
        int Length { get; }

        /// <summary>
        /// 构建器的长度
        /// </summary>
        void Clear();
    }
}