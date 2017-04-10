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
using System.Collections.Generic;

namespace CatLib.Container
{
    /// <summary>
    /// 拦截器管道
    /// </summary>
    internal class InterceptionPipeline
    {
        /// <summary>
        /// 拦截器列表
        /// </summary>
        private readonly List<IInterception> interceptionBehaviors;

        /// <summary>
        /// 调用计数堆栈
        /// </summary>
        private readonly Stack<int> stack;

        /// <summary>
        /// 构造一个拦截器管道
        /// </summary>
        public InterceptionPipeline()
        {
            interceptionBehaviors = new List<IInterception>();
            stack = new Stack<int>();
        }

        /// <summary>
        /// 执行管道
        /// </summary>
        /// <param name="methodInvoke">方法调用信息</param>
        /// <param name="then">当执行完成后调用</param>
        /// <returns>执行结果</returns>
        public object Do(IMethodInvoke methodInvoke, Func<object> then)
        {
            if (interceptionBehaviors.Count <= 0)
            {
                return then.Invoke();
            }

            stack.Push(-1);

            var ret = WhileToCanCall(methodInvoke, then);

            stack.Pop();

            return ret;
        }

        /// <summary>
        /// 下一步操作
        /// </summary>
        /// <param name="methodInvoke">方法调用信息</param>
        /// <param name="then">当执行完成后调用</param>
        /// <returns>下一步操作委托</returns>
        private Func<object> NextWrapper(IMethodInvoke methodInvoke, Func<object> then)
        {
            return () => WhileToCanCall(methodInvoke, then);
        }

        /// <summary>
        /// 循环到第一个可以调用的拦截器并调用
        /// </summary>
        /// <param name="methodInvoke">方法调用信息</param>
        /// <param name="then">当执行完成后调用</param>
        /// <returns>调用结果</returns>
        private object WhileToCanCall(IMethodInvoke methodInvoke, Func<object> then)
        {
            int index;
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
        /// 拦截器是否是生效的
        /// </summary>
        /// <param name="methodInvoke">方法调用信息</param>
        /// <param name="interception">拦截器</param>
        /// <returns>返回一个bool表示拦截器是否生效</returns>
        private bool IsEnable(IMethodInvoke methodInvoke, IInterception interception)
        {
            if (!interception.Enable) { return false; }

            foreach (var ret in interception.GetRequiredAttr())
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
        /// <param name="interceptor">拦截器</param>
        public void Add(IInterception interceptor)
        {
            interceptionBehaviors.Add(interceptor);
        }
    }
}