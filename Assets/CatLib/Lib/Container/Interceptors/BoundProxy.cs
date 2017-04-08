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
using CatLib.API.Container;

namespace CatLib.Container{

    class BoundProxy : IBoundProxy{

        /// <summary>
        /// 创建代理类
        /// </summary>
        /// <param name="target"></param>
        /// <param name="bindData"></param>
        /// <returns></returns>
        public object Bound(object target , BindData bindData){

            IInterception[] interceptors = bindData.GetInterceptors();
            if (interceptors == null) { return target; }

            if(target is MarshalByRefObject) {

                return CreateRealProxy(interceptors, target.GetType(), target);

            }

            return target;

        }

        /// <summary>
        /// 创建动态代理
        /// </summary>
        /// <param name="t"></param>
        /// <param name="target"></param>
        /// <param name="additionalInterfaces"></param>
        /// <returns></returns>
        public object CreateRealProxy(IInterception[] interceotors, Type t, object target)
        {

            InterceptingRealProxy realProxy = new InterceptingRealProxy(target, t);

            for(int i = 0; i < interceotors.Length; i++)
            {
                realProxy.AddInterception(interceotors[i]);
            }

            return realProxy.GetTransparentProxy();

        }

    }

}