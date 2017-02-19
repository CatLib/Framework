using System;

namespace CatLib.Thread
{

    public class TaskRunner
    {

        public Func<object> TaskWithResult { get; set; }

        public Action Task { get; set; }

        public Action<object> OnCompleteWithResult { get; set; }

        public Action OnComplete { get; set; }

        public Action<Exception> OnError { get; set; }

        public bool ReturnResult { get; set; }

        public bool IsComplete { get; set; }

    }

}