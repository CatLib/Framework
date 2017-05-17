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
using CatLib.API;
using CatLib.API.Config;
using CatLib.Stl;

namespace CatLib.Config
{
    /// <summary>
    /// 配置容器
    /// </summary>
    public sealed class Config : IConfig
    {
        /// <summary>
        /// 服务程序
        /// </summary>
        [Inject]
        public IApplication App { get; set; }

        /// <summary>
        /// 配置定位器
        /// </summary>
        private readonly SortSet<IConfigLocator, int> locators;

        /// <summary>
        /// 构造一个配置容器
        /// </summary>
        public Config()
        {
            locators = new SortSet<IConfigLocator, int>();
        }

        /// <summary>
        /// 注册一个配置定位器
        /// 框架会依次遍历配置定位器来获取配置
        /// </summary>
        /// <param name="locator"></param>
        public void Register(IConfigLocator locator)
        {
            Guard.Requires<ArgumentNullException>(locator != null);
            var priorities = App.GetPriorities(locator.GetType(), "Get");
            locators.Add(locator, priorities);
        }

        /// <summary>
        /// 根据配置名获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get { return Get<string>(name); }
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
            try
            {
                string val = null;
                foreach (var locator in locators)
                {
                    if (locator.TryGetValue(name, out val))
                    {
                        break;
                    }
                }

                if (val == null)
                {
                    return def;
                }

                var type = typeof(T);
                if (type == typeof(int))
                {
                    return (T)Convert.ChangeType(val, typeof(int));
                }
                if (type == typeof(string))
                {
                    return (T)Convert.ChangeType(val, typeof(string));
                }

                return def;
            }
            catch
            {
                throw new ArgumentException(" field [" + name + "] is can not conversion to " + typeof(T));
            }
        }
    }
}