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

namespace CatLib.API.Flux
{

    public interface IFluxDispatcher
    {

        bool IsDispatching { get; }

        string On(Action<IAction> action);

        void On(string token, Action<IAction> action);

        void Off(string token);

        void WaitFor(string key, IAction payload);

        void Dispatch(string token, IAction payload);

        void Dispatch(IAction payload);

    }

}