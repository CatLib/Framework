/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
using System;
using System.IO;
using MsgPack.Serialization;

namespace CatLib.MsgPack
{
    public sealed class MsgPackAdapter : IMsgPackAdapter
    {

        public MsgPackAdapter()
        {
            
        }

        public byte[] Serializers<T>(T dataObj)
        {
            var serializer = MessagePackSerializer.Get<T>();
            var stream = new MemoryStream();

            serializer.Pack(stream, dataObj);

            return stream.ToArray();
        }

        public T UnSerializers<T>(byte[] data)
        {
            var serializer = MessagePackSerializer.Get<T>();
            var stream = new MemoryStream(data);
            object dataObj = serializer.Unpack(stream);

            return (T)dataObj;
        }
    }
}

