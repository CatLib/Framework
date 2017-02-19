
using CatLib.Contracts.Thread;
using System.Threading;

namespace CatLib.Thread
{

    /// <summary>
    /// 多线程运行器
    /// </summary>
    public class ThreadRuner : IThread
    {

        public void Task(System.Action task)
        {
            Task(task, null);
        }

        public void Task(System.Action task, System.Action onComplete)
        {
            Task(task, onComplete, null);
        }

        public void Task(System.Action task, System.Action onComplete, System.Action<System.Exception> onError)
        {
            var taskRunner = new TaskRunner
            {
                OnComplete = onComplete,
                OnError = onError,
                Task = task,
                ReturnResult = false,
            };

            AddTask(taskRunner);
        }


        public void Task(System.Func<object> task)
        {
            Task(task, null);
        }

        public void Task(System.Func<object> task, System.Action<object> onComplete)
        {
            Task(task, onComplete, null);
        }

        public void Task(System.Func<object> task, System.Action<object> onComplete, System.Action<System.Exception> onError)
        {

            var taskRunner = new TaskRunner
            {
                OnCompleteWithResult = onComplete,
                OnError = onError,
                TaskWithResult = task,
                ReturnResult = true,
            };

            AddTask(taskRunner);

        }

        private void AddTask(TaskRunner taskRunner)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecuter), taskRunner);
        }

        private void ThreadExecuter(object state)
        {
            try
            {
                if (typeof(TaskRunner) == state.GetType()) { RunTaskThread((TaskRunner)state); }
                else
                {
                    App.Instance.Trigger(ThreadEvents.ON_THREAD_EXECURE_ERROR, 
                                                new ErrorEventArgs(
                                                    new System.Exception(string.Format("type '{0}' not supported!", state.GetType())
                                                )));
                }
            }
            catch (System.Exception exception)
            {
                App.Instance.Trigger(ThreadEvents.ON_THREAD_EXECURE_ERROR, new ErrorEventArgs(exception));
            }
        }

        private void RunTaskThread(TaskRunner taskRunner)
        {
            try
            {
                if (taskRunner.ReturnResult)
                {
                    var result = taskRunner.TaskWithResult();
                    if (taskRunner.OnCompleteWithResult != null)
                    {
                        App.Instance.MainThread(() =>
                        {
                            taskRunner.OnCompleteWithResult(result);
                        });
                    }
                }
                else
                {
                    taskRunner.Task();
                    if (taskRunner.OnComplete != null)
                    {
                        App.Instance.MainThread(() =>
                        {
                            taskRunner.OnComplete();
                        });
                    }
                }

            }
            catch (System.Exception exception)
            {
                if (taskRunner.OnError != null)
                {
                    App.Instance.MainThread(() =>
                    {
                        taskRunner.OnError(exception);
                    });
                }
            }
            finally
            {
                taskRunner.IsComplete = true;
            }
        }

    }


}