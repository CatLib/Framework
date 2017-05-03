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

using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using CatLib.API.LocalSetting;

namespace CatLib.LocalSetting
{
    /// <summary>
    /// Unity设置
    /// </summary>
    public sealed class UnitySetting : ILocalSetting
    {
        /// <summary>
        /// 保存设置
        /// </summary>
        public void Save()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 是否有指定的键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>键是否存在</returns>
        public bool Has(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// 移除指定的键
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }

        /// <summary>
        /// 设定布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetBool(string key, bool val)
        {
            PlayerPrefs.SetInt(key, val ? 1 : 0);
        }

        /// <summary>
        /// 获取整形
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        /// <summary>
        /// 设定整形
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetInt(string key, int val)
        {
            PlayerPrefs.SetInt(key, val);
        }

        /// <summary>
        /// 获取浮点数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// 设定浮点数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetFloat(string key, float val)
        {
            PlayerPrefs.SetFloat(key, val);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public string GetString(string key, string defaultValue = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        /// <summary>
        /// 设定字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">配置值</param>
        public void SetString(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
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
            IFormatter formatter = new BinaryFormatter();
            var buffer = Convert.FromBase64String(GetString(key));
            var stream = new MemoryStream(buffer);
            defaultValue = (T)formatter.Deserialize(stream);
            stream.Flush();
            stream.Close();
            return defaultValue;
        }

        /// <summary>
        /// 设定对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="obj">配置对象</param>
        public void SetObject<T>(string key, T obj)
        {
            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            SetString(key, Convert.ToBase64String(buffer));
        }
    }
}