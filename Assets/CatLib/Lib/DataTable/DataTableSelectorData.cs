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

namespace CatLib.DataTable{

	/// <summary>
	/// 查询数据
	/// </summary>
	public class DataTableSelectorData{

		/// <summary>
		/// 查询类型
		/// </summary>
		public string SelectType{ get; set; }

		/// <summary>
		/// 查询字段
		/// </summary>
		public string Field{ get; set; }

		/// <summary>
		/// 查询操作符
		/// </summary>
		public string Operator{ get; set; }

		/// <summary>
		/// 查询目标值
		/// </summary>
		public string Value{ get; set; }

		/// <summary>
		/// 查询器
		/// </summary>
		public DataTableSelector Selector{ get; set; }

		/// <summary>
		/// 查询链接标识
		/// </summary>
		public SelectorLinker Linker{ get; set; }
		
	}

}