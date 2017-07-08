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

namespace CatLib.Translation
{
    /// <summary>
    /// 选择器
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// 对翻译进行处理
        /// </summary>
        /// <param name="line">语言字符串</param>
        /// <param name="number">数量</param>
        /// <param name="locale">语言</param>
        /// <returns>处理后的字符串</returns>
        string Choose(string line, int number, string locale);
    }
}