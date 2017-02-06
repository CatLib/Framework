using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.Contracts.Base
{
	
	[LuaCallCSharp]
	///<summary>延迟刷新</summary>
	public interface ILateUpdate{

		void LateUpdate();
		
	}

}