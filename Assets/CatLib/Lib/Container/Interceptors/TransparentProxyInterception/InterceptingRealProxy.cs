
using System;
using System.Collections.ObjectModel;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace CatLib.Container{

    /// <summary>
    /// 拦截动态代理
    /// </summary>
	public class InterceptingRealProxy : RealProxy, IRemotingTypeInfo , IInterceptingProxy
    {

		private readonly object target;
		
		private readonly ReadOnlyCollection<Type> additionalInterfaces;
		
		private string typeName;

		public InterceptingRealProxy(object target, Type classToProxy, params Type[] additionalInterfaces)
            : base(classToProxy)
        {
            this.target = target;
            typeName = target.GetType().FullName;
			this.additionalInterfaces = new ReadOnlyCollection<Type>(additionalInterfaces);
        }

        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="interceptor"></param>
        public void Interception(IInterceptionBehavior interceptor)
        {
            if(interceptor == null)
            {
                throw new ArgumentNullException("interceptor", "can not be null");
            }
            //this.interceptorsPipeline.Add(interceptor);
        }

        /// <summary>
        /// 是否可以转换到
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="o"></param>
        /// <returns></returns>
		public bool CanCastTo(Type fromType, object o)
        {
            if (fromType.IsAssignableFrom(o.GetType()))
            {
                return true;
            }

            foreach (Type @interface in additionalInterfaces)
            {
                if (fromType.IsAssignableFrom(@interface))
                {
                    return true;
                }
            }

            return false;
        }

		/// <summary>
        /// 类型名
        /// </summary>
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
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