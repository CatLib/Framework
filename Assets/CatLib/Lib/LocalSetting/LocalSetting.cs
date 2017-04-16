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

using CatLib.API.LocalSetting;

namespace CatLib.LocalSetting
{
    /// <summary>
    /// 本地设置
    /// </summary>
    public sealed class LocalSetting : ILocalSetting
    {
        /// <summary>
        /// 配置容器
        /// </summary>
        private ILocalSetting settingStore;

        /// <summary>
        /// 设定配置容器
        /// </summary>
        /// <param name="setting"></param>
        public void SetSettingStore(ILocalSetting setting)
        {
            settingStore = setting;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            settingStore.Save();
        }

        /// <summary>
        /// 是否有指定的键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>键是否存在</returns>
        public bool Has(string key)
        {
            return settingStore.Has(key);
        }

        /// <summary>
        /// 移除指定的键
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            settingStore.Remove(key);
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public bool GetBool(string key, bool defaultValue = false)
        {
            return settingStore.GetBool(key, defaultValue);
        }

        /// <summary>
        /// 设定布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetBool(string key, bool val)
        {
            settingStore.SetBool(key, val);
        }

        /// <summary>
        /// 获取整形
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public int GetInt(string key, int defaultValue = 0)
        {
            return settingStore.GetInt(key, defaultValue);
        }

        /// <summary>
        /// 设定整形
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetInt(string key, int val)
        {
            settingStore.SetInt(key, val);
        }

        /// <summary>
        /// 获取浮点数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public float GetFloat(string key, float defaultValue = 0)
        {
            return settingStore.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// 设定浮点数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetFloat(string key, float val)
        {
            settingStore.SetFloat(key, val);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public string GetString(string key, string defaultValue = null)
        {
            return settingStore.GetString(key, defaultValue);
        }

        /// <summary>
        /// 设定字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetString(string key, string val)
        {
            settingStore.SetString(key, val);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置对象</returns>
        public T GetObject<T>(string key, T defaultValue = default(T))
        {
            return settingStore.GetObject<T>(key, defaultValue);
        }

        /// <summary>
        /// 设定对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="obj">配置对象</param>
        public void SetObject<T>(string key, T obj)
        {
            settingStore.SetObject<T>(key, obj);
        }
    }
}
