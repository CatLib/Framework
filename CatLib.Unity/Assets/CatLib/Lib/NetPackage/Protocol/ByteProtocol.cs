﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using CatLib.API.Network;

namespace CatLib.NetPackage
{

    public class ByteProtocol : IProtocol
    {

        /// <summary>
        /// 协议反序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public IPackage Decode(byte[] bytes)
        {

            return new BasePackage(bytes);

        }

        /// <summary>
        /// 协议序列化
        /// </summary>
        /// <param name="package">协议包</param>
        /// <returns></returns>
        public byte[] Encode(IPackage package)
        {

            return package.ToByte();

        }

    }

}
