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

using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System.Collections.Generic;
using CatLib.API.Container;

namespace CatLib.Container
{

    /// <summary>
    /// 函数调用
    /// </summary>
    public class MethodInvoke : IMethodInvoke
    {

        /// <summary>
        /// 方法调用消息
        /// </summary>
        private IMethodCallMessage callMessage;

        /// <summary>
        /// 代理目标
        /// </summary>
        private object target;

        /// <summary>
        /// 调用参数
        /// </summary>
        private object[] arguments;

        /// <summary>
        /// 上下文（用于aop方法间额外的参数传递）
        /// </summary>
        private IDictionary<string, object> context;

        /// <summary>
        /// 方法函数的参数
        /// </summary>
        private ParameterCollection allParams;

        /// <summary>
        /// 输入的参数
        /// </summary>
        private ParameterCollection inputParams;

        /// <summary>
        /// 方法调用
        /// </summary>
        /// <param name="callMessage"></param>
        /// <param name="target"></param>
        public MethodInvoke(IMethodCallMessage callMessage, object target)
        {
            this.callMessage = callMessage;
            context = new Dictionary<string, object>();
            this.target = target;
            arguments = callMessage.Args;
            inputParams = new ParameterCollection(arguments , callMessage.MethodBase.GetParameters() , info => !info.IsOut);
            allParams = new ParameterCollection(arguments, callMessage.MethodBase.GetParameters() , info => true);
        }

        /// <summary>
        /// 方法函数的参数（不包含输出参数）
        /// </summary>
        public IParameters Inputs { get { return inputParams; } }

        /// <summary>
        /// 方法函数的参数（包含输出参数）
        /// </summary>
        IParameters IMethodInvoke.Arguments { get { return allParams; } }

        /// <summary>
        /// 目标对象(代理中的原始对象)
        /// </summary>
        public object Target { get { return target; } }

        /// <summary>
        /// 基础方法
        /// </summary>
        public MethodBase MethodBase { get { return callMessage.MethodBase; } }

        /// <summary>
        /// 上下文（用于aop方法间额外的参数传递）
        /// </summary>
        public IDictionary<string, object> Context{ get { return context; } }

        /// <summary>
        /// 方法函数的参数（包含输出参数）
        /// </summary>
        internal object[] Arguments
        {
            get { return arguments; }
        }

    }

}