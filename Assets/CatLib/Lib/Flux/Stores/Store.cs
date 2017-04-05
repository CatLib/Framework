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

        protected Store(IFluxDispatcher dispatcher) : base(dispatcher)
        {

        }

        protected override IAction StoreAction()
        {
            return new FluxAction(ActionName);
        }

    }

}