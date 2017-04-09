
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

            stack.Push(-1);

            object ret = WhileToCanCall(methodInvoke, then);

            stack.Pop();

            return ret;

        }

        /// <summary>
        /// 下一步操作
        /// </summary>
        /// <param name="methodInvoke"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        private Func<object> NextWrapper(IMethodInvoke methodInvoke, Func<object> then)
        {
            return () =>
            {
                return WhileToCanCall(methodInvoke, then);
            };
        }

        /// <summary>
        /// 循环到第一个可以调用的拦截器并调用
        /// </summary>
        /// <param name="methodInvoke"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        private object WhileToCanCall(IMethodInvoke methodInvoke, Func<object> then)
        {
            int index = 0;
            do
            {
                index = stack.Pop();
                stack.Push(++index);

                if (index >= interceptionBehaviors.Count)
                {
                    return then.Invoke();
                }

            } while (!IsEnable(methodInvoke, interceptionBehaviors[index]));

            return interceptionBehaviors[index].Interception(methodInvoke, NextWrapper(methodInvoke, then));
        }

        /// <summary>
        /// 是否是生效的
        /// </summary>
        /// <param name="interception"></param>
        /// <returns></returns>
        private bool IsEnable(IMethodInvoke methodInvoke, IInterception interception)
        {
            if (!interception.Enable) { return false; }

            foreach(var ret in interception.GetRequiredAttr())
            {
                if (!methodInvoke.MethodBase.IsDefined(ret, false))
                {
                    return false;
                }
            }

            return true;
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