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

namespace CatLib.Flux
{

    public interface IFluxDispatcher<TPayload>
    {

        bool IsDispatching { get; }

        string On(Action<TPayload> action);

        void On(string token, Action<TPayload> action);

        void Off(string token);

        void WaitFor(string key, TPayload payload);

        void Dispatch(string token, TPayload payload);

        void Dispatch(TPayload payload);

    }

}