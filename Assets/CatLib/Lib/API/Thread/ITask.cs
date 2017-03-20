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

namespace CatLib.API.Thread
{

    public interface ITask
    {
        ITask Delay(float time);

        ITask OnComplete(Action onComplete);

        ITask OnComplete(Action<object> onComplete);

        ITask OnError(Action<System.Exception> onError);

        ITaskHandler Start();

    }

}