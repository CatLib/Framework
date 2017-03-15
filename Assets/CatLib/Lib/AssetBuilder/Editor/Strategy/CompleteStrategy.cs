
using CatLib.API.Resources;
using CatLib.API.AssetBuilder;
using UnityEditor;

namespace CatLib.AssetBuilder
{

	public class CompleteStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Complete; } }

		public void Build(IBuildContext context){

			AssetDatabase.Refresh();

		}



	}

}