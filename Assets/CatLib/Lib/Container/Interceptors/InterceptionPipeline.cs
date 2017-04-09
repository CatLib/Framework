
using System;
using CatLib.API.Container;
using System.Collections.Generic;

namespace CatLib.Container
{

    /// <summary>
    /// 拦截器管道
    /// </summary>
    public class InterceptionPipeline
    {

        private readonly List<IInterception> interceptionBehaviors;
        private Stack<int> stack;

        public InterceptionPipeline()
        {
            interceptionBehaviors = new List<IInterception>();
            stack = new Stack<int>();
        }

        /// <summary>
        /// 执行管道
        /// </summary>
        /// <param name="methodInvoke"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        public object Do(IMethodInvoke methodInvoke , Func<object> then)
        {

            if (interceptionBehaviors.Count <= 0)
            {
                return then.Invoke();
            }

            stack.Push(0);

            object ret = interceptionBehaviors[0].Interception(methodInvoke, Next(methodInvoke , then));

            stack.Pop();

            return ret;

        }

        /// <summary>
        /// 下一步操作
        /// </summary>
        /// <param name="methodInvoke"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        private Func<object> Next(IMethodInvoke methodInvoke, Func<object> then)
        {
            return () =>
            {
                int index = stack.Pop();
                stack.Push(++index);
                if (index >= interceptionBehaviors.Count)
                {
                    return then.Invoke();
                }
                return interceptionBehaviors[index].Interception(methodInvoke, Next(methodInvoke, then));
            };
        }

        /// <summary>
        /// 增加一个拦截器
        /// </summary>
        /// <param name="interceptor"></param>
        public void Add(IInterception interceptor)
        {
            interceptionBehaviors.Add(interceptor);
        }

    }

}