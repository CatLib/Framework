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
    /// 翻译(国际化)
    /// </summary>
    public interface ITranslator
    {
        /// <summary>
        /// 翻译内容
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        string Trans(string key, params string[] replace);

        /// <summary>
        /// 翻译内容的复数形式
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="number">数值</param>
        /// <param name="replace">替换翻译内容的占位符</param>
        /// <returns>翻译的值</returns>
        string TransChoice(string key, int number, params string[] replace);

        /// <summary>
        /// 获取默认本地语言
        /// </summary>
        /// <returns></returns>
        string GetLocale();

        /// <summary>
        /// 设定默认本地语言
        /// </summary>
        /// <param name="local">设定默认本地语言</param>
        void SetLocale(string local);
    }
}