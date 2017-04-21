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

namespace CatLib.API.Config
{
    /// <summary>
    /// 配置容器
    /// </summary>
    public interface IConfigStore
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置最终转换到的类型</typeparam>
        /// <param name="name">配置所属类型的名字</param>
        /// <param name="field">配置的字段名</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        T Get<T>(string name, string field, T def = default(T));

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置最终转换到的类型</typeparam>
        /// <param name="type">配置所属类型</param>
        /// <param name="field">配置字段</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        T Get<T>(Type type, string field, T def = default(T));

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name">配置所属类型的名字</param>
        /// <param name="field">配置的字段名</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        string Get(string name, string field, string def);

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="type">配置所属类型的名字</param>
        /// <param name="field">配置的字段名</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        string Get(Type type, string field, string def);
    }
}