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

            if (target == null) { return null; }

            IInterception[] interceptors = bindData.GetInterceptors();
            if (interceptors == null) { return target; }

            IInterceptingProxy proxy = null;
            if (target is MarshalByRefObject) {

                if(target.GetType().IsDefined(typeof(AOPAttribute) , false))
                {
                    proxy = CreateRealProxy(interceptors, target);
                }

            }

            if (proxy != null)
            {
                AddInterceptions(proxy, interceptors);
                return proxy.GetTransparentProxy();
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
        public IInterceptingProxy CreateRealProxy(IInterception[] interceotors, object target)
        {
            return new InterceptingRealProxy(target);
        }

        /// <summary>
        /// 增加拦截器
        /// </summary>
        /// <param name="proxy">代理</param>
        /// <param name="interceotors">要增加的拦截器</param>
        /// <returns></returns>
        private IInterceptingProxy AddInterceptions(IInterceptingProxy proxy , IInterception[] interceotors)
        {
            for (int i = 0; i < interceotors.Length; i++)
            {
                proxy.AddInterception(interceotors[i]);
            }
            return proxy;
        }

    }

}