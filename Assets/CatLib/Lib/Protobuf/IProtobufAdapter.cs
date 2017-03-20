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

namespace CatLib.Protobuf
{

    public interface IProtobufAdapter
    {

        byte[] Serializers<T>(T proto);

        object UnSerializers(byte[] data, Type type);

    }

}