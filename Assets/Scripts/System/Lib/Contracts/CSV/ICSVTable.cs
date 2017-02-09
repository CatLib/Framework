using UnityEngine;
using System.Collections;
using System;

namespace CatLib.Contracts.CSV{

	public interface ICSVTable{

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		ICSVSelect Where(string field, string operators = null, string value = null, ECSVLinker linker = ECSVLinker.AND);

		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		ICSVSelect Where(Action<ICSVSelect> nested, ECSVLinker linker = ECSVLinker.AND);

		/// <summary>
		/// 执行一个查询（如果查询不存在则根据输入的参数建立一个查询后执行）
		/// </summary>
		ICSVField[] Get(string column = null , string operators = null, string value = null, ECSVLinker linker = ECSVLinker.AND);
		
		/// <summary>
		/// 执行一个查询（如果查询不存在则根据输入的参数建立一个查询后执行）
		/// </summary>
		T[] Get<T>(string column = null , string operators = null, string value = null, ECSVLinker linker = ECSVLinker.AND);

	}

}
