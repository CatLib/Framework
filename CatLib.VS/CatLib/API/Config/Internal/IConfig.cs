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
    /// 配置
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns></returns>
        string this[string name] { get; }

        /// <summary>
        /// 增加转换器
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="converter">类型对应转换器</param>
        void AddConverter(Type type, ITypeStringConverter converter);

        /// <summary>
        /// 注册一个配置定位器
        /// 框架会依次遍历配置定位器来获取配置
        /// </summary>
        /// <param name="locator">配置定位器</param>
        /// <param name="priority">查询优先级(值越小越优先)</param>
        void AddLocator(IConfigLocator locator, int priority = int.MaxValue);

        /// <summary>
        /// 设定配置的值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="name">配置名</param>
        /// <param name="value">配置的值</param>
        void Set<T>(string name, T value);

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