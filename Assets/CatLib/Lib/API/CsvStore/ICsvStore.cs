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

using CatLib.API.DataTable;

namespace CatLib.API.CsvStore
{
    /// <summary>
    /// Csv存储容器
    /// </summary>
    public interface ICsvStore
    {
        /// <summary>
        /// 获取一个数据表
        /// </summary>
        /// <param name="table">数据表表名</param>
        /// <returns>数据表</returns>
        IDataTable Get(string table);

        /// <summary>
        /// 获取一个数据表
        /// </summary>
        /// <param name="table">数据表表名</param>
        /// <returns>数据表</returns>
        IDataTable this[string table] { get; }
    }
}
