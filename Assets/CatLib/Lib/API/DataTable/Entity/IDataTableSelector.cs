
using System;

namespace CatLib.API.DataTable{


	public interface IDataTableSelector{

		/// <summary>
		/// 直接获取查询结果
		/// </summary>
		IDataTableResult[] Get();

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		IDataTableSelector Where(string field, string operators, string val, SelectorLinker linker = SelectorLinker.And);
		
		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		IDataTableSelector Where(Action<IDataTableSelector> nested, SelectorLinker linker = SelectorLinker.And);

		/// <summary>
		/// andWhere查询
		/// </summary>
		IDataTableSelector AndWhere(string field, string operators, string val);

		/// <summary>
		/// andWhere 嵌套查询
		/// </summary>
		IDataTableSelector AndWhere(Action<IDataTableSelector> nested);

		/// <summary>
		/// andWhere查询
		/// </summary>
		IDataTableSelector OrWhere(string field, string operators, string val);

		/// <summary>
		/// orWhere 嵌套查询
		/// </summary>
		IDataTableSelector OrWhere(Action<IDataTableSelector> nested);

	}

}