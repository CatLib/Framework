
using System.Reflection;
using System.Collections.Generic;

namespace CatLib.API.Container
{

    /// <summary>
    /// 函数调用
    /// </summary>
    public interface IMethodInvoke
    {

        /// <summary>
        /// 方法函数的参数（不包含out参数）
        /// </summary>
        IParameters Inputs { get; }

        /// <summary>
        /// 方法函数的参数（包含out参数）
        /// </summary>
        IParameters Arguments { get; }

        /// <summary>
        /// 目标对象(代理中的原始对象)
        /// </summary>
        object Target { get; }

        /// <summary>
        /// 基础方法
        /// </summary>
        MethodBase MethodBase { get; }

        /// <summary>
        /// 上下文（用于aop方法间额外的参数传递）
        /// </summary>
        IDictionary<string, object> Context { get; }

    }

}