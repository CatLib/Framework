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

namespace CatLib.Container
{
    /// <summary>
    /// 代理包装器
    /// </summary>
    internal interface IBoundProxy
    {
        /// <summary>
        /// 构建代理
        /// </summary>
        /// <param name="target">服务实例</param>
        /// <param name="bindData">绑定数据</param>
        /// <returns></returns>
        object Bound(object target, BindData bindData);
    }
}