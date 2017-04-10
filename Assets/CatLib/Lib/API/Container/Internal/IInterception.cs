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
        /// 是否生效
        /// </summary>
        bool Enable { get; }

        /// <summary>
        /// 必须的属性类型
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetRequiredAttr();

        /// <summary>
        /// 拦截器
        /// </summary>
        /// <param name="methodInvoke"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        object Interception(IMethodInvoke methodInvoke, Func<object> next);

    }

}