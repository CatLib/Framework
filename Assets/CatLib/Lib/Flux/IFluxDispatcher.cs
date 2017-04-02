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

    public interface IFluxDispatcher
    {

        bool IsDispatching { get; }

        string On(Action<INotification> action);

        void On(string token, Action<INotification> action);

        void Off(string token);

        void WaitFor(string key, INotification payload);

        void Dispatch(string token, INotification payload);

        void Dispatch(INotification payload);

    }

}