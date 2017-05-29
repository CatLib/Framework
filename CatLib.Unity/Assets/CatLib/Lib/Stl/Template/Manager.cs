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
using CatLib.API;

namespace CatLib.Stl
{
    /// <summary>
    /// 管理器模版
    /// </summary>
    public abstract class Manager<TInterface>
    {
        /// <summary>
        /// 自定义解决器
        /// </summary>
        private readonly Dictionary<string, Func<TInterface>> customResolve = new Dictionary<string, Func<TInterface>>();

        /// <summary>
        /// 默认名字
        /// </summary>
        private readonly string defaultName = "default";

        /// <summary>
        /// 获取解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        /// <returns>解决方案</returns>
        public TInterface Get(string name = null)
        {
            name = string.IsNullOrEmpty(name) ? GetDefaultName() : name;
            return Resolve(name);
        }

        /// <summary>
        /// 获取解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        /// <returns>解决方案</returns>
        public TInterface this[string name]
        {
            get { return Get(name); }
        }

        /// <summary>
        /// 自定义解决方案
        /// </summary>
        /// <param name="resolve">解决方案</param>
        /// <param name="name">名字</param>
        public void Extend(Func<TInterface> resolve, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = GetDefaultName();
            }
            if (customResolve.ContainsKey(name))
            {
                throw new RuntimeException("Custom resolve [" + name + "](" + GetType() + ") is already exists.");
            }
            customResolve.Add(name, resolve);
        }

        /// <summary>
        /// 获取默认名字
        /// </summary>
        /// <returns>默认名字</returns>
        protected virtual string GetDefaultName()
        {
            return defaultName;
        }

        /// <summary>
        /// 生成所需求的解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        /// <returns>解决方案</returns>
        private TInterface Resolve(string name)
        {
            Func<TInterface> elementCustomResolve;
            if (customResolve.TryGetValue(name, out elementCustomResolve))
            {
                return elementCustomResolve.Invoke();
            }

            throw new RuntimeException("Can not find [" + name + "](" + GetType() + ") Extend.");
        }
    }
}
