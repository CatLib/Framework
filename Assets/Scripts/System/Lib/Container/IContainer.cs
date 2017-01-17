using UnityEngine;
using System.Collections;
using System;

namespace CatLib.Container
{
	///<summary>容器接口</summary>
    public interface IContainer
    {

        IContainer Bind(Type from, Func<IContainer, object[], object> to, string name, bool isStatic);

        object Make(Type from, string alias, params object[] param);
        
    }
}