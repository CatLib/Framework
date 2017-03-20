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

namespace CatLib.API.DataTable{

	public interface IDataTable {

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		IDataTableSelector Where(string field, string operators, string val, SelectorLinker linker = SelectorLinker.And);

		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		IDataTableSelector Where(Action<IDataTableSelector> nested, SelectorLinker linker = SelectorLinker.And);

		/// <summary>
		/// 执行一个查询（如果查询不存在则根据输入的参数建立一个查询后执行）
		/// </summary>
		IDataTableResult[] Get();

		/// <summary>
		/// 根据一个下标获取结果
		/// </summary>
		IDataTableResult Get(int index);

        /// <summary>
        /// 转为数组
        /// </summary>
        /// <returns></returns>
        string[][] ToArray();
		

	}

}