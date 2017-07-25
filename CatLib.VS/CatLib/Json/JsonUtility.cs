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
using System;
using System.Collections.Generic;

namespace CatLib.Json
{
    /// <summary>
    /// Json处理器
    /// </summary>
    internal sealed class JsonUtility : IJson, IJsonAware
    {
        /// <summary>
        /// 处理器
        /// </summary>
        private IJson handler;

        /// <summary>
        /// 设定记录器实例接口
        /// </summary>
        /// <param name="handler">记录器</param>
        public void SetJson(IJson handler)
        {
            if (handler == this)
            {
                throw new InvalidOperationException("Own instance can not give itself");
            }
            this.handler = handler;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>反序列化的类型</returns>
        public T Decode<T>(string json)
        {
            GuardHandler();
            return handler.Decode<T>(json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns>反序列化的对象</returns>
        public IDictionary<string, object> Decode(string json)
        {
            GuardHandler();
            return handler.Decode(json);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item">需要序列化的对象</param>
        /// <returns>json数据</returns>
        public string Encode(object item)
        {
            GuardHandler();
            return handler.Encode(item);
        }

        /// <summary>
        /// 校验json处理器有效性
        /// </summary>
        private void GuardHandler()
        {
            if (handler == null)
            {
                throw new RuntimeException("Undefiend json handler , please call IJsonAware.SetJson");
            }
        }
    }
}