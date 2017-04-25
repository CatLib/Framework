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
using CatLib.API.Protobuf;

namespace CatLib.Protobuf
{
    /// <summary>
    /// Protobuf
    /// </summary>
    public sealed class Protobuf : IProtobuf
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private readonly IProtobufAdapter adapter;

        /// <summary>
        /// Protobuf
        /// </summary>
        /// <param name="adapter">适配器</param>
        public Protobuf(IProtobufAdapter adapter)
        {
            this.adapter = adapter;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="proto">需要序列化的类</param>
        /// <returns>被序列化的数据</returns>
        public byte[] Serializers<T>(T proto)
        {
            return adapter.Serializers(proto);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">解析后的类型</typeparam>
        /// <param name="data">需要被反序列化的数据</param>
        /// <returns>反序列化的结果</returns>
        public T UnSerializers<T>(byte[] data)
        {
            return (T)UnSerializers(data, typeof(T));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">需要被反序列化的数据</param>
        /// <param name="type">反序列化的类型</param>
        /// <returns>反序列化的结果</returns>
        public object UnSerializers(byte[] data, Type type)
        {
            return adapter.UnSerializers(data, type);
        }
    }
}