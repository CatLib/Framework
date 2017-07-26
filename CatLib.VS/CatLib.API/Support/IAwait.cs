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

namespace CatLib
{
    /// <summary>
    /// 等待接口
    /// </summary>
    /// <typeparam name="TInterface">目标接口</typeparam>
    public interface IAwait<TInterface>
    {
        /// <summary>
        /// 等待接口完成，当接口完成后触发callback
        /// </summary>
        /// <param name="callback">回调</param>
        void Await(Action<TInterface> callback);
    }
}
