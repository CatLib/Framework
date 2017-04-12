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
using System.Collections.Generic;
using CatLib.API.Container;

namespace CatLib.Container
{
    /// <summary>
    /// 代理
    /// </summary>
    internal sealed class BoundProxy : IBoundProxy
    {
        /// <summary>
        /// 创建代理类
        /// </summary>
        /// <param name="target">服务实例</param>
        /// <param name="bindData">服务绑定数据</param>
        /// <returns>如果构建成功则返回服务透明代理实例，否则返回服务实例</returns>
        public object Bound(object target, BindData bindData)
        {
            if (target == null)
            {
                return null;
            }

            var interceptors = bindData.GetInterceptors();
            if (interceptors == null)
            {
                return target;
            }

            IInterceptingProxy proxy = null;
            if (target is MarshalByRefObject)
            {
                if (target.GetType().IsDefined(typeof(AOPAttribute), false))
                {
                    proxy = CreateRealProxy(target);
                }
            }

            if (proxy == null)
            {
                return target;
            }

            AddInterceptions(proxy, interceptors);
            return proxy.GetTransparentProxy();
        }

        /// <summary>
        /// 创建动态代理
        /// </summary>
        /// <param name="target">服务实例</param>
        /// <returns>动态代理</returns>
        private IInterceptingProxy CreateRealProxy(object target)
        {
            return new InterceptingRealProxy(target);
        }

        /// <summary>
        /// 增加拦截器
        /// </summary>
        /// <param name="proxy">动态代理</param>
        /// <param name="interceotors">要增加的拦截器</param>
        private void AddInterceptions(IInterceptingProxy proxy, IList<IInterception> interceotors)
        {
            for (var i = 0; i < interceotors.Count; i++)
            {
                proxy.AddInterception(interceotors[i]);
            }
        }
    }

}