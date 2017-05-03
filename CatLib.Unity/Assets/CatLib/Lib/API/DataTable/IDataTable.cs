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

namespace CatLib.API.DataTable
{
    /// <summary>
    /// 数据表
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// 建立一个Where查询
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="operators">操作符</param>
        /// <param name="val">值</param>
        /// <param name="linker">语句链接符</param>
        /// <returns>查询器</returns>
        IDataTableSelector Where(string field, string operators, string val, SelectorLinker linker = SelectorLinker.And);

        /// <summary>
        /// 建立一个Where嵌套查询
        /// </summary>
        /// <param name="nested">嵌套查询</param>
        /// <param name="linker">链接标识</param>
        /// <returns>查询器</returns>
        IDataTableSelector Where(Action<IDataTableSelector> nested, SelectorLinker linker = SelectorLinker.And);

        /// <summary>
        /// 执行一个查询获取查询结果集
        /// </summary>
        IDataTableResult[] Get();

        /// <summary>
        /// 根据下标获取一行记录
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>一行记录</returns>
        IDataTableResult Get(int index);

        /// <summary>
        /// 设定数据
        /// </summary>
        /// <param name="result">结果集</param>
	    IDataTable SetData(string[][] result);

        /// <summary>
        /// 将数据表转为数组
        /// </summary>
        /// <returns>将数据表转为数组</returns>
        string[][] ToArray();
    }
}