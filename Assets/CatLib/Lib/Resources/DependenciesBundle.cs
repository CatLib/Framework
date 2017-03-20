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