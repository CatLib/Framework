using UnityEngine;
using System.Collections;
using System;

namespace CatLib.API.Csv{

	public interface ICsvTable{

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		ICsvSelect Where(string field, string operators = null, string value = null, ECsvLinker linker = ECsvLinker.And);

		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		ICsvSelect Where(Action<ICsvSelect> nested, ECsvLinker linker = ECsvLinker.And);

		/// <summary>
		/// 执行一个查询（如果查询不存在则根据输入的参数建立一个查询后执行）
		/// </summary>
		ICsvField[] Get(string column = null , string operators = null, string value = null, ECsvLinker linker = ECsvLinker.And);
		
		/// <summary>
		/// 执行一个查询（如果查询不存在则根据输入的参数建立一个查询后执行）
		/// </summary>
		T[] Get<T>(string column = null , string operators = null, string value = null, ECsvLinker linker = ECsvLinker.And);

	}

}
