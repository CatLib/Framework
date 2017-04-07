
using System;
using System.Collections.ObjectModel;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace CatLib.Container{

	public class InterceptingRealProxy : RealProxy, IRemotingTypeInfo{

		private readonly object target;
		
		private readonly ReadOnlyCollection<Type> additionalInterfaces;
		
		private string typeName;

		public InterceptingRealProxy(
            object target,
            Type classToProxy,
            params Type[] additionalInterfaces
            )
            : base(classToProxy)
        {
            this.target = target;
            typeName = target.GetType().FullName;
			this.additionalInterfaces = new ReadOnlyCollection<Type>(additionalInterfaces);
        }

		public bool CanCastTo(Type fromType, object o)
        {
            if (fromType.IsAssignableFrom(o.GetType()))
            {
                return true;
            }

            foreach (Type @interface in this.additionalInterfaces)
            {
                if (fromType.IsAssignableFrom(@interface))
                {
                    return true;
                }
            }

            return false;
        }

		
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

		public override IMessage Invoke(IMessage msg)
        {

			IMethodCallMessage callMessage = (IMethodCallMessage)msg;

			UnityEngine.Debug.Log("before Invoke");
            var ret = ((IMethodCallMessage)msg).MethodBase.Invoke(target, null);
            UnityEngine.Debug.Log("after Invoke");
            return new ReturnMessage(ret, null, 0, null, null);

		}

	}

}