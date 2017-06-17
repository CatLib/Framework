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
    public interface ITranslatorMapping
    {
        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="segments">片段</param>
        /// <param name="str">返回的值</param>
        /// <param name="def">默认值</param>
        /// <returns>是否成功获取</returns>
        bool TryGetValue(string[] segments, out string str, string def = null);
    }
}