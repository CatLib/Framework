﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

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
        /// 函数的参数（不包含输出参数）
        /// </summary>
        IParameters Inputs { get; }

        /// <summary>
        /// 函数的参数（包含输出out参数）
        /// </summary>
        IParameters Arguments { get; }

        /// <summary>
        /// 服务实例(代理中的原始对象)
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