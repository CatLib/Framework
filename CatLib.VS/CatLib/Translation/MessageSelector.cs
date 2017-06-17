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
using CatLib.API.Translation;

namespace CatLib.Translation
{
    /// <summary>
    /// 消息选择器
    /// </summary>
    public sealed class MessageSelector : ISelector
    {
        /// <summary>
        /// 对翻译进行处理
        /// </summary>
        /// <param name="line">语言字符串</param>
        /// <param name="number">数量</param>
        /// <param name="locale">语言</param>
        /// <returns>处理后的字符串</returns>
        public string Choose(string line, int number, string locale)
        {
            if (line == null)
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
            var regstr = @"^[\{\[]([^\[\]\{\}]*)[\}\]](.*)";
            var reg = new Regex(regstr);
            var mc = reg.Matches(parts);

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

            if (!condition.Contains(","))
            {
                return null;
            }

            var fromTo = condition.Split(new[] { ',' }, 2);
            if (fromTo[1] == "*" && number >= int.Parse(fromTo[0]))
            {
                return val;
            }

            if (fromTo[0] == "*" && number <= int.Parse(fromTo[1]))
            {
                return val;
            }

            if (number >= int.Parse(fromTo[0]) && number <= int.Parse(fromTo[1]))
            {
                return val;
            }

            return null;
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
                segments[i] = Regex.Replace(segments[i], @"^[\{\[]([^\[\]\{\}]*)[\}\]]", string.Empty);
            }

            return segments;
        }

        /// <summary>
        /// 获取语言的复数形式
        /// </summary>
        /// <param name="locale">语言</param>
        /// <param name="number">数量</param>
        /// <returns>复数形式</returns>
        private int GetPluralIndex(string locale, int number)
        {
            switch (locale)
            {
                case Language.Azerbaijani:
                case Language.Tibetan:
                case Language.Bhutani:
                case Language.Indonesian:
                case Language.Japanese:
                case Language.Javanese:
                case Language.Georgian:
                case Language.Cambodian:
                case Language.Kannada:
                case Language.Korean:
                case Language.Malay:
                case Language.Thai:
                case Language.Turkish:
                case Language.Vietnamese:
                case Language.Chinese:
                case Language.ChineseTw:
                    return 0;
                case Language.Afrikaans:
                case Language.Bengali:
                case Language.Bulgarian:
                case Language.Catalan:
                case Language.Danish:
                case Language.German:
                case Language.Greek:
                case Language.English:
                case Language.Esperanto:
                case Language.Spanish:
                case Language.Estonian:
                case Language.Basque:
                case Language.Farsi:
                case Language.Finnish:
                case Language.Faeroese:
                case Language.Frisian:
                case Language.Galician:
                case Language.Gujarati:
                case Language.Hausa:
                case Language.Hebrew:
                case Language.Hungarian:
                case Language.Icelandic:
                case Language.Italian:
                case Language.Kurdish:
                case Language.Malayalam:
                case Language.Mongolian:
                case Language.Marathi:
                case Language.Nepali:
                case Language.Dutch:
                case Language.Norwegian:
                case Language.Oromo:
                case Language.Oriya:
                case Language.Punjabi:
                case Language.Pashto:
                case Language.Portuguese:
                case Language.Somali:
                case Language.Albanian:
                case "sv":
                case "sw":
                case "ta":
                case "te":
                case "tk":
                case "ur":
                case "zu":
                    return (number == 1) ? 0 : 1;
                case "am":
                case "bh":
                case "fil":
                case "fr":
                case "gun":
                case "hi":
                case "hy":
                case "ln":
                case "mg":
                case "nso":
                case "xbr":
                case "ti":
                case "wa":
                    return ((number == 0) || (number == 1)) ? 0 : 1;
                case "be":
                case "bs":
                case "hr":
                case "ru":
                case "sr":
                case "uk":
                    return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);
                case "cs":
                case "sk":
                    return (number == 1) ? 0 : (((number >= 2) && (number <= 4)) ? 1 : 2);
                case "ga":
                    return (number == 1) ? 0 : ((number == 2) ? 1 : 2);
                case "lt":
                    return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);
                case "sl":
                    return (number % 100 == 1) ? 0 : ((number % 100 == 2) ? 1 : (((number % 100 == 3) || (number % 100 == 4)) ? 2 : 3));
                case "mk":
                    return (number % 10 == 1) ? 0 : 1;
                case "mt":
                    return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 1) && (number % 100 < 11))) ? 1 : (((number % 100 > 10) && (number % 100 < 20)) ? 2 : 3));
                case "lv":
                    return (number == 0) ? 0 : (((number % 10 == 1) && (number % 100 != 11)) ? 1 : 2);
                case "pl":
                    return (number == 1) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 12) || (number % 100 > 14))) ? 1 : 2);
                case "cy":
                    return (number == 1) ? 0 : ((number == 2) ? 1 : (((number == 8) || (number == 11)) ? 2 : 3));
                case "ro":
                    return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 0) && (number % 100 < 20))) ? 1 : 2);
                case "ar":
                    return (number == 0) ? 0 : ((number == 1) ? 1 : ((number == 2) ? 2 : (((number % 100 >= 3) && (number % 100 <= 10)) ? 3 : (((number % 100 >= 11) && (number % 100 <= 99)) ? 4 : 5))));
                default:
                    return 0;
            }
        }
    }
}