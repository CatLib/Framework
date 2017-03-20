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