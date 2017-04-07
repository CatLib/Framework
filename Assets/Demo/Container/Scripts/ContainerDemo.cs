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

using UnityEngine;
using CatLib.API.Container;

namespace CatLib.Demo.Container{

public class ContainerDemo : MonoBehaviour {


	/**
	 * Container 是一个特殊的组件，一般情况下CatLib提供的App就继承自Container实例。
	 * 但您也可以自建自己的Container.
	 */
	public void Start () {
		
		CatLib.Container.Container container = new CatLib.Container.Container();

		container.Resolving((app , bind, obj)=>{

			UnityEngine.Debug.Log("(Global) Container.Resolving() , " + obj.GetType());
			return obj;

		});

		NormalBindDemo(container);

	}


	public class NormalBindDemoClass{

		public void Call(){ UnityEngine.Debug.Log("NormalBindDemoClass.Call();"); }

	}

	private void NormalBindDemo(IContainer container){

		container.Bind<NormalBindDemoClass>().Resolving((app , bind, obj)=>{
			UnityEngine.Debug.Log(obj);
			UnityEngine.Debug.Log("(Local) Container.Resolving() , " + obj.GetType());
			return obj;	

		}).Alias("normal-bind-demo");

		NormalBindDemoClass cls = container.Make("normal-bind-demo") as NormalBindDemoClass;
		//NormalBindDemoClass cls = container.Make<NormalBindDemoClass>();
		//NormalBindDemoClass cls = container["normal-bind-demo"] as NormalBindDemoClass;

		cls.Call();

	}

}

}