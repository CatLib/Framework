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
using CatLib.API.Json;

namespace CatLib.Json
{
    /// <summary>
    /// Facebook Simple Json 适配器
    /// </summary>
    internal sealed class SimpleJsonAdapter : IJson
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列化的类型</returns>
        public T Decode<T>(string json)
        {
            return (T)Decode(json, typeof(T));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="type">反序列化的类型</param>
        /// <returns>反序列化的结果</returns>
        public object Decode(string json, Type type)
        {
            return SimpleJson.SimpleJson.DeserializeObject(json, type);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item">需要序列化的对象</param>
        /// <returns>json数据</returns>
        public string Encode(object item)
        {
            return SimpleJson.SimpleJson.SerializeObject(item);
        }
    }
}
