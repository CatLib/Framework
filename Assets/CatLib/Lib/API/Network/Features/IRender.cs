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

namespace CatLib.API.Network
{
    /// <summary>
    /// 数据渲染流
    /// </summary>
    public interface IRender
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bufferBuilder">流</param>
        void Decode(IBufferBuilder bufferBuilder);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="bufferBuilder">流</param>
        void Encode(IBufferBuilder bufferBuilder);
    }
}