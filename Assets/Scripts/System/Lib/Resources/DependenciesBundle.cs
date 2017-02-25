
using UnityEngine;

namespace CatLib.Resources{

	public class DependenciesBundle {

		private int refCount = 0;

		public int RefCount{ get { return RefCount; } set{ refCount = value; } }
		
    	public UnityEngine.AssetBundle Bundle{ get; set; }

		public DependenciesBundle(UnityEngine.AssetBundle assetBundle){

			Bundle = assetBundle;
			refCount = 1;

		}

	}

}