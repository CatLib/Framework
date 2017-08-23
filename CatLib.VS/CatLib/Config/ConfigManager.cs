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
using System.Reflection;
using CatLib.API.Config;

namespace CatLib.Config
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    internal sealed class ConfigManager : SingleManager<IConfig>, IConfigManager
    {
        /// <summary>
        /// 默认的配置名字
        /// </summary>
        private string defaultName;

        /// <summary>
        /// 配置管理器
        /// </summary>
        public ConfigManager()
        {
            App.On(ApplicationEvents.OnIniting, (provider) =>
            {
                App.Make<IConfigManager>().Config(provider);
            });
        }

        /// <summary>
        /// 设定默认的配置名字
        /// </summary>
        /// <param name="name">默认配置名字</param>
        public void SetDefault(string name)
        {
            if (name == string.Empty)
            {
                name = null;
            }

            defaultName = name;
        }

        /// <summary>
        /// 获取默认的配置名字
        /// </summary>
        /// <returns>默认的文件系统名字</returns>
        protected override string GetDefaultName()
        {
            return defaultName ?? "default";
        }

        /// <summary>
        /// 对目标实例注入配置
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="name">使用的配置容器名字</param>
        public void Config(object instance, string name = null)
        {
            Guard.Requires<ArgumentNullException>(instance != null);
            var config = Get(name);
            foreach (var property in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                if (!property.IsDefined(typeof(ConfigAttribute), false))
                {
                    continue;
                }

                var configAttr = (ConfigAttribute)property.GetCustomAttributes(typeof(ConfigAttribute), false)[0];

                var value = configAttr.Default;
                var configName = configAttr.Name;
                if (string.IsNullOrEmpty(configName))
                {
                    configName = instance.GetType().Name + "." + property.Name;
                }

                var result = config.SafeGet(configName, property.PropertyType, value);
                property.SetValue(instance, result, null);
            }
        }
    }
}
