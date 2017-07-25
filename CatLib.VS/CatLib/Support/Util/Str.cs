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

using System.Text.RegularExpressions;

namespace CatLib
{
    /// <summary>
    /// 字符串
    /// </summary>
    public static class Str
    {
        /// <summary>
        /// 判断是否属于(允许使用信号通配表达式)
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
    }
}
