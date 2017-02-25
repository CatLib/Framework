
using CatLib.API.Resources;
using UnityEditor;

namespace CatLib.Resources{

	public class CompleteStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Complete; } }

		public void Build(IBuildContext context){

			AssetDatabase.Refresh();

		}



	}

}