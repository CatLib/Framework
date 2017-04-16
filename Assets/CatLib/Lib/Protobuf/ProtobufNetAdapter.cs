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

using System.IO;
using System;

namespace CatLib.Protobuf
{
    /// <summary>
    /// Protobuf-net 版本的protobuf适配器
    /// </summary>
    public class ProtobufNetAdapter : IProtobufAdapter
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="proto">需要序列化的类</param>
        /// <returns>被序列化的数据</returns>
        public byte[] Serializers<T>(T proto)
        {
            if (proto == null)
            {
                return null;
            }
            try
            {
                using (var ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, proto);
                    var result = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">需要被反序列化的数据</param>
        /// <param name="type">反序列化的类型</param>
        /// <returns>反序列化的结果</returns>
        public object UnSerializers(byte[] data, Type type)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                var proto = ProtoBuf.Serializer.Deserialize(type, memoryStream);
                return proto;
            }
        }
    }
}