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
    /// 结果集中的一条数据
    /// </summary>
    public sealed class DataTableResult : IDataTableResult
    {
        /// <summary>
        /// 所属数据表
        /// </summary>
        private readonly DataTable table;

        /// <summary>
        /// 列
        /// </summary>
        private readonly string[] column;

        /// <summary>
        /// 列
        /// </summary>
        public string[] Column
        {
            get { return column; }
        }

        /// <summary>
        /// 创建一条记录
        /// </summary>
        /// <param name="table">所属数据表</param>
        /// <param name="column">一条记录</param>
        public DataTableResult(DataTable table, string[] column)
        {
            this.table = table;
            this.column = column;
        }

        /// <summary>
        /// 获取指定字段的记录
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns>字段中的值</returns>
        public string Get(string field)
        {
            var index = table.GetIndex(field);
            return index == -1 ? null : column[index];
        }

        /// <summary>
        /// 获取指定字段的记录
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns>字段中的值</returns>
        public string this[string field]
        {
            get
            {
                return Get(field);
            }
        }
    }
}