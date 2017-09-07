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
    /// 翻译映射
    /// </summary>
    public interface ITranslateResources
    {
        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="locale">语言</param>
        /// <param name="key">键</param>
        /// <param name="str">返回的值</param>
        /// <returns>是否成功获取</returns>
        bool TryGetValue(string locale, string key, out string str);
    }
}