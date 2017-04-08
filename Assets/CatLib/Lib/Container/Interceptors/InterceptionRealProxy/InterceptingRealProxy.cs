
using System;
using System.Collections.ObjectModel;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using CatLib.API.Container;

namespace CatLib.Container{

    /// <summary>
    /// 拦截动态代理
    /// </summary>
	public class InterceptingRealProxy : RealProxy
    {

		private readonly object target;
		
		private string typeName;

		public InterceptingRealProxy(object target, Type classToProxy)
            : base(classToProxy)
        {
            this.target = target;
            typeName = target.GetType().FullName;
        }

        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="interceptor"></param>
        public void AddInterception(IInterception interceptor)
        {
            if(interceptor == null)
            {
                throw new ArgumentNullException("interceptor", "can not be null");
            }
            //this.interceptorsPipeline.Add(interceptor);
        }
        
        /// <summary>
        /// 代理调用时
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
		public override IMessage Invoke(IMessage msg)
        {
			IMethodCallMessage callMessage = (IMethodCallMessage)msg;
			UnityEngine.Debug.Log("before Invoke");
            var ret = ((IMethodCallMessage)msg).MethodBase.Invoke(target, callMessage.InArgs);
            UnityEngine.Debug.Log("after Invoke");
            return new ReturnMessage(ret, null, 0, null, null);
		}

	}

}