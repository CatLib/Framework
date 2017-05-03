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
    /// �����װ��
    /// </summary>
    internal sealed class BoundProxy : IBoundProxy
    {
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="target">����ʵ��</param>
        /// <param name="bindData">���������</param>
        /// <returns>��������ɹ��򷵻ط���͸������ʵ�������򷵻ط���ʵ��</returns>
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
                if (target.GetType().IsDefined(typeof(AopAttribute), false))
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
        /// ������̬����
        /// </summary>
        /// <param name="target">����ʵ��</param>
        /// <returns>��̬����</returns>
        private IInterceptingProxy CreateRealProxy(object target)
        {
            return new InterceptingRealProxy(target);
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="proxy">��̬����</param>
        /// <param name="interceotors">Ҫ���ӵ�������</param>
        private void AddInterceptions(IInterceptingProxy proxy, IList<IInterception> interceotors)
        {
            for (var i = 0; i < interceotors.Count; i++)
            {
                proxy.AddInterception(interceotors[i]);
            }
        }
    }

}