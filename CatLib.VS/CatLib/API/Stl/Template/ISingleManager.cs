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

namespace CatLib.API.Stl
{
    /// <summary>
    /// 管理器
    /// </summary>
    public interface ISingleManager<TInterface> : IManager<TInterface>
    {
        /// <summary>
        /// 获取默认的解决方案
        /// </summary>
        TInterface Default { get; }

        /// <summary>
        /// 释放解决方案
        /// </summary>
        /// <param name="name">解决方案名</param>
        void Release(string name = null);
    }
}
