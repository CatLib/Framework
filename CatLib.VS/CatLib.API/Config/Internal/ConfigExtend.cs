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
using CatLib.API.Config;

namespace CatLib
{
    /// <summary>
    /// 配置扩展
    /// </summary>
    public static class ConfigExtend
    {
        /// <summary>
        /// 安全的获取配置
        /// <para>自动对IConfig对象进行空判断，如果为空则返回默认值</para>
        /// </summary>
        /// <typeparam name="T">默认类型</typeparam>
        /// <param name="config">配置中枢</param>
        /// <param name="name">配置名字</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T SafeGet<T>(this IConfig config, string name, T def = default(T))
        {
            return config == null ? def : config.Get(name, def);
        }

        /// <summary>
        /// 安全的获取配置
        /// <para>自动对IConfig对象进行空判断，如果为空则返回默认值</para>
        /// </summary>
        /// <param name="config">配置中枢</param>
        /// <param name="name">配置名字</param>
        /// <param name="type">配置的类型</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static object SafeGet(this IConfig config, string name, Type type, object def = null)
        {
            return config == null ? def : config.Get(name, type, def);
        }

        /// <summary>
        /// 安全的观察
        /// <para>自动对IConfig对象进行空判断，如果为空则不处理</para>
        /// </summary>
        /// <param name="config">配置中枢</param>
        /// <param name="name">配置名字</param>
        /// <param name="callback">默认值</param>
        public static void SafeWatch(this IConfig config, string name, Action<object> callback)
        {
            if (config != null)
            {
                config.Watch(name, callback);
            }
        }

        /// <summary>
        /// 安全的设置
        /// <para>自动对IConfig对象进行空判断，如果为空则不处理</para>
        /// </summary>
        /// <param name="config">配置中枢</param>
        /// <param name="name">配置名字</param>
        /// <param name="value">配置的值</param>
        public static void SafeSet(this IConfig config, string name, object value)
        {
            if (config != null)
            {
                config.Set(name, value);
            }
        }
    }
}
