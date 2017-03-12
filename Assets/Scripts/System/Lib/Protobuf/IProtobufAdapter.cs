
using System;

namespace CatLib.Protobuf
{

    public interface IProtobufAdapter
    {

        byte[] Serializers<T>(T proto);

        object UnSerializers(byte[] data, Type type);

    }

}