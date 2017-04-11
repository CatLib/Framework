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

using CatLib.API.Container;

namespace CatLib.Container
{
    /// <summary>
    /// 拦截代理
    /// 用于增加拦截操作
    /// </summary>
    internal interface IInterceptingProxy
    {
        /// <summary>
        /// 增加一个拦截器
        /// </summary>
        /// <param name="interceptor">拦截器</param>
        void AddInterception(IInterception interceptor);

        /// <summary>
        /// 获取透明代理
        /// </summary>
        /// <returns>透明代理实例</returns>
        object GetTransparentProxy();
    }
}