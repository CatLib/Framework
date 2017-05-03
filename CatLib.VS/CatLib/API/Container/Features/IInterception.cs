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
using System.Collections.Generic;

namespace CatLib.API.Container
{
    /// <summary>
    /// 拦截器脚本
    /// </summary>
    public interface IInterception
    {
        /// <summary>
        /// 拦截器是否生效
        /// </summary>
        bool Enable { get; }

        /// <summary>
        /// 必须的属性类型才会被拦截
        /// </summary>
        /// <returns>属性列表</returns>
        IEnumerable<Type> GetRequiredAttr();

        /// <summary>
        /// 拦截器方案
        /// </summary>
        /// <param name="methodInvoke">方法调用</param>
        /// <param name="next">下一个拦截器</param>
        /// <returns>拦截返回值</returns>
        object Interception(IMethodInvoke methodInvoke, Func<object> next);
    }
}