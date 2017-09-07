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
using System;

namespace CatLib.Translation
{
    /// <summary>
    /// 国际化(I18N)
    /// 语言代码使用 ISO 639, ISO 639-1, ISO 639-2, ISO 639-3 标准
    /// </summary>
    public sealed class Translator : ITranslator
    {
        /// <summary>
        /// 消息选择器
        /// </summary>
        private ISelector selector;

        /// <summary>
        /// 当前语言
        /// </summary>
        private string locale;

        /// <summary>
        /// 备选语言
        /// </summary>
        private string fallback;

        /// <summary>
        /// 翻译翻译资源
        /// </summary>
        private ITranslateResources resources;

        /// <summary>
        /// 设定翻译资源
        /// </summary>
        /// <param name="map">映射</param>
        public void SetResources(ITranslateResources map)
        {
            resources = map;
        }

        /// <summary>
        /// 设定替补语言
        /// </summary>
        /// <param name="fallback">替补语言</param>
        public void SetFallback(string fallback)
        {
            this.fallback = fallback;
        }

        /// <summary>
        /// 设定消息选择器
        /// </summary>
        /// <param name="selector">选择器</param>
        public void SetSelector(ISelector selector)
        {
            this.selector = selector;
        }

        /// <summary>
        /// 依次遍历给定的语言获取翻译,翻译根据传入数量使用指定复数形式,如果都没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">数量</param>
        /// <param name="locales">遍历的语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译后的内容</returns>
        public string GetBy(string key, int number, string[] locales, params string[] replace)
        {
            foreach (var locale in locales)
            {
                var line = Choice(key, number, locale, replace);
                if (line != null)
                {
                    return line;
                }
            }
            return Choice(key, number, fallback, replace) ?? string.Empty;
        }

        /// <summary>
        /// 依次遍历给定的语言获取翻译,如果都没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="locales">多语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的内容</returns>
        public string GetBy(string key, string[] locales, params string[] replace)
        {
            foreach (var locale in locales)
            {
                var line = GetLine(key, locale, replace);
                if (line != null)
                {
                    return line;
                }
            }
            return GetLine(key, fallback, replace) ?? string.Empty;
        }

        /// <summary>
        /// 从指定的语言获取翻译,翻译根据传入数量使用指定复数形式,如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">语言</param>
        /// <param name="locale">指定语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译后的内容</returns>
        public string GetBy(string key, int number, string locale, params string[] replace)
        {
            var line = Choice(key, number, locale, replace) ?? Choice(key, number, fallback, replace);
            return line ?? string.Empty;
        }

        /// <summary>
        /// 从指定的语言获取翻译,如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="locale">语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的内容</returns>
        public string GetBy(string key, string locale, params string[] replace)
        {
            var line = GetLine(key, locale, replace) ?? GetLine(key, fallback, replace);
            return line ?? string.Empty;
        }

        /// <summary>
        /// 在当前语言环境下翻译内容，如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        public string Get(string key, params string[] replace)
        {
            var line = GetLine(key, locale, replace) ?? GetLine(key, fallback, replace);
            return line ?? string.Empty;
        }

        /// <summary>
        /// 在当前语言环境下翻译带有数量的内容，如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">数值</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        public string Get(string key, int number, params string[] replace)
        {
            var line = Choice(key, number, locale, replace) ?? Choice(key, number, fallback, replace);
            return line ?? string.Empty;
        }

        /// <summary>
        /// 获取当前语言环境
        /// </summary>
        /// <returns></returns>
        public string GetLocale()
        {
            return locale;
        }

        /// <summary>
        /// 设定当前语言环境
        /// </summary>
        /// <param name="locale">设定默认本地语言(语言代码使用 ISO 639, ISO 639-1, ISO 639-2, ISO 639-3 标准)</param>
        public void SetLocale(string locale)
        {
            this.locale = locale;
        }

        /// <summary>
        /// 选择性翻译（选择合适的复数形式进行翻译）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">值</param>
        /// <param name="locale">语言</param>
        /// <param name="replace">替换的内容</param>
        /// <returns>翻译的值</returns>
        private string Choice(string key, int number, string locale, string[] replace)
        {
            if (string.IsNullOrEmpty(locale))
            {
                return null;
            }

            var line = GetLine(key, locale, replace);
            if (line == null)
            {
                return null;
            }

            var newReplace = new string[replace.Length + 2];
            Array.Copy(replace, newReplace, replace.Length);
            newReplace[replace.Length] = "count";
            newReplace[replace.Length + 1] = number.ToString();

            if (selector != null)
            {
                line = selector.Choose(line, number, locale);
            }
            return MakeReplacements(line, newReplace);
        }

        /// <summary>
        /// 获取一行数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="locale">当前语言</param>
        /// <param name="replace">替换的值</param>
        /// <returns>翻译的值</returns>
        private string GetLine(string key, string locale, string[] replace)
        {
            if (resources == null)
            {
                throw new RuntimeException("Undefiend lanuages resources , Please call SetResources()");
            }

            if (string.IsNullOrEmpty(locale))
            {
                return null;
            }

            string line;
            resources.TryGetValue(locale, key, out line);
            return line != null ? MakeReplacements(line, replace) : null;
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="line">字符串</param>
        /// <param name="replace">替换的内容</param>
        private string MakeReplacements(string line, string[] replace)
        {
            if (line == null)
            {
                return null;
            }
            if (replace.Length <= 0)
            {
                return line;
            }

            string[] tmp;
            string replaceLeft, replaceRight;
            for (var i = 0; i < replace.Length;)
            {
                tmp = replace[i].Split(':');
                if (tmp.Length == 2)
                {
                    replaceLeft = tmp[0];
                    replaceRight = tmp[1];
                    i += 1;
                }
                else
                {
                    if (i + 1 >= replace.Length)
                    {
                        break;
                    }
                    replaceLeft = replace[i];
                    replaceRight = replace[i + 1];
                    i += 2;
                }

                line = line.Replace(":" + replaceLeft, replaceRight);
                line = line.Replace(":" + replaceLeft.ToUpper(), replaceRight.ToUpper());
                if (replaceRight.Length >= 1 && replaceRight.Length >= 1)
                {
                    line = line.Replace(":" + replaceLeft.Substring(0, 1).ToUpper() + replaceLeft.Substring(1), replaceRight.Substring(0, 1).ToUpper() + replaceRight.Substring(1));
                }
            }
            return line;
        }
    }
}