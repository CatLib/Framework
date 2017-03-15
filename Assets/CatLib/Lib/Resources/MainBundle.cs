
using UnityEngine;

namespace CatLib.Resources{

	public class MainBundle {

        /// <summary>
        /// 资源包
        /// </summary>
		public AssetBundle Bundle{ get; set; }

        public MainBundle(AssetBundle bundle){

			Bundle = bundle;

        }


    }

}