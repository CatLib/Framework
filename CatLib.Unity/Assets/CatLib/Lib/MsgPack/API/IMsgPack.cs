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
namespace CatLib.API.MsgPack
{
    public interface IMsgPack {

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dataObj">需要序列化的类</param>
        /// <returns>被序列化的数据</returns>
        byte[] Serializers<T>(T dataObj);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">解析后的类型</typeparam>
        /// <param name="data">需要被反序列化的数据</param>
        /// <returns>反序列化的结果</returns>
        T UnSerializers<T>(byte[] data);
    }
}
