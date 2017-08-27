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
using System.Text;
using System.Text.RegularExpressions;

namespace CatLib
{
    /// <summary>
    /// 字符串
    /// </summary>
    public static class Str
    {
        /// <summary>
        /// 判断是否属于(允许使用星号通配表达式)
        /// </summary>
        /// <param name="pattern">比较字符串</param>
        /// <param name="value">输入字符串</param>
        /// <returns>是否匹配</returns>
        public static bool Is(string pattern, string value)
        {
            return pattern == value || Regex.IsMatch(value, "^" + AsteriskWildcard(pattern) + "$");
        }

        /// <summary>
        /// 将语句翻译为星号通配表达式(即删减正则表达式中除了星号外的所有功能)
        /// </summary>
        /// <param name="pattern">输入</param>
        /// <returns>输出</returns>
        public static string AsteriskWildcard(string pattern)
        {
            pattern = RegexQuote(pattern);
            pattern = pattern.Replace(@"\*", ".*?");

            return pattern;
        }

        /// <summary>
        /// 转义正则表达式
        /// </summary>
        /// <param name="str">输入</param>
        /// <returns>输出</returns>
        public static string RegexQuote(string str)
        {
            string[] quote = { @"\", ".", "+", "*", "?", "[", "^", "]", "$", "(", ")", "{", "}", "=", "!", "<", ">", "|", ":", "-" };
            foreach (var q in quote)
            {
                str = str.Replace(q, @"\" + q);
            }
            return str;
        }

        /// <summary>
        /// 根据长度将字符串分割到数组中
        /// </summary>
        /// <param name="str">要分割的字符串</param>
        /// <param name="length">规定每个数组元素的长度。默认是 1。</param>
        /// <returns>分割的字符串</returns>
        public static string[] Split(string str, int length = 1)
        {
            Guard.Requires<ArgumentNullException>(str != null);
            Guard.Requires<ArgumentOutOfRangeException>(length > 0);
            var requested = new string[str.Length / length + (str.Length % length == 0 ? 0 : 1)];

            for (var i = 0; i < str.Length; i += length)
            {
                requested[i / length] = str.Substring(i, Math.Min(str.Length - i, length));
            }

            return requested;
        }
    }
}
