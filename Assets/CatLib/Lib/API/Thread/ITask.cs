
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