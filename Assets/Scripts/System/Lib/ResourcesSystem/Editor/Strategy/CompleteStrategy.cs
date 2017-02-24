
using CatLib.API.ResourcesSystem;
using UnityEditor;
using UnityEngine;
using CatLib;
using System;
using CatLib.API.IO;

namespace CatLib.ResourcesSystem{

	public class CompleteStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Complete; } }

		public void Build(IBuildContext context){

			AssetDatabase.Refresh();

		}



	}

}