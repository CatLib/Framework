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
using System.Collections.Generic;
using CatLib.API.Config;
using CatLib.API.Container;

namespace CatLib.Config
{
    /// <summary>
    /// 配置容器
    /// </summary>
    public sealed class ConfigStore : IConfigStore
    {
        /// <summary>
        /// 服务容器
        /// </summary>
        [Dependency]
        public IContainer App { get; set; }

        /// <summary>
        /// 配置字典，key为指定类型，其中的值为配置的键值对
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, object>> configs;

        /// <summary>
        /// 构造一个配置容器
        /// </summary>
        public ConfigStore()
        {
            configs = new Dictionary<string, Dictionary<string, object>>();
        }

        /// <summary>
        /// 增加配置
        /// </summary>
        /// <param name="config">配置信息</param>
        public void AddConfig(IConfig config)
        {
            configs.Remove(config.Name.ToString());
            configs.Add(config.Name.ToString(), ParseConfig(config));
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置最终转换到的类型</typeparam>
        /// <param name="type">配置所属类型</param>
        /// <param name="field">配置字段</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        public T Get<T>(Type type, string field, T def = default(T))
        {
            return Get(type.ToString(), field, def);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置最终转换到的类型</typeparam>
        /// <param name="name">配置所属类型的名字</param>
        /// <param name="field">配置的字段名</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        public T Get<T>(string name, string field, T def = default(T))
        {
            try
            {
                var obj = Get(name, field, (object)def);
                if (obj == null)
                {
                    return def;
                }
                if (typeof(T) == typeof(int))
                {
                    return (T)Convert.ChangeType(obj, typeof(int));
                }
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(obj, typeof(string));
                }
                return (T)obj;
            }
            catch { throw new ArgumentException(" field [" + field + "] is can not conversion to " + typeof(T)); }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="type">配置所属类型的名字</param>
        /// <param name="field">配置的字段名</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        public string Get(Type type, string field, string def)
        {
            return Get(type.ToString(), field, def);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name">配置所属类型的名字</param>
        /// <param name="field">配置的字段名</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        public string Get(string name, string field, string def)
        {
            return Get<string>(name, field, def);
        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <returns>解析完的配置</returns>
        private Dictionary<string, object> ParseConfig(IConfig config)
        {
            if (config.Config.Length <= 0)
            {
                return new Dictionary<string, object>();
            }
            if (config.Config.Length%2 != 0)
            {
                throw new ArgumentException("param is not incorrect");
            }

            var fields = new Dictionary<string, object>();
            var param = config.Config;
            for (var i = 0; i < param.Length; i += 2)
            {
                fields.Add(param[i].ToString(), param[i + 1]);
            }

            return fields;
        }
    }
}