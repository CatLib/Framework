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

namespace CatLib.API.Protobuf
{
    /// <summary>
    /// Protobuf
    /// </summary>
    public interface IProtobuf
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="proto">需要序列化的类</param>
        /// <returns>被序列化的数据</returns>
        byte[] Serializers<T>(T proto);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">解析后的类型</typeparam>
        /// <param name="data">需要被反序列化的数据</param>
        /// <returns>反序列化的结果</returns>
        T UnSerializers<T>(byte[] data);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">需要被反序列化的数据</param>
        /// <param name="type">反序列化的类型</param>
        /// <returns>反序列化的结果</returns>
        object UnSerializers(byte[] data, Type type);
    }
}