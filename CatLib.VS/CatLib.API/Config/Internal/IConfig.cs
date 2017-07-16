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

using CatLib.API.Converters;

namespace CatLib.API.Config
{
    /// <summary>
    /// 配置
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns>配置值</returns>
        string this[string name] { get; }

        /// <summary>
        /// 设定转换器
        /// </summary>
        /// <param name="converter">类型转换器</param>
        void SetConverters(IConverters converter);

        /// <summary>
        /// 设定配置定位器
        /// </summary>
        /// <param name="locator">配置定位器</param>
        void SetLocator(IConfigLocator locator);

        /// <summary>
        /// 设定配置的值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置的值</param>
        void Set(string name, object value);

        /// <summary>
        /// 保存配置
        /// </summary>
        void Save();

        /// <summary>
        /// 根据配置名获取配置
        /// </summary>
        /// <typeparam name="T">配置最终转换到的类型</typeparam>
        /// <param name="name">配置所属类型的名字</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        T Get<T>(string name, T def = default(T));
    }
}