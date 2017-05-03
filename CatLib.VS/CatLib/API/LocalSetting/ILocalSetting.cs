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

namespace CatLib.API.LocalSetting
{
    /// <summary>
    /// 本地配置
    /// </summary>
    public interface ILocalSetting
    {
        /// <summary>
        /// 保存设置
        /// </summary>
        void Save();

        /// <summary>
        /// 是否有指定的键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>键是否存在</returns>
        bool Has(string key);

        /// <summary>
        /// 移除指定的键
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        bool GetBool(string key, bool defaultValue = false);

        /// <summary>
        /// 设定布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        void SetBool(string key, bool val);

        /// <summary>
        /// 获取整形
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        int GetInt(string key, int defaultValue = 0);

        /// <summary>
        /// 设定整形
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        void SetInt(string key, int val);

        /// <summary>
        /// 获取浮点数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        float GetFloat(string key, float defaultValue = 0);

        /// <summary>
        /// 设定浮点数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        void SetFloat(string key, float val);

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        string GetString(string key, string defaultValue = null);

        /// <summary>
        /// 设定字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        void SetString(string key, string val);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置对象</returns>
        T GetObject<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// 设定对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="obj">配置对象</param>
        void SetObject<T>(string key, T obj);
    }
}