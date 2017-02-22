
using System;

namespace CatLib.API.Thread
{

    public interface ITask
    {

        ITask Name(string name);

        ITask Delay(float time);

        ITask OnComplete(Action onComplete);

        ITask OnComplete(Action<object> onComplete);

        ITask OnError(Action<Exception> onError);

        void Start();

    }

}