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

using System.Collections.Generic;
using CatLib.API.Config;
using CatLib.Stl;
using UnityEngine;

namespace CatLib.Config
{
    /// <summary>
    /// Unity设置定位器
    /// </summary>
    public sealed class UnitySettingLocator : IConfigLocator
    {
#if !UNITY_5_OR_NEW
        /// <summary>
        /// 配置字典
        /// </summary>
        private readonly Dictionary<string, string> dict;

        /// <summary>
        /// 配置定位器
        /// </summary>
        public UnitySettingLocator()
        {
            dict = new Dictionary<string, string>();
        }
#endif

        /// <summary>
        /// 设定值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        public void Set(string name, string value)
        {
            Guard.NotNull(name, "name");
#if UNITY_5_OR_NEW
            PlayerPrefs.SetString(name, value);
#else
            dict.Remove(name);
            dict.Add(name, value);
#endif
        }

        /// <summary>
        /// 根据配置名获取配置的值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        /// <returns>是否获取到配置</returns>
        public bool TryGetValue(string name, out string value)
        {
            Guard.NotNull(name, "name");
#if UNITY_5_OR_NEW
            value = string.Empty;
            if (PlayerPrefs.HasKey(name))
            {
                return false;
            }
            value = PlayerPrefs.GetString(name);
            return true;
#else
            return dict.TryGetValue(name, out value);
#endif
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
#if UNITY_5_OR_NEW
            PlayerPrefs.Save();
#endif
        }
    }
}
