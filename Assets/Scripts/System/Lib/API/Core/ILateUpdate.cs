using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.API
{
	
	///<summary>延迟刷新</summary>
	public interface ILateUpdate{

		void LateUpdate();
		
	}

}