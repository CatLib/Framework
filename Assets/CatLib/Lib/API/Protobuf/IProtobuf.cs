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

namespace CatLib.API.Protobuf{

    public interface IProtobuf{

        byte[] Serializers<T>(T proto);

        T UnSerializers<T>(byte[] data);

        object UnSerializers(byte[] data, Type type);

    }

}