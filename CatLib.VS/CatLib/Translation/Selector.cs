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

using CatLib.API.Translation;
using System.Text.RegularExpressions;

namespace CatLib.Translation
{
    /// <summary>
    /// 选择器
    /// </summary>
    public sealed class Selector : ISelector
    {
        /// <summary>
        /// 区间匹配表达式
        /// </summary>
        private const string StripMatchStr = @"[\{\[]([^\[\]\{\}]*)[\}\]]";

        /// <summary>
        /// 展开处理匹配式
        /// </summary>
        private readonly Regex extractReg = new Regex(@"[\{\[]([^\[\]\{\}]*)[\}\]](.*)");

        /// <summary>
        /// 对翻译进行处理
        /// </summary>
        /// <param name="line">语言字符串</param>
        /// <param name="number">数量</param>
        /// <param name="locale">语言</param>
        /// <returns>处理后的字符串</returns>
        public string Choose(string line, int number, string locale)
        {
            if (string.IsNullOrEmpty(line))
            {
                return string.Empty;
            }

            var segments = line.Split('|');

            string val;
            if ((val = Extract(segments, number)) != null)
            {
                return val.Trim();
            }

            segments = StripConditions(segments);
            var pluralIndex = GetPluralIndex(locale, number);
            if (segments.Length == 1 || segments.Length < pluralIndex)
            {
                return segments[0];
            }

            return segments[pluralIndex];
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="segments">片段</param>
        /// <param name="number">数字</param>
        /// <returns>字符串</returns>
        private string Extract(string[] segments, int number)
        {
            string line = null;
            for (var i = 0; i < segments.Length; i++)
            {
                if ((line = RangeExtract(segments[i], number)) != null)
                {
                    return line;
                }
            }
            return line;
        }

        /// <summary>
        /// 范围处理
        /// </summary>
        /// <param name="parts">单片段</param>
        /// <param name="number">数字</param>
        /// <returns></returns>
        private string RangeExtract(string parts, int number)
        {
            var mc = extractReg.Matches(parts);

            if (mc.Count < 1)
            {
                return null;
            }
            if (mc[0].Groups.Count != 3)
            {
                return null;
            }

            var condition = mc[0].Groups[1].ToString();
            var val = mc[0].Groups[2].ToString();

            if (condition.Contains(","))
            {
                var fromTo = condition.Split(',');

                if (fromTo[0] == "*" && fromTo[1] == "*")
                {
                    return val;
                }

                if (fromTo[1] == "*" && number >= int.Parse(fromTo[0]))
                {
                    return val;
                }

                if (fromTo[0] == "*" && number <= int.Parse(fromTo[1]))
                {
                    return val;
                }

                if (fromTo[0] != "*" &&
                    fromTo[1] != "*" &&
                    number >= int.Parse(fromTo[0]) &&
                    number <= int.Parse(fromTo[1]))
                {
                    return val;
                }
            }

            return (condition == "*" || condition == number.ToString()) ? val : null;
        }

        /// <summary>
        /// 过滤未被处理的片段
        /// </summary>
        /// <param name="segments">片段</param>
        /// <returns>处理后的结果</returns>
        private string[] StripConditions(string[] segments)
        {
            for (var i = 0; i < segments.Length; i++)
            {
                segments[i] = Regex.Replace(segments[i], StripMatchStr, string.Empty);
            }

            return segments;
        }

        /// <summary>
        /// 获取语言的复数形式
        /// 
        /// 语言复数形式规则来自于：Zend Framework
        /// The plural rules are derived from code of the Zend Framework (2010-09-25), which
        /// is subject to the new BSD license(http://framework.zend.com/license/new-bsd)
        /// Copyright (c) 2005-2010 - Zend Technologies USA Inc. (http://www.zend.com)
        /// </summary>
        /// <param name="locale">语言</param>
        /// <param name="number">数量</param>
        /// <returns>复数形式</returns>
        [ExcludeFromCodeCoverage]
        private int GetPluralIndex(string locale, int number)
        {
            switch (locale)
            {
                case Languages.Azerbaijani:
                case Languages.Tibetan:
                case Languages.Bhutani:
                case Languages.Indonesian:
                case Languages.Japanese:
                case Languages.Javanese:
                case Languages.Georgian:
                case Languages.Cambodian:
                case Languages.Kannada:
                case Languages.Korean:
                case Languages.Malay:
                case Languages.Thai:
                case Languages.Turkish:
                case Languages.Vietnamese:
                case Languages.Chinese:
                case Languages.ChineseTw:
                    return 0;
                case Languages.Afrikaans:
                case Languages.Bengali:
                case Languages.Bulgarian:
                case Languages.Catalan:
                case Languages.Danish:
                case Languages.German:
                case Languages.Greek:
                case Languages.English:
                case Languages.Esperanto:
                case Languages.Spanish:
                case Languages.Estonian:
                case Languages.Basque:
                case Languages.Farsi:
                case Languages.Finnish:
                case Languages.Faeroese:
                case Languages.Frisian:
                case Languages.Galician:
                case Languages.Gujarati:
                case Languages.Hausa:
                case Languages.Hebrew:
                case Languages.Hungarian:
                case Languages.Icelandic:
                case Languages.Italian:
                case Languages.Kurdish:
                case Languages.Malayalam:
                case Languages.Mongolian:
                case Languages.Marathi:
                case Languages.Nepali:
                case Languages.Dutch:
                case Languages.Norwegian:
                case Languages.Oromo:
                case Languages.Oriya:
                case Languages.Punjabi:
                case Languages.Pashto:
                case Languages.Portuguese:
                case Languages.Somali:
                case Languages.Albanian:
                case Languages.Swedish:
                case Languages.Swahili:
                case Languages.Tamil:
                case Languages.Telugu:
                case Languages.Turkmen:
                case Languages.Urdu:
                case Languages.Zulu:
                    return (number == 1) ? 0 : 1;
                case Languages.Amharic:
                case Languages.Bihari:
                case Languages.French:
                case Languages.Hindi:
                case Languages.Armenian:
                case Languages.Lingala:
                case Languages.Malagasy:
                case Languages.Tigrinya:
                    return ((number == 0) || (number == 1)) ? 0 : 1;
                case Languages.Byelorussian:
                case Languages.Croatian:
                case Languages.Russian:
                case Languages.Serbian:
                case Languages.Ukrainian:
                    return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);
                case Languages.Czech:
                case Languages.Slovak:
                    return (number == 1) ? 0 : (((number >= 2) && (number <= 4)) ? 1 : 2);
                case Languages.Irish:
                    return (number == 1) ? 0 : ((number == 2) ? 1 : 2);
                case Languages.Lithuanian:
                    return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);
                case Languages.Slovenian:
                    return (number % 100 == 1) ? 0 : ((number % 100 == 2) ? 1 : (((number % 100 == 3) || (number % 100 == 4)) ? 2 : 3));
                case Languages.Macedonian:
                    return (number % 10 == 1) ? 0 : 1;
                case Languages.Maltese:
                    return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 1) && (number % 100 < 11))) ? 1 : (((number % 100 > 10) && (number % 100 < 20)) ? 2 : 3));
                case Languages.Latvian:
                    return (number == 0) ? 0 : (((number % 10 == 1) && (number % 100 != 11)) ? 1 : 2);
                case Languages.Polish:
                    return (number == 1) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 12) || (number % 100 > 14))) ? 1 : 2);
                case Languages.Welsh:
                    return (number == 1) ? 0 : ((number == 2) ? 1 : (((number == 8) || (number == 11)) ? 2 : 3));
                case Languages.Romanian:
                    return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 0) && (number % 100 < 20))) ? 1 : 2);
                case Languages.Arabic:
                    return (number == 0) ? 0 : ((number == 1) ? 1 : ((number == 2) ? 2 : (((number % 100 >= 3) && (number % 100 <= 10)) ? 3 : (((number % 100 >= 11) && (number % 100 <= 99)) ? 4 : 5))));
                default:
                    return 0;
            }
        }
    }
}