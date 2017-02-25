
using CatLib.API.ResourcesSystem;
using UnityEditor;

namespace CatLib.ResourcesSystem{

	public class CompleteStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Complete; } }

		public void Build(IBuildContext context){

			AssetDatabase.Refresh();

		}



	}

}