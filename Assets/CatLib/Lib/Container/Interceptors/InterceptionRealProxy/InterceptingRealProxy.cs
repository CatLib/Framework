
using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using CatLib.API.Container;

namespace CatLib.Container{

    /// <summary>
    /// 拦截动态代理
    /// </summary>
	public class InterceptingRealProxy : RealProxy , IInterceptingProxy
    {

        /// <summary>
        /// 拦截器管道
        /// </summary>
        private InterceptionPipeline interceptors;

        /// <summary>
        /// 代理对象
        /// </summary>
		private readonly object target;
		
        /// <summary>
        /// 代理类型
        /// </summary>
		private string typeName;

        /// <summary>
        /// 构建一个动态代理
        /// </summary>
        /// <param name="target"></param>
		public InterceptingRealProxy(object target)
            : base(target.GetType())
        {
            this.target = target;
            typeName = target.GetType().FullName;
            interceptors = new InterceptionPipeline();
        }

        /// <summary>
        /// 增加拦截
        /// </summary>
        /// <param name="interceptor"></param>
        public void AddInterception(IInterception interceptor)
        {
            if(interceptor == null)
            {
                throw new ArgumentNullException("interceptor", "can not be null");
            }
            interceptors.Add(interceptor);
        }
        
        /// <summary>
        /// 代理调用时
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
		public override IMessage Invoke(IMessage msg)
        {
			IMethodCallMessage callMessage = (IMethodCallMessage)msg;

            if(!callMessage.MethodBase.IsDefined(typeof(AOPAttribute) , false))
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

                var ret = interceptors.Do(methodInvoke, () =>
                {

                    return ((IMethodCallMessage)msg).MethodBase.Invoke(methodInvoke.Target, methodInvoke.Arguments);

                });

                return new ReturnMessage(ret,
                                    methodInvoke.Arguments,
                                    methodInvoke.Arguments.Length,
                                    callMessage.LogicalCallContext,
                                    callMessage);

            }
            catch(Exception ex)
            {
                return new ReturnMessage(ex, callMessage);
            }

           
		}

	}

}