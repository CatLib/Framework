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
using System.Collections.Generic;
using CatLib.API;
using CatLib.API.Translation;
using CatLib.Stl;

namespace CatLib.Translation
{
    /// <summary>
    /// 国际化
    /// </summary>
    public sealed class Translator : ITranslator
    {
        /// <summary>
        /// 片段分隔符
        /// </summary>
        private static readonly char[] Segments = { '.' , ':' , '/' };
        
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
        /// 已经被转义过的key缓存
        /// </summary>
        private readonly LruCache<string, string[]> parsed;

        /// <summary>
        /// 翻译映射
        /// </summary>
        private readonly SortSet<ITranslatorMapping, int> maps;

        /// <summary>
        /// 构建一个国际化组件
        /// </summary>
        public Translator()
        {
            maps = new SortSet<ITranslatorMapping, int>();
            parsed = new LruCache<string, string[]>(64);
        }

        /// <summary>
        /// 增加翻译资源映射
        /// </summary>
        /// <param name="map">映射</param>
        /// <param name="priority">优先级</param>
        public void AddMapping(ITranslatorMapping map , int priority)
        {
            maps.Add(map, priority);
        }

        /// <summary>
        /// 设定替补语言
        /// </summary>
        /// <param name="fallback">替补语言</param>
        public void SetFallback(string fallback)
        {
            if (!string.IsNullOrEmpty(fallback))
            {
                this.fallback = fallback;
            }
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
        /// 翻译内容
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        public string Trans(string key, params string[] replace)
        {
            var line = Get(locale, key, replace) ?? Get(fallback, key, replace);
            return line ?? string.Empty;
        }

        /// <summary>
        /// 翻译内容的复数形式
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">数值</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        public string TransChoice(string key, int number, params string[] replace)
        {
            return Choice(key, number, replace);
        }

        /// <summary>
        /// 获取默认本地语言
        /// </summary>
        /// <returns></returns>
        public string GetLocale()
        {
            return locale;
        }

        /// <summary>
        /// 设定默认本地语言
        /// </summary>
        /// <param name="locale">设定默认本地语言</param>
        public void SetLocale(string locale)
        {
            this.locale = locale;
        }

        /// <summary>
        /// 执行翻译
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">值</param>
        /// <param name="replace">替换的内容</param>
        /// <returns>翻译的值</returns>
        private string Choice(string key, int number, string[] replace)
        {
            var locale = this.locale;
            var line = Get(locale, key, replace);
            if (line == null)
            {
                line = Get(fallback, key, replace);
                locale = fallback;
            }

            replace = new[] { "count", number.ToString() };

            return MakeReplacements(selector.Choose(line, number, locale), replace);
        }

        /// <summary>
        /// 获取翻译
        /// </summary>
        /// <param name="locale">当前语言</param>
        /// <param name="key">键</param>
        /// <param name="replace">替换的值</param>
        /// <returns>翻译的值</returns>
        private string Get(string locale, string key, string[] replace)
        {
            return GetLine(ParseKey(key), locale, replace);
        }

        /// <summary>
        /// 获取一行数据
        /// </summary>
        /// <param name="segments">key的片段</param>
        /// <param name="locale">当前语言</param>
        /// <param name="replace">替换的值</param>
        /// <returns>翻译的值</returns>
        private string GetLine(string[] segments, string locale, string[] replace)
        {
            string line = null;
            foreach (var map in maps)
            {
                if (map.TryGetValue(segments, out line))
                {
                    break;
                }
            }

            return line != null ? MakeReplacements(line, replace) : line;
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

        /// <summary>
        /// 格式化key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>key的片段</returns>
        private string[] ParseKey(string key)
        {
            string[] segments;
            if (parsed.Get(key, out segments))
            {
                return segments;
            }

            segments = key.Split(Segments, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 1)
            {
                parsed.Add(key, segments);
                return segments;
            }
            throw new RuntimeException("translator key is invalid");
        }
    }
}