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

namespace CatLib.DataTable
{
    /// <summary>
    /// Datatable构建器
    /// </summary>
    public sealed class DataTableFactory : IDataTableFactory
    {
        /// <summary>
        /// 构建Datatable
        /// </summary>
        /// <param name="result">结果集</param>
        /// <returns>数据表</returns>
        public IDataTable Make(string[][] result)
        {
            return new DataTable(result);
        }
    }
}