using System;
using System.IO;
using CatLib.API.Protobuf;

namespace CatLib.Protobuf
{

    public class Protobuf : IProtobuf
    {

        private IProtobufAdapter protoParse;

        public Protobuf(IProtobufAdapter adapter)
        {
            protoParse = adapter;
        }

        public byte[] Serializers<T>(T proto)
        {
            return protoParse.Serializers<T>(proto);
        }

        public T UnSerializers<T>(byte[] data)
        {
            return (T)UnSerializers(data, typeof(T));
        }

        public object UnSerializers(byte[] data , Type type)
        {
            return protoParse.UnSerializers(data , type);
        }

        
    }

}