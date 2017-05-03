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
    /// DataTable
    /// </summary>
    public sealed class DataTable : IDataTable
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Dictionary<string, int> title;

        /// <summary>
        /// 查询集合(不含头)
        /// </summary>
        private List<DataTableResult> dataTableResult;

        /// <summary>
        /// 选择器
        /// </summary>
        private DataTableSelector selector;

        /// <summary>
        /// 设定数据
        /// </summary>
        /// <param name="result">结果集</param>
        public IDataTable SetData(string[][] result)
        {
            GuardResult(result);
            ExtractionTitle(result);
            ExtractionContent(result);
            return this;
        }

        /// <summary>
        /// 将数据表转为数组
        /// </summary>
        /// <returns>将数据表转为数组</returns>
        public string[][] ToArray()
        {
            if (title == null || dataTableResult == null)
            {
                return null;
            }

            var returnData = new List<string[]>();
            var head = new string[title.Keys.Count];

            title.Keys.CopyTo(head, 0);
            returnData.Add(head);

            for (var i = 0; i < dataTableResult.Count; i++)
            {
                returnData.Add(dataTableResult[i].Column);
            }

            return returnData.ToArray();
        }

        /// <summary>
        /// 获取标题对应的下标
        /// </summary>
        /// <param name="field">标题</param>
        /// <returns>下标</returns>
        public int GetIndex(string field)
        {
            if (title == null)
            {
                return -1;
            }

            if (title.ContainsKey(field))
            {
                return title[field];
            }

            return -1;
        }

        /// <summary>
        /// 从结果集提取标题
        /// </summary>
        /// <param name="result">结果集</param>
        private void ExtractionTitle(IList<string[]> result)
        {
            title = new Dictionary<string, int>();
            var titleRow = result[0];

            for (var i = 0; i < titleRow.Length; i++)
            {
                title.Add(titleRow[i], i);
            }
        }

        /// <summary>
        /// 从结果集提取内容
        /// </summary>
        /// <param name="result">结果集</param>
        private void ExtractionContent(IList<string[]> result)
        {
            dataTableResult = new List<DataTableResult>();
            for (var i = 1; i < result.Count; i++)
            {
                if (result[i].Length != title.Count)
                {
                    continue;
                }
                dataTableResult.Add(new DataTableResult(this, result[i]));
            }
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
            selector = new DataTableSelector(this);
            return selector.Where(field, operators, val, linker);
        }

        /// <summary>
        /// 建立一个Where嵌套查询
        /// </summary>
        /// <param name="nested">嵌套查询</param>
        /// <param name="linker">链接标识</param>
        /// <returns>查询器</returns>
        public IDataTableSelector Where(Action<IDataTableSelector> nested, SelectorLinker linker = SelectorLinker.And)
        {
            selector = new DataTableSelector(this);
            return selector.Where(nested, linker);
        }

        /// <summary>
        /// 执行一个查询获取查询结果集
        /// </summary>
        public IDataTableResult[] Get()
        {
            if (dataTableResult == null)
            {
                return null;
            }

            if (selector == null)
            {
                return dataTableResult.ToArray();
            }

            var result = Parser();
            selector = null;
            return result;
        }

        /// <summary>
        /// 根据下标获取一行记录
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>一行记录</returns>
        public IDataTableResult Get(int index)
        {
            if (dataTableResult == null)
            {
                return null;
            }
            return dataTableResult.Count <= index ? null : dataTableResult[index];
        }

        /// <summary>
        /// 执行一个查询获取结果集
        /// </summary>
        private IDataTableResult[] Parser()
        {
            var result = new List<DataTableResult>();
            var wheres = selector.Wheres;
            for (var i = 0; i < dataTableResult.Count; i++)
            {
                if (Filter(dataTableResult[i], wheres))
                {
                    result.Add(dataTableResult[i]);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 检查结果集是否符合条件如果不符合应该返回false
        /// </summary>
        private bool Filter(DataTableResult row, DataTableSelectorData[] wheres)
        {
            bool statu = true, statuNext;
            for (var i = 0; i < wheres.Length; i++)
            {
                statuNext = QueryWhere(row, wheres[i]);
                switch (wheres[i].Linker)
                {
                    case SelectorLinker.And:
                        statu = statu && statuNext;
                        break;
                    case SelectorLinker.Or:
                        if (statu) { return true; }
                        statu = statuNext;
                        break;
                    default:
                        throw new Exception("undefined selector linker :" + wheres[i].Linker);
                }
            }
            return statu;
        }

        /// <summary>
        /// 执行Where
        /// </summary>
        /// <param name="row">一行记录</param>
        /// <param name="wheres">查询条件</param>
        /// <returns>是否符合条件</returns>
        private bool QueryWhere(DataTableResult row, DataTableSelectorData wheres)
        {
            switch (wheres.SelectType)
            {
                case "Nested":
                    return WhereNested(row, wheres);
                case "Basic":
                    return WhereBasic(row, wheres);
                case "NotNull":
                    return WhereNotNull(row, wheres);
                case "Null":
                    return WhereNull(row, wheres);
                default:
                    return false;
            }
        }

        /// <summary>
        /// 嵌套查询
        /// </summary>
        /// <param name="row">一行记录</param>
        /// <param name="wheres">查询条件</param>
        /// <returns>是否符合条件</returns>
        private bool WhereNested(DataTableResult row, DataTableSelectorData wheres)
        {
            return Filter(row, wheres.Selector.Wheres);
        }

        /// <summary>
        /// 基础操作符查询
        /// </summary>
        /// <param name="row">一行记录</param>
        /// <param name="wheres">查询条件</param>
        /// <returns>是否符合条件</returns>
        private bool WhereBasic(DataTableResult row, DataTableSelectorData wheres)
        {
            switch (wheres.Operator)
            {
                case "==":
                case "=":
                    return row[wheres.Field] == wheres.Value;
                case "<":
                    return int.Parse(row[wheres.Field]) < int.Parse(wheres.Value);
                case ">":
                    return int.Parse(row[wheres.Field]) > int.Parse(wheres.Value);
                case "<=":
                    return int.Parse(row[wheres.Field]) <= int.Parse(wheres.Value);
                case ">=":
                    return int.Parse(row[wheres.Field]) >= int.Parse(wheres.Value);
                case "<>":
                case "!=":
                    return row[wheres.Field] != wheres.Value;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 空查询
        /// </summary>
        /// <param name="row">一行记录</param>
        /// <param name="wheres">查询条件</param>
        /// <returns>是否符合条件</returns>
        private bool WhereNull(DataTableResult row, DataTableSelectorData wheres)
        {
            return string.IsNullOrEmpty(row[wheres.Field]);
        }

        /// <summary>
        /// 非空查询
        /// </summary>
        /// <param name="row">一行记录</param>
        /// <param name="wheres">查询条件</param>
        /// <returns>是否符合条件</returns>
        private bool WhereNotNull(DataTableResult row, DataTableSelectorData wheres)
        {
            return !WhereNull(row, wheres);
        }

        /// <summary>
        /// 结果集验证
        /// </summary>
        /// <param name="result">结果集</param>
        private void GuardResult(string[][] result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("illegal result", "result");
            }

            if (result.Length <= 0)
            {
                throw new ArgumentException("illegal result", "result");
            }
        }
    }
}
