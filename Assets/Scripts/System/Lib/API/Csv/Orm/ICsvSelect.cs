using UnityEngine;
using System.Collections;
using System;

namespace CatLib.API.Csv{

	/// <summary>
	/// CSV查询器接口
	/// </summary>
	public interface ICsvSelect{

		/// <summary>
		/// 直接获取查询结果
		/// </summary>
		ICsvField[] Get();

		/// <summary>
		/// 直接获取查询结果
		/// </summary>
		T[] Get<T>();

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		ICsvSelect Where(string field, string operators = null, string value = null, ECsvLinker linker = ECsvLinker.And);
		
		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		ICsvSelect Where(Action<ICsvSelect> nested, ECsvLinker linker = ECsvLinker.And);

		/// <summary>
		/// andWhere查询
		/// </summary>
		ICsvSelect AndWhere(string field, string operators = null, string value = null);

		/// <summary>
		/// andWhere 嵌套查询
		/// </summary>
		ICsvSelect Where(Action<ICsvSelect> nested);

		/// <summary>
		/// andWhere查询
		/// </summary>
		ICsvSelect OrWhere(string field, string operators = null, string value = null);

		/// <summary>
		/// orWhere 嵌套查询
		/// </summary>
		ICsvSelect OrWhere(Action<ICsvSelect> nested);

	}

}
