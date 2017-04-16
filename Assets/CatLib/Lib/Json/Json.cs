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

using CatLib.API.Json;

namespace CatLib.Json
{
    /// <summary>
    /// json工具
    /// </summary>
    public sealed class Json : IJson
    {
        /// <summary>
        /// Json适配器
        /// </summary>
        private readonly IJsonAdapter adapter;

        /// <summary>
        /// 构建一个json工具
        /// </summary>
        /// <param name="adapter">适配器</param>
        public Json(IJsonAdapter adapter)
        {
            this.adapter = adapter;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列化的类型</returns>
        public T Decode<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return adapter != null ? adapter.Decode<T>(json) : default(T);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item">需要序列化的对象</param>
        /// <returns>json数据</returns>
        public string Encode(object item)
        {
            return item == null ? null : adapter.Encode(item);
        }
    }
}
