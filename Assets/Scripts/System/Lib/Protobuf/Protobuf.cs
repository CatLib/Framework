using System;
using System.IO;
using CatLib.API.Protobuf;

namespace CatLib.Protobuf
{

    public class Protobuf : IProtobuf
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

        public T UnSerializers<T>(byte[] data)
        {
            return (T)UnSerializers(data, typeof(T));
        }

        public object UnSerializers(byte[] data , Type type)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                object proto = ProtoBuf.Serializer.Deserialize(type , memoryStream);
                return proto;
            }
        }

        
    }

}