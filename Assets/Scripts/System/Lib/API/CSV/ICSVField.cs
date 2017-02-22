using UnityEngine;
using System.Collections;

namespace CatLib.API.CSV{

	public interface ICSVField{

		/// <summary>
		/// 获取指定字段的内容
		/// </summary>
		string Get(string field);
		
	}

}