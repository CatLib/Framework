
using System;

namespace CatLib.API.TimeQueue
{

    /// <summary>
    /// 时间任务
    /// </summary>
    public interface ITimeTask
    {

        ITimeTask Delay(float time);

        ITimeTask DelayFrame(int frame);

        ITimeTask Loop(float time);

        ITimeTask Loop(Func<bool> loopFunc);

        ITimeTask LoopFrame(int frame);

        ITimeTask OnComplete(Action onComplete);

        ITimeTask OnComplete(Action<object> onComplete);

        ITimeTask Task(Action task);

        ITimeTask Task(Action<object> task);

        ITimeTaskHandler Push();

        ITimeQueue Play();

    }

}