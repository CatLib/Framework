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

using System.Collections.Generic;
using CatLib.API.Translation;

namespace CatLib.Translation
{
    /// <summary>
    /// 国际化
    /// </summary>
    public sealed class Translator : ITranslator
    {
        /// <summary>
        /// 已经被加载的语言集(locale , file , IFileMapping)
        /// </summary>
        private readonly Dictionary<string, IFileMapping> loaded;

        /// <summary>
        /// 消息选择器
        /// </summary>
        private ISelector selector;

        /// <summary>
        /// 根目录
        /// </summary>
        private string root;

        /// <summary>
        /// 当前语言
        /// </summary>
        private string locale;

        /// <summary>
        /// 备选语言
        /// </summary>
        private string fallback;

        /// <summary>
        /// 加载器
        /// </summary>
        private IFileLoader loader;

        /// <summary>
        /// 已经被转义过的key缓存
        /// </summary>
        private readonly Dictionary<string, string[]> parsed;

        /// <summary>
        /// 设定根目录
        /// </summary>
        /// <param name="root">根目录</param>
        public void SetRoot(string root)
        {
            if (!string.IsNullOrEmpty(root))
            {
                this.root = root;
            }
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
        /// 构建一个国际化组件
        /// </summary>
        public Translator()
        {
            loaded = new Dictionary<string, IFileMapping>();
            parsed = new Dictionary<string, string[]>();
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
        /// 设定文件加载器
        /// </summary>
        /// <param name="loader">加载器</param>
        public void SetFileLoader(IFileLoader loader)
        {
            this.loader = loader;
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
            //segments: file , key
            var segments = ParseKey(key);

            return GetLine(segments[0], segments[1], locale, replace);
        }

        /// <summary>
        /// 获取一行数据
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="key">键</param>
        /// <param name="locale">当前语言</param>
        /// <param name="replace">替换的值</param>
        /// <returns>翻译的值</returns>
        private string GetLine(string file, string key, string locale, string[] replace)
        {
            Load(file, key, locale);

            if (loaded[locale + "." + file] == null)
            {
                return null;
            }

            var line = loaded[locale + "." + file].Get(key);

            return MakeReplacements(line, replace);
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
        /// 加载数据
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="key">键</param>
        /// <param name="locale">当前语言</param>
        private void Load(string file, string key, string locale)
        {
            if (loaded.ContainsKey(locale + "." + file))
            {
                return;
            }

            var mapping = loader.Load(root, locale, file);
            loaded.Add(locale + "." + file, mapping);
        }

        /// <summary>
        /// 格式化key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>解析的结果为file 和 key</returns>
        private string[] ParseKey(string key)
        {
            if (parsed.ContainsKey(key))
            {
                return parsed[key];
            }

            var segments = new string[2];

            var keySegments = key.Split('.');
            if (keySegments.Length > 1)
            {
                segments[0] = keySegments[0];
                segments[1] = keySegments[1];
            }
            else
            {
                throw new System.Exception("translator key is invalid");
            }

            return segments;
        }
    }
}