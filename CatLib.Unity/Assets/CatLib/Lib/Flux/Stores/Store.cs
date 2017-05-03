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

using CatLib.API.Flux;

namespace CatLib.Flux
{
    /// <summary>
    /// 存储
    /// </summary>
    public class Store : FluxStore
    {
        /// <summary>
        /// 构建一个存储
        /// </summary>
        /// <param name="dispatcher">调度器</param>
        protected Store(IFluxDispatcher dispatcher) 
            : base(dispatcher)
        {
        }

        /// <summary>
        /// 构建一个存储行为
        /// </summary>
        /// <returns>行为</returns>
        protected override IAction StoreAction()
        {
            return new FluxAction(ActionName);
        }
    }
}