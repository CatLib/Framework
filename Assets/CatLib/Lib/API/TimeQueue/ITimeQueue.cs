
using System;

namespace CatLib.API.TimeQueue
{

    /// <summary>
    /// 时间队列
    /// </summary>
    public interface ITimeQueue
    {

        ITimeTask Task(Action task);

        ITimeTask Task(Action<object> task);

        ITimeQueue OnComplete(Action<object> onComplete);

        ITimeQueue OnComplete(Action onComplete);

        ITimeQueue SetContext(object context);

        bool Pause();

        bool Play();

        bool Stop();

        bool Replay();

    }

}