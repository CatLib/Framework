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
    /// 使用的标准
    /// </summary>
    public interface IStandard
    {
        /// <summary>
        /// 解析一行数据
        /// </summary>
        /// <param name="line">一行数据的数据内容</param>
        /// <returns>数组的每个元素为行中的列</returns>
        string[] Parse(string line);
    }
}