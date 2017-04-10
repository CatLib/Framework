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
    public interface IBoundProxy
    {
        /// <summary>
        /// 构建代理
        /// </summary>
        /// <param name="target"></param>
        /// <param name="bindData"></param>
        /// <returns></returns>
        object Bound(object target, BindData bindData);
    }
}