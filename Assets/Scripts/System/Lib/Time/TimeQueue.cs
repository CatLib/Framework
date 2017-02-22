
using CatLib.API;
using CatLib.API.Time;
using System;
using System.Collections.Generic;

namespace CatLib.Time
{

    public class TimeQueue : Component , ITimeQueue , ITimeRunner
    {

        private Action queueOnComplete;
        private Action<object> queueOnCompleteWithContext;
        private object context;

        private List<TimeTask> queueTasks = new List<TimeTask>();
        public bool IsComplete { get; set; }

        public ITimeTaskHandler Push(TimeTask task)
        {
            queueTasks.Add(task);
            return task;
        }

        public void Cancel(TimeTask task)
        {
            queueTasks.Remove(task);
        }

        public ITimeTask Task(Action task)
        {
            TimeTask timeTask = new TimeTask(this)
            {
                ActionTask = task
            };
            return timeTask;
        }

        public ITimeTask Task(Action<object> task)
        {
            TimeTask timeTask = new TimeTask(this)
            {
                ActionTaskWithContext = task
            };
            return timeTask;
        }

        public ITimeQueue OnComplete(Action<object> onComplete)
        {
            queueOnCompleteWithContext = onComplete;
            return this;
        }

        public ITimeQueue OnComplete(Action onComplete)
        {
            queueOnComplete = onComplete;
            return this;
        }

        public ITimeQueue SetContext(object context)
        {
            this.context = context;
            return this;
        }

        public bool Pause()
        {
            return App.Time.StopRunner(this);
        }

        public bool Play()
        {
            IsComplete = false;
            return App.Time.Runner(this);
        }

        public bool Stop()
        {
            bool statu = App.Time.StopRunner(this);
            if (statu)
            {
                Reset();
            }
            return statu;
        }

        public bool Replay()
        {
            var statu = Stop();
            if (statu)
            {
                statu = Play();
            }
            return statu;
        }

        private void Reset()
        {
            TimeTask task;
            for (int i = 0; i < queueTasks.Count; ++i)
            {
                task = queueTasks[i];
                task.IsComplete = false;
                task.WaitDelayTime = 0;
                task.WaitLoopTime = 0;
            }
        }

        public void Update()
        {
            bool isAllComplete = true;
            for (int i = 0; i < queueTasks.Count; ++i)
            {
                var task = queueTasks[i];
                if (task.IsComplete) { continue; }

                isAllComplete = false;

                if (task.DelayTime > 0 && task.WaitDelayTime < task.DelayTime)
                {
                    task.WaitDelayTime += App.Time.DeltaTime;
                    break;
                }

                if (task.loopStatusFunc == null)
                {
                    if (task.LoopTime > 0 && task.WaitLoopTime < task.LoopTime)
                    {
                        if (task.ActionTask != null)
                        {
                            task.ActionTask();
                        }
                        if (task.ActionTaskWithContext != null)
                        {
                            task.ActionTaskWithContext(context);
                        }
                        task.WaitLoopTime += App.Time.DeltaTime;
                        break;
                    }
                }else
                {
                    if (task.loopStatusFunc.Invoke())
                    {
                        if (task.ActionTask != null)
                        {
                            task.ActionTask();
                        }
                        if (task.ActionTaskWithContext != null)
                        {
                            task.ActionTaskWithContext(context);
                        }
                        break;
                    }
                }

                task.IsComplete = true;
                if (task.TaskOnComplete != null)
                {
                    task.TaskOnComplete.Invoke();
                }
                if (task.TaskOnCompleteWithContext != null)
                {
                    task.TaskOnCompleteWithContext.Invoke(context);
                }
            }

            if (isAllComplete)
            {
                if(queueOnComplete != null)
                {
                    queueOnComplete.Invoke();
                }
                if(queueOnCompleteWithContext != null)
                {
                    queueOnCompleteWithContext.Invoke(context);
                }
                IsComplete = true;
            }
        }

    }

}