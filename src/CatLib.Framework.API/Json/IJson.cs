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

namespace CatLib.API.Json
{
    /// <summary>
    /// Json 工具
    /// </summary>
    public interface IJson
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">反序列化的类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列化的结果</returns>
        T Decode<T>(string json);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="type">反序列化的类型</param>
        /// <returns>反序列化的结果</returns>
        object Decode(string json, Type type);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item">需要序列化的对象</param>
        /// <returns>json数据</returns>
        string Encode(object item);
    }
}
