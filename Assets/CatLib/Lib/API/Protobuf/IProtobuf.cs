
using System;

namespace CatLib.API.Protobuf{

    public interface IProtobuf{

        byte[] Serializers<T>(T proto);

        T UnSerializers<T>(byte[] data);

        object UnSerializers(byte[] data, Type type);

    }

}