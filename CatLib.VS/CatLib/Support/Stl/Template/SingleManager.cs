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

using CatLib.API.Stl;
using System.Collections.Generic;

namespace CatLib.Stl
{
    /// <summary>
    /// 管理器模版（拓展解决方案为单例）
    /// </summary>
    public abstract class SingleManager<TInterface> : Manager<TInterface>, ISingleManager<TInterface>
    {
        /// <summary>
        /// 解决方案字典
        /// </summary>
        private readonly Dictionary<string, TInterface> elements = new Dictionary<string, TInterface>();

        /// <summary>
        /// 获取解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        /// <returns>解决方案</returns>
        public new TInterface Get(string name = null)
        {
            name = string.IsNullOrEmpty(name) ? GetDefaultName() : name;
            return Make(name);
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public TInterface Default
        {
            get
            {
                return Get();
            }
        }

        /// <summary>
        /// 获取解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        /// <returns>解决方案</returns>
        public new TInterface this[string name]
        {
            get
            {
                return Get(name);
            }
        }

        /// <summary>
        /// 释放解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        public void Release(string name = null)
        {
            name = string.IsNullOrEmpty(name) ? GetDefaultName() : name;
            elements.Remove(name);
        }

        /// <summary>
        /// 生成解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        /// <returns>解决方案实现</returns>
        private TInterface Make(string name)
        {
            TInterface element;
            if (elements.TryGetValue(name, out element))
            {
                return element;
            }
            element = base.Get(name);
            elements.Add(name, element);
            return element;
        }
    }
}
