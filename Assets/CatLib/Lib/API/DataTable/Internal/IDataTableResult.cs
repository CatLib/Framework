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

namespace CatLib.API.DataTable
{
    /// <summary>
    /// 数据表查询结果
    /// </summary>
    public interface IDataTableResult
    {
        /// <summary>
        /// 获取指定字段的记录
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns>字段中的值</returns>
        string Get(string field);

        /// <summary>
        /// 获取指定字段的记录
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns>字段中的值</returns>
        string this[string field] { get; }
    }
}