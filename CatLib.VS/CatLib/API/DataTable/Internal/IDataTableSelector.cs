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
    /// 数据表查询器
    /// </summary>
    public interface IDataTableSelector
    {
        /// <summary>
        /// 直接获取查询结果
        /// </summary>
        IDataTableResult[] Get();

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
        /// 以And链接符进行Where查询
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="operators">操作符</param>
        /// <param name="val">操作值</param>
        /// <returns>查询器</returns>
        IDataTableSelector AndWhere(string field, string operators, string val);

        /// <summary>
        /// 以And链接符进行Where嵌套查询
        /// </summary>
        /// <param name="nested">嵌套查询</param>
        IDataTableSelector AndWhere(Action<IDataTableSelector> nested);

        /// <summary>
        /// 以Or链接符进行Where查询
        /// </summary>
        IDataTableSelector OrWhere(string field, string operators, string val);

        /// <summary>
        /// 以Or链接符进行Where嵌套查询
        /// </summary>
        /// <param name="nested">嵌套查询</param>
        IDataTableSelector OrWhere(Action<IDataTableSelector> nested);
    }
}