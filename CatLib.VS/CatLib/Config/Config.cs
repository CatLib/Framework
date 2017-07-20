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

using CatLib.API.Config;
using CatLib.API.Converters;
using System;

namespace CatLib.Config
{
    /// <summary>
    /// 配置容器
    /// </summary>
    public sealed class Config : IConfig
    {
        /// <summary>
        /// 配置定位器
        /// </summary>
        private IConfigLocator locator;

        /// <summary>
        /// 类型转换器
        /// </summary>
        private IConverters converters;

        /// <summary>
        /// 构造配置容器
        /// </summary>
        /// <param name="converters">转换器</param>
        /// <param name="locator">配置定位器</param>
        public Config(IConverters converters, IConfigLocator locator)
        {
            Guard.Requires<ArgumentNullException>(converters != null);
            Guard.Requires<ArgumentNullException>(locator != null);

            this.converters = converters;
            this.locator = locator;
        }

        /// <summary>
        /// 设定类型转换器
        /// </summary>
        /// <param name="converters">转换器</param>
        public void SetConverters(IConverters converters)
        {
            Guard.Requires<ArgumentNullException>(converters != null);
            this.converters = converters;
        }

        /// <summary>
        /// 注册一个配置定位器
        /// </summary>
        /// <param name="locator">配置定位器</param>
        public void SetLocator(IConfigLocator locator)
        {
            Guard.Requires<ArgumentNullException>(locator != null);
            this.locator = locator;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            GuardLocator();
            locator.Save();
        }

        /// <summary>
        /// 根据配置名获取配置
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns>配置的值</returns>
        public string this[string name]
        {
            get { return Get<string>(name); }
        }

        /// <summary>
        /// 设定配置的值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置的值</param>
        public void Set(string name, object value)
        {
            Guard.Requires<ArgumentNullException>(name != null);
            GuardConverters();
            GuardLocator();
            locator.Set(name, converters.Convert<string>(value));
        }

        /// <summary>
        /// 根据配置名获取配置
        /// </summary>
        /// <typeparam name="T">配置最终转换到的类型</typeparam>
        /// <param name="name">配置所属类型的名字</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        public T Get<T>(string name, T def = default(T))
        {
            Guard.Requires<ArgumentNullException>(name != null);
            GuardConverters();
            GuardLocator();

            string val;
            if (!locator.TryGetValue(name, out val))
            {
                return def;
            }

            T result;
            return converters.TryConvert(val, out result) ? result : def;
        }

        /// <summary>
        /// 检验定位器是否有效
        /// </summary>
        private void GuardLocator()
        {
            if (locator == null)
            {
                throw new RuntimeException("Undefiend config locator , please call IConfig.SetLocator");
            }
        }

        /// <summary>
        /// 校验转换器是否有效
        /// </summary>
        private void GuardConverters()
        {
            if (converters == null)
            {
                throw new RuntimeException("Undefiend converters , please call IConfig.SetConverters");
            }
        }
    }
}