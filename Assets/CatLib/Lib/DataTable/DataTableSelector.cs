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

using System;
using System.Collections.Generic;
using CatLib.API.DataTable;

namespace CatLib.DataTable
{
    /// <summary>
    /// 数据表选择器
    /// </summary>
    public class DataTableSelector : IDataTableSelector
    {
        /// <summary>
        /// 数据表
        /// </summary>
        protected readonly DataTable dataTable;

        /// <summary>
        /// 合法的操作符
        /// </summary>
        protected static readonly string[] operators = {
            "==","=", "<", ">", "<=", ">=", "<>", "!=",
        };

        /// <summary>
        /// 查询条件
        /// </summary>
        protected readonly List<DataTableSelectorData> wheres;

        /// <summary>
        /// 获取查询条件
        /// </summary>
        public DataTableSelectorData[] Wheres
        {
            get { return wheres.ToArray(); }
        }

        /// <summary>
        /// 构建一个条件查询器
        /// </summary>
        /// <param name="dataTable">数据表</param>
        public DataTableSelector(DataTable dataTable)
        {
            this.dataTable = dataTable;
            wheres = new List<DataTableSelectorData>();
        }

        /// <summary>
        /// 直接获取查询结果
        /// </summary>
        public IDataTableResult[] Get()
        {
            return dataTable.Get();
        }

        /// <summary>
        /// 建立一个Where查询
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="operators">操作符</param>
        /// <param name="val">值</param>
        /// <param name="linker">语句链接符</param>
        /// <returns>查询器</returns>
        public IDataTableSelector Where(string field, string operators, string val, SelectorLinker linker = SelectorLinker.And)
        {
            GuardInvalidOperatorAndValue(operators, val);

            if (string.IsNullOrEmpty(val))
            {
                return WhereNull(field, linker, operators != "=");
            }

            wheres.Add(new DataTableSelectorData
            {
                SelectType = "Basic",
                Field = field,
                Operator = operators,
                Value = val,
                Linker = linker
            });

            return this;
        }

        /// <summary>
        /// 建立一个Where嵌套查询
        /// </summary>
        /// <param name="nested">嵌套查询</param>
        /// <param name="linker">链接标识</param>
        /// <returns>查询器</returns>
        public IDataTableSelector Where(Action<IDataTableSelector> nested, SelectorLinker linker = SelectorLinker.And)
        {
            var selector = NewSelector();
            nested(selector);
            return NestedWhereSelector(selector, linker);
        }

        /// <summary>
        /// 以And链接符进行Where查询
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="operators">操作符</param>
        /// <param name="val">操作值</param>
        /// <returns>查询器</returns>
        public IDataTableSelector AndWhere(string field, string operators, string val)
        {
            return Where(field, operators, val);
        }

        /// <summary>
        /// 以And链接符进行Where嵌套查询
        /// </summary>
        /// <param name="nested">嵌套查询</param>
        public IDataTableSelector AndWhere(Action<IDataTableSelector> nested)
        {
            return Where(nested);
        }

        /// <summary>
        /// 以Or链接符进行Where查询
        /// </summary>
        public IDataTableSelector OrWhere(string field, string operators = null, string val = null)
        {
            return Where(field, operators, val, SelectorLinker.Or);
        }

        /// <summary>
        /// 以Or链接符进行Where嵌套查询
        /// </summary>
        /// <param name="nested">嵌套查询</param>
        public IDataTableSelector OrWhere(Action<IDataTableSelector> nested)
        {
            return Where(nested, SelectorLinker.Or);
        }

        /// <summary>
        /// 是否为为 null 的查询
        /// </summary>
        /// <param name="field">嵌套查询</param>
        /// <param name="linker">链接符</param>
        /// <param name="negate">是否取反</param>
        protected IDataTableSelector WhereNull(string field, SelectorLinker linker = SelectorLinker.And, bool negate = false)
        {
            wheres.Add(new DataTableSelectorData
            {
                SelectType = negate ? "NotNull" : "Null",
                Field = field,
                Linker = linker
            });
            return this;
        }

        /// <summary>
        /// 增加一个嵌套查询
        /// </summary>
        /// <param name="selector">查询器</param>
        /// <param name="linker">链接符</param>
        /// <returns>查询器</returns>
        protected IDataTableSelector NestedWhereSelector(DataTableSelector selector, SelectorLinker linker)
        {
            wheres.Add(new DataTableSelectorData
            {
                SelectType = "Nested",
                Selector = selector,
                Linker = linker
            });
            return this;
        }

        /// <summary>
        /// 获取一个新的查询器
        /// </summary>
        protected DataTableSelector NewSelector()
        {
            return new DataTableSelector(dataTable);
        }

        /// <summary>
        /// 检测无效的操作符和值
        /// </summary>
        /// <param name="operators">操作符</param>
        /// <param name="val">操作符的值</param>
        protected void GuardInvalidOperatorAndValue(string operators, string val)
        {
            var isOperator = operators.Contains(operators);
            if (!isOperator)
            {
                throw new ArgumentException("illegal operator combination.");
            }
        }
    }
}