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

namespace CatLib
{
    /// <summary>
    /// 从配置组件中获取配置填充到属性中
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public object Default { get; private set; }

        /// <summary>
        /// 配置名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 被标记的对象可以配置
        /// </summary>
        /// <param name="defaultValue">默认值</param>
        /// <param name="name">使用输入的配置名来获取配置</param>
        public ConfigAttribute(object defaultValue = null, string name = null)
        {
            Default = defaultValue;
            Name = name;
        }
    }
}
