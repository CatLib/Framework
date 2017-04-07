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

namespace CatLib.Container{

    class BoundProxy : IBoundProxy{

        public object Bound(object target , BindData bindData){

            UnityEngine.Debug.Log("Bound Proxy");
            return target;

        }

    }

}