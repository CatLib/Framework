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
    /// �����װ��
    /// </summary>
    internal interface IBoundProxy
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="target">����ʵ��</param>
        /// <param name="bindData">������</param>
        /// <returns></returns>
        object Bound(object target, BindData bindData);
    }
}