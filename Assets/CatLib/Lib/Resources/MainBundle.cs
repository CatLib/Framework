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