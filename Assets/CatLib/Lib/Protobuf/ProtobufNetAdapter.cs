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
        public byte[] Serializers<T>(T proto)
        {
            if (proto == null) { return null; }
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize<T>(ms, proto);
                    byte[] result = new byte[ms.Length];
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

        public object UnSerializers(byte[] data, Type type)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                object proto = ProtoBuf.Serializer.Deserialize(type, memoryStream);
                return proto;
            }
        }

    }

}