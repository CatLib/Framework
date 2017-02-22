using UnityEngine;
using System.Collections;
using System;

namespace CatLib.API.CSV{

	/// <summary>
	/// CSV查询器接口
	/// </summary>
	public interface ICSVSelect{

		/// <summary>
		/// 直接获取查询结果
		/// </summary>
		ICSVField[] Get();

		/// <summary>
		/// 直接获取查询结果
		/// </summary>
		T[] Get<T>();

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		ICSVSelect Where(string field, string operators = null, string value = null, ECSVLinker linker = ECSVLinker.AND);
		
		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		ICSVSelect Where(Action<ICSVSelect> nested, ECSVLinker linker = ECSVLinker.AND);

		/// <summary>
		/// andWhere查询
		/// </summary>
		ICSVSelect AndWhere(string field, string operators = null, string value = null);

		/// <summary>
		/// andWhere 嵌套查询
		/// </summary>
		ICSVSelect Where(Action<ICSVSelect> nested);

		/// <summary>
		/// andWhere查询
		/// </summary>
		ICSVSelect OrWhere(string field, string operators = null, string value = null);

		/// <summary>
		/// orWhere 嵌套查询
		/// </summary>
		ICSVSelect OrWhere(Action<ICSVSelect> nested);

	}

}
