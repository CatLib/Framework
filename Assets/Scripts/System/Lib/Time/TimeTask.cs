using CatLib.Contracts.Time;
using System;

namespace CatLib.Time
{

    public class TimeTask : ITimeTask , ITimeTaskHandler
    {

        private TimeQueue queue;

        public Action ActionTask { get; set; }

        public Action<object> ActionTaskWithContext { get; set; }

        public float DelayTime { get; private set; }

        public float WaitDelayTime { get; set; }

        public float LoopTime { get; private set; }

        public float WaitLoopTime { get; set; }

        public Action TaskOnComplete { get; set; }

        public Action<object> TaskOnCompleteWithContext { get; set; }

        public Func<bool> loopStatusFunc { get; set; }

        public bool IsComplete { get; set; }

        private Func<float> loopDurationFunc;

        public TimeTask(TimeQueue queue)
        {
            this.queue = queue;
        }

        public ITimeTask Delay(float time)
        {
            DelayTime = time;
            return this;
        }

        public ITimeTask Loop(float time)
        {
            LoopTime = time;
            return this;
        }

        public ITimeTask Loop(Func<float> loopFunc)
        {
            loopDurationFunc = loopFunc;
            return this;
        }

        public ITimeTask Loop(Func<bool> loopFunc)
        {
            loopStatusFunc = loopFunc;
            return this;
        }

        public ITimeTask OnComplete(Action onComplete)
        {
            TaskOnComplete = onComplete;
            return this;
        }

        public ITimeTask OnComplete(Action<object> onComplete)
        {
            TaskOnCompleteWithContext = onComplete;
            return this;
        }

        public ITimeTask Task(Action task)
        {
            Push();
            return queue.Task(task);
        }

        public ITimeTask Task(Action<object> task)
        {
            Push();
            return queue.Task(task);
        }

        public ITimeTaskHandler Push()
        {
            if(loopDurationFunc != null)
            {
                LoopTime = loopDurationFunc.Invoke();
            }
            return queue.Push(this);
        }

        public void Cancel()
        {
            queue.Cancel(this);
        }
    }

}