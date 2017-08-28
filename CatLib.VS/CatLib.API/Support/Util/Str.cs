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
        /// 填充类型
        /// </summary>
        public enum PadTypes
        {
            /// <summary>
            /// 填充字符串的两侧。如果不是偶数，则右侧获得额外的填充。
            /// </summary>
            Both,

            /// <summary>
            /// 填充字符串的左侧。
            /// </summary>
            Left,

            /// <summary>
            /// 填充字符串的右侧。默认。
            /// </summary>
            Right
        }

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

        /// <summary>
        /// 将字符串重复指定的次数
        /// </summary>
        /// <param name="str">需要被重复的字符串</param>
        /// <param name="num">重复的次数</param>
        /// <returns>重复后的字符串</returns>
        public static string Repeat(string str, int num)
        {
            Guard.Requires<ArgumentNullException>(str != null);
            Guard.Requires<ArgumentOutOfRangeException>(num >= 0);

            if (num == 0)
            {
                return str;
            }

            var requested = new StringBuilder();
            for (var i = 0; i < num; i++)
            {
                requested.Append(str);
            }
            return requested.ToString();
        }

        /// <summary>
        /// 随机打乱字符串中的所有字符
        /// </summary>
        /// <param name="str">需要被打乱的字符串</param>
        /// <param name="seed">种子</param>
        /// <returns>被打乱的字符串</returns>
        public static string Shuffle(string str, int? seed = null)
        {
            Guard.Requires<ArgumentNullException>(str != null);
            var random = new Random(seed.GetValueOrDefault(Guid.NewGuid().GetHashCode()));

            var requested = new string[str.Length];
            for (var i = 0; i < str.Length; i++)
            {
                var index = random.Next(0, str.Length - 1);

                requested[i] = requested[i] ?? str.Substring(i, 1);
                requested[index] = requested[index] ?? str.Substring(index, 1);

                if (index == i)
                {
                    continue;
                }

                var temp = requested[i];
                requested[i] = requested[index];
                requested[index] = temp;
            }

            return Arr.Reduce(requested, (v1, v2) => v1 + v2, string.Empty);
        }

        /// <summary>
        /// 计算子串在字符串中出现的次数
        /// <para>子串是区分大小写的</para>
        /// <para>该函数不计数重叠的子串</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="subStr"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static int SubstringCount(string str, string subStr, int start = 0, int? length = null , StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            Guard.Requires<ArgumentNullException>(str != null);
            Guard.Requires<ArgumentNullException>(subStr != null);

            Util.NormalizationPosition(str.Length, ref start, ref length);

            var count = 0;
            int index;
            while (length.Value > 0)
            {
                if ((index = str.IndexOf(subStr, start, length.Value, comparison)) < 0)
                {
                    break;
                    
                }
                count++;
                length -= index + subStr.Length - start;
                start = index + subStr.Length;
            }

            return count;
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="str">规定要反转的字符串</param>
        /// <returns>反转后的字符串</returns>
        public static string Reverse(string str)
        {
            var chars = str.ToCharArray();
            Array.Reverse(chars);

            return new string(chars);
        }

        /// <summary>
        /// 把字符串填充为新的长度。
        /// </summary>
        /// <param name="str">规定要填充的字符串</param>
        /// <param name="length">规定新的字符串长度。如果该值小于字符串的原始长度，则不进行任何操作。</param>
        /// <param name="padStr">规定供填充使用的字符串。默认是空白。如果传入的字符串长度小于等于0那么会使用空白代替。</param>
        /// <param name="type">
        /// 规定填充字符串的哪边。
        /// <para><see cref="PadTypes.Both"/>填充字符串的两侧。如果不是偶数，则右侧获得额外的填充。</para>
        /// <para><see cref="PadTypes.Left"/>填充字符串的左侧。</para>
        /// <para><see cref="PadTypes.Right"/>填充字符串的右侧。默认。</para>
        /// </param>
        /// <returns>被填充的字符串</returns>
        public static string Pad(string str, int length, string padStr = null, PadTypes type = PadTypes.Right)
        {
            Guard.Requires<ArgumentNullException>(str != null);

            var needPadding = length - str.Length;
            if (needPadding <= 0)
            {
                return str;
            }

            int rightPadding;
            var leftPadding = rightPadding = 0;

            if (type == PadTypes.Both)
            {
                leftPadding = needPadding >> 1;
                rightPadding = (needPadding >> 1) + (needPadding % 2 == 0 ? 0 : 1);
            }else if (type == PadTypes.Right)
            {
                rightPadding = needPadding;
            }
            else
            {
                leftPadding = needPadding;
            }

            padStr = padStr ?? " ";
            padStr = padStr.Length <= 0 ? " " : padStr;

            var leftPadCount = leftPadding / padStr.Length + (leftPadding % padStr.Length == 0 ? 0 : 1);
            var rightPadCount = rightPadding / padStr.Length + (rightPadding % padStr.Length == 0 ? 0 : 1);

            return Repeat(padStr, leftPadCount).Substring(0, leftPadding) + str +
                   Repeat(padStr, rightPadCount).Substring(0, rightPadding);
        }
    }
}
