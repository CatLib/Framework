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

using TinyJson;

namespace CatLib.Json
{
    public class TinyJsonAdapter : IJsonAdapter
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列化的类型</returns>
        public T Decode<T>(string json)
        {
            return JSONParser.FromJson<T>(json);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item">需要序列化的对象</param>
        /// <returns>json数据</returns>
        public string Encode(object item)
        {
            return JSONWriter.ToJson(item);
        }
    }
}