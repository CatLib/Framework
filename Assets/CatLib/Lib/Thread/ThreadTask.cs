using CatLib.API.Thread;
using System;

namespace CatLib.Thread
{

    public class ThreadTask : ITask , ITaskHandler
    {

        private ThreadRuner runner;

        public Func<object> TaskWithResult { get; set; }

        public Action Task { get; set; }

        public Action<object> CompleteWithResult { get; private set; }

        public Action Complete { get; private set; }

        public Action<Exception> Error { get; private set; }

        public bool ReturnResult { get; set; }

        public float DelayTime { get; private set; }

        public float StartTime { get; set; }

        public ThreadTask(ThreadRuner runner)
        {
            this.runner = runner;
        }

        public ITask OnComplete(Action onComplete)
        {
            Complete = onComplete;
            return this;
        }

        public ITask OnComplete(Action<object> onComplete)
        {
            CompleteWithResult = onComplete;
            return this;
        }

        public ITask Delay(float time)
        {
            DelayTime = time;
            return this;
        }

        public ITask OnError(Action<Exception> onError)
        {
            Error = onError;
            return this;
        }

        public void Cancel(){
            
            runner.Cancel(this);

        }

        public ITaskHandler Start()
        {
            return runner.AddTask(this);
        }

    }

}