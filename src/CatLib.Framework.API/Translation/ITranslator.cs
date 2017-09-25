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

namespace CatLib.API.Translation
{
    /// <summary>
    /// 国际化(I18N)
    /// 语言代码使用 ISO 639, ISO 639-1, ISO 639-2, ISO 639-3 标准
    /// </summary>
    public interface ITranslator
    {
        /// <summary>
        /// 设定翻译资源
        /// </summary>
        /// <param name="map">翻译资源</param>
        void SetResources(ITranslateResources map);

        /// <summary>
        /// 在当前语言环境下翻译内容，如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        string Get(string key, params string[] replace);

        /// <summary>
        /// 在当前语言环境下翻译带有数量的内容，如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">数值</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        string Get(string key, int number, params string[] replace);

        /// <summary>
        /// 依次遍历给定的语言获取翻译,如果都没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="locales">多语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的内容</returns>
        string GetBy(string key, string[] locales, params string[] replace);

        /// <summary>
        /// 依次遍历给定的语言获取翻译,翻译根据传入数量使用指定复数形式,如果都没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">数量</param>
        /// <param name="locales">遍历的语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译后的内容</returns>
        string GetBy(string key, int number, string[] locales, params string[] replace);

        /// <summary>
        /// 从指定的语言获取翻译,如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="locale">语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的内容</returns>
        string GetBy(string key, string locale, params string[] replace);

        /// <summary>
        /// 从指定的语言获取翻译,翻译根据传入数量使用指定复数形式,如果没有命中则使用替补语言
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">语言</param>
        /// <param name="locale">指定语言</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译后的内容</returns>
        string GetBy(string key, int number, string locale, params string[] replace);

        /// <summary>
        /// 获取当前语言环境
        /// </summary>
        /// <returns>当前语言</returns>
        string GetLocale();

        /// <summary>
        /// 设定当前语言环境
        /// </summary>
        /// <param name="locale">当前语言(语言代码使用 ISO 639, ISO 639-1, ISO 639-2, ISO 639-3 标准)</param>
        void SetLocale(string locale);

        /// <summary>
        /// 设定替补语言
        /// </summary>
        /// <param name="fallback">替补语言(语言代码使用 ISO 639, ISO 639-1, ISO 639-2, ISO 639-3 标准)</param>
        void SetFallback(string fallback);
    }
}