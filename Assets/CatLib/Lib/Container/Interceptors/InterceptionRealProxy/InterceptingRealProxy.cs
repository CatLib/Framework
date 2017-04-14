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
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using CatLib.API.Container;

namespace CatLib.Container
{
    /// <summary>
    /// 拦截动态代理
    /// </summary>
    public sealed class InterceptingRealProxy : RealProxy, IInterceptingProxy
    {
        /// <summary>
        /// 拦截器管道
        /// </summary>
        private readonly InterceptionPipeline interceptors;

        /// <summary>
        /// 代理对象
        /// </summary>
        private readonly object target;

        /// <summary>
        /// 构建一个动态代理
        /// </summary>
        /// <param name="target">代理的原始对象</param>
        public InterceptingRealProxy(object target)
            : base(target.GetType())
        {
            this.target = target;
            interceptors = new InterceptionPipeline();
        }

        /// <summary>
        /// 增加拦截
        /// </summary>
        /// <param name="interceptor">拦截器实例</param>
        public void AddInterception(IInterception interceptor)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException("interceptor", "can not be null");
            }
            interceptors.Add(interceptor);
        }

        /// <summary>
        /// 代理调用时
        /// </summary>
        /// <param name="msg">函数调用消息</param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMessage = (IMethodCallMessage)msg;

            if (!callMessage.MethodBase.IsDefined(typeof(AopAttribute), false))
            {
                return new ReturnMessage(callMessage.MethodBase.Invoke(target, callMessage.Args),
                                        callMessage.Args,
                                        callMessage.Args.Length,
                                        callMessage.LogicalCallContext,
                                        callMessage);
            }

            MethodInvoke methodInvoke = new MethodInvoke(callMessage, target);

            try
            {
                var ret = interceptors.Do(methodInvoke, () => ((IMethodCallMessage)msg).MethodBase.Invoke(methodInvoke.Target, methodInvoke.Arguments));

                return new ReturnMessage(ret,
                                    methodInvoke.Arguments,
                                    methodInvoke.Arguments.Length,
                                    callMessage.LogicalCallContext,
                                    callMessage);
            }
            catch (Exception ex)
            {
                return new ReturnMessage(ex, callMessage);
            }
        }
    }
}