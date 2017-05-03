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
using CatLib.API.MsgPack;

namespace CatLib.MsgPack
{
    internal sealed class MsgPack : IMsgPack
    {
        private IMsgPackAdapter msgPackAdapter;

        public MsgPack(IMsgPackAdapter adapter)
        {
            if (adapter == null)
            {
                throw new System.ArgumentNullException("IMsgPackAdapter adapter is NULL");
            }
            msgPackAdapter = adapter;
        }

        public byte[] Serializers<T>(T dataObj)
        {
            return msgPackAdapter.Serializers<T>(dataObj);
        }

        public T UnSerializers<T>(byte[] data)
        {
            return msgPackAdapter.UnSerializers<T>(data);
        }
    }
}

