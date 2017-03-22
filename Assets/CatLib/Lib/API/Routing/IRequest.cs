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
        /// FullPath eg: catlib://login/register
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// 方案 eg: catlib
        /// </summary>
        string Scheme { get; }

        /// <summary>
        /// host eg: login
        /// </summary>
        string Host { get; }

        /// <summary>
        /// 获取 URI 的绝对路径(不带参数) eg:/register
        /// </summary>
        string Path { get; }

        /// <summary>
        /// scheme + host + path 组合内容 eg: catlib://login/register
        /// </summary>
        string SchemeHostPath { get; }

        /// <summary>
        /// 请求中附带的用户信息
        /// </summary>
        string UserInfo { get; }

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
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string Get(string key, string defaultValue = null);

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