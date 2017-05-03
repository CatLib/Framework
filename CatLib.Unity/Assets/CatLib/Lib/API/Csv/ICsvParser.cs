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

namespace CatLib.API.Csv
{
    /// <summary>
    /// Csv解析器
    /// </summary>
    public interface ICsvParser
    {
        /// <summary>
        /// 解析Csv数据
        /// </summary>
        /// <param name="data">需要被解析的字符串</param>
        /// <returns>返回的二维数组为行和列的数据</returns>
        string[][] Parser(string data);
    }
}
