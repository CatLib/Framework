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

using CatLib.API.Routing;
using System;

namespace CatLib.Routing
{
    /// <summary>
    /// 请求(由于某些异常导致请求都无法被构建所以我们提供一个必定可以被构建的请求类)
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ExceptionRequest : IRequest
    {
        /// <summary>
        /// Uri
        /// </summary>
        public System.Uri Uri
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 访问uri
        /// </summary>
        private readonly string uri;

        /// <summary>
        /// 上下文
        /// </summary>
        private object context;

        /// <summary>
        /// 构建一个请求
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="context">上下文</param>
        public ExceptionRequest(string uri , object context)
        {
            this.uri = uri;
            this.context = context;
        }

        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns>上下文</returns>
        public object GetContext()
        {
            return context;
        }

        /// <summary>
        /// 构成uri路径段的数组
        /// </summary>
        /// <param name="index">下标</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string Segment(int index, string defaultValue = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取字符串附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string Get(string key, string defaultValue = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 替换参数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void ReplaceParameter(string key, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 替换上下文
        /// </summary>
        /// <param name="context">上下文</param>
        public void ReplaceContext(object context)
        {
            this.context = context;
        }

        /// <summary>
        /// 获取字符串附加物
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return Get(key);
            }
        }

        /// <summary>
        /// 获取字符串附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        /// <summary>
        /// 获取整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取长整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long GetLong(string key, long defaultValue = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取短整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public short GetShort(string key, short defaultValue = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取字符的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public char GetChar(string key, char defaultValue = char.MinValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取浮点数的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float GetFloat(string key, float defaultValue = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取双精度浮点数的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double GetDouble(string key, double defaultValue = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取布尔值的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBoolean(string key, bool defaultValue = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns>uri</returns>
        public override string ToString()
        {
            return uri;
        }
    }
}
