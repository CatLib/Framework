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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 请求
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Uri
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// 上下文
        /// </summary>
        /// <returns></returns>
        object GetContext();

        /// <summary>
        /// 构成uri路径段的数组
        /// </summary>
        /// <param name="index">下标</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        string Segment(int index, string defaultValue = null);

        /// <summary>
        /// 获取字符串附加物
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        string Get(string key, string defaultValue = null);

        /// <summary>
        /// 替换参数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void ReplaceParameter(string key, string value);

        /// <summary>
        /// 替换上下文
        /// </summary>
        /// <param name="context">上下文</param>
        void ReplaceContext(object context);

        /// <summary>
        /// 获取字符串附加物
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get; }

        /// <summary>
        /// 获取字符串附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string GetString(string key, string defaultValue = null);

        /// <summary>
        /// 获取整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int GetInt(string key, int defaultValue = 0);

        /// <summary>
        /// 获取长整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        long GetLong(string key, long defaultValue = 0);

        /// <summary>
        /// 获取短整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        short GetShort(string key, short defaultValue = 0);

        /// <summary>
        /// 获取字符的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        char GetChar(string key, char defaultValue = char.MinValue);

        /// <summary>
        /// 获取浮点数的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        float GetFloat(string key, float defaultValue = 0);

        /// <summary>
        /// 获取双精度浮点数的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        double GetDouble(string key, double defaultValue = 0);

        /// <summary>
        /// 获取布尔值的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        bool GetBoolean(string key, bool defaultValue = false);
    }
}