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

using System;
using System.Runtime.Remoting.Proxies;

namespace CatLib.Container{

    class BoundProxy : IBoundProxy{

        public object Bound(object target , BindData bindData){

            if(!(target is MarshalByRefObject)) { return target; }
            RealProxy proxy = CreateProxy(target.GetType(), target);
            return proxy.GetTransparentProxy();

        }

        public RealProxy CreateProxy(Type t, object target, params Type[] additionalInterfaces)
        {
            RealProxy realProxy = new InterceptingRealProxy(target, t, additionalInterfaces);
            return realProxy;
        }

    }

}