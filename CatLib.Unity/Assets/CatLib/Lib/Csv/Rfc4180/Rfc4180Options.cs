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

namespace CatLib.Csv
{
    /// <summary>
    /// RFC4180规则配置
    /// </summary>
    internal sealed class Rfc4180Options
    {
        /// <summary>
        /// 引用字符
        /// </summary>
        public char QuoteChar { get; set; }

        /// <summary>
        /// 分隔符
        /// </summary>
        public char DelimiterChar { get; set; }

        /// <summary>
        /// 构建一个RFC4180规则配置
        /// </summary>
        public Rfc4180Options()
        {
            QuoteChar = '"';
            DelimiterChar = ',';
        }
    }
}